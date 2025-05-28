#!/bin/bash
# Script to generate SSL/TLS certificates for gRPC services and Nginx
# This script should be run from the project root directory

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration from .env
PROJECT_PREFIX="estate-elite"
CA_COUNTRY="US"
CA_STATE="State"
CA_CITY="City"
CA_ORG="Estate Elite"
CA_ORG_UNIT="IT"

# Certificate validity (in days)
CA_VALIDITY_DAYS=3650  # 10 years for CA
CERT_VALIDITY_DAYS=365 # 1 year for service certificates

echo -e "${BLUE}Starting SSL/TLS Certificate Generation for Estate Elite...${NC}"

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Create directories for certificates
print_status "Creating certificate directories..."
mkdir -p ./services/identity/ssl
mkdir -p ./services/property/ssl
mkdir -p ./services/payment/ssl
mkdir -p ./nginx/ssl

# Generate CA key and certificate (Root CA)
print_status "Generating CA key and certificate..."
openssl genrsa -out ./nginx/ssl/ca.key 4096

openssl req -new -x509 -days $CA_VALIDITY_DAYS -key ./nginx/ssl/ca.key -out ./nginx/ssl/ca.crt \
    -subj "/C=$CA_COUNTRY/ST=$CA_STATE/L=$CA_CITY/O=$CA_ORG/OU=$CA_ORG_UNIT/CN=estate-elite-ca"

# Generate DH parameters for Nginx (for stronger security)
print_status "Generating DH parameters for Nginx (this may take a while)..."
openssl dhparam -out ./nginx/ssl/nginx.pem 2048

# Generate key and certificate for Nginx
print_status "Generating Nginx key and certificate..."
openssl genrsa -out ./nginx/ssl/nginx.key 2048

# Create Nginx certificate with SAN extension
cat > ./nginx/ssl/nginx.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
extendedKeyUsage = serverAuth
subjectAltName = @alt_names

[alt_names]
DNS.1 = localhost
DNS.2 = estate-elite.com
DNS.3 = www.estate-elite.com
DNS.4 = ${PROJECT_PREFIX}-nginx
IP.1 = 127.0.0.1
IP.2 = ::1
EOF

# Generate CSR for Nginx
openssl req -new -key ./nginx/ssl/nginx.key -out ./nginx/ssl/nginx.csr \
    -subj "/C=$CA_COUNTRY/ST=$CA_STATE/L=$CA_CITY/O=$CA_ORG/OU=$CA_ORG_UNIT/CN=estate-elite.com"

# Sign Nginx certificate
openssl x509 -req -days $CERT_VALIDITY_DAYS -in ./nginx/ssl/nginx.csr \
    -CA ./nginx/ssl/ca.crt -CAkey ./nginx/ssl/ca.key -CAcreateserial \
    -out ./nginx/ssl/nginx.crt -extensions v3_ext -extfile ./nginx/ssl/nginx.ext

# Function to generate service certificates
generate_service_cert() {
    local SERVICE_NAME=$1
    local SERVICE_DIR=$2
    local SERVICE_CN=$3
    local GRPC_PORT=$4
    local HTTP_PORT=$5
    local CERT_PASSWORD=$6

    print_status "Generating certificates for $SERVICE_NAME service..."
    
    # Generate private key
    openssl genrsa -out $SERVICE_DIR/ssl/$SERVICE_NAME.key 2048
    
    # Create service specific config for SAN extension
    cat > $SERVICE_DIR/ssl/$SERVICE_NAME.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
extendedKeyUsage = serverAuth, clientAuth
subjectAltName = @alt_names

[alt_names]
DNS.1 = $SERVICE_CN
DNS.2 = ${PROJECT_PREFIX}-$SERVICE_NAME-service-1
DNS.3 = ${PROJECT_PREFIX}-$SERVICE_NAME-service-2
DNS.4 = localhost
IP.1 = 127.0.0.1
IP.2 = ::1
EOF
    
    # Generate CSR
    openssl req -new -key $SERVICE_DIR/ssl/$SERVICE_NAME.key -out $SERVICE_DIR/ssl/$SERVICE_NAME.csr \
        -subj "/C=$CA_COUNTRY/ST=$CA_STATE/L=$CA_CITY/O=$CA_ORG/OU=$CA_ORG_UNIT/CN=$SERVICE_CN"
    
    # Sign the certificate
    openssl x509 -req -days $CERT_VALIDITY_DAYS -in $SERVICE_DIR/ssl/$SERVICE_NAME.csr \
        -CA ./nginx/ssl/ca.crt -CAkey ./nginx/ssl/ca.key -CAcreateserial \
        -out $SERVICE_DIR/ssl/$SERVICE_NAME.crt -extensions v3_ext -extfile $SERVICE_DIR/ssl/$SERVICE_NAME.ext
    
    # Create PKCS#12 file with password protection (for .NET services)
    openssl pkcs12 -export -out $SERVICE_DIR/ssl/$SERVICE_NAME.pfx \
        -inkey $SERVICE_DIR/ssl/$SERVICE_NAME.key \
        -in $SERVICE_DIR/ssl/$SERVICE_NAME.crt \
        -certfile ./nginx/ssl/ca.crt \
        -password pass:$CERT_PASSWORD
    
    # Create PEM bundle (contains both cert and key)
    cat $SERVICE_DIR/ssl/$SERVICE_NAME.crt $SERVICE_DIR/ssl/$SERVICE_NAME.key > $SERVICE_DIR/ssl/$SERVICE_NAME.pem
    
    # Set proper permissions
    chmod 600 $SERVICE_DIR/ssl/$SERVICE_NAME.key
    chmod 600 $SERVICE_DIR/ssl/$SERVICE_NAME.pfx
    chmod 600 $SERVICE_DIR/ssl/$SERVICE_NAME.pem
    chmod 644 $SERVICE_DIR/ssl/$SERVICE_NAME.crt
    
    # Clean up temporary files
    rm -f $SERVICE_DIR/ssl/$SERVICE_NAME.csr
    rm -f $SERVICE_DIR/ssl/$SERVICE_NAME.ext
    
    print_status "Certificates for $SERVICE_NAME service generated successfully"
    echo "  - Certificate: $SERVICE_DIR/ssl/$SERVICE_NAME.crt"
    echo "  - Private Key: $SERVICE_DIR/ssl/$SERVICE_NAME.key"
    echo "  - PKCS#12: $SERVICE_DIR/ssl/$SERVICE_NAME.pfx (Password: $CERT_PASSWORD)"
    echo "  - PEM Bundle: $SERVICE_DIR/ssl/$SERVICE_NAME.pem"
}

# Generate certificates for each service
# Parameters: service_name, service_dir, service_cn, grpc_port, http_port, cert_password
generate_service_cert "identity" "./services/identity" "identity.estate-elite.com" "50051" "5001" "lau2962003"
generate_service_cert "property" "./services/property" "property.estate-elite.com" "50052" "5002" "lau2962003"
generate_service_cert "payment" "./services/payment" "payment.estate-elite.com" "50053" "5003" "lau2962003"

# Set proper permissions for CA files
chmod 600 ./nginx/ssl/ca.key
chmod 644 ./nginx/ssl/ca.crt
chmod 600 ./nginx/ssl/nginx.key
chmod 644 ./nginx/ssl/nginx.crt
chmod 644 ./nginx/ssl/nginx.pem

# Clean up temporary files
rm -f ./nginx/ssl/nginx.csr
rm -f ./nginx/ssl/nginx.ext

print_status "Generating certificate information summary..."

# Create certificate info file
cat > ./certificate_info.txt << EOF
SSL/TLS Certificate Information for Estate Elite
================================================

Generated on: $(date)

Certificate Authority (CA):
- Certificate: ./nginx/ssl/ca.crt
- Private Key: ./nginx/ssl/ca.key
- Validity: $CA_VALIDITY_DAYS days

Nginx Load Balancer:
- Certificate: ./nginx/ssl/nginx.crt
- Private Key: ./nginx/ssl/nginx.key
- DH Parameters: ./nginx/ssl/nginx.pem
- Domains: localhost, estate-elite.com, www.estate-elite.com

Identity Service:
- Certificate: ./services/identity/ssl/identity.crt
- Private Key: ./services/identity/ssl/identity.key
- PKCS#12: ./services/identity/ssl/identity.pfx
- Password: lau2962003
- Ports: HTTP/1.1 (5001), HTTP/2 gRPC (50051)

Property Service:
- Certificate: ./services/property/ssl/property.crt
- Private Key: ./services/property/ssl/property.key
- PKCS#12: ./services/property/ssl/property.pfx
- Password: lau2962003
- Ports: HTTP/1.1 (5002), HTTP/2 gRPC (50052)

Payment Service:
- Certificate: ./services/payment/ssl/payment.crt
- Private Key: ./services/payment/ssl/payment.key
- PKCS#12: ./services/payment/ssl/payment.pfx
- Password: lau2962003
- Ports: HTTP/1.1 (5003), HTTP/2 gRPC (50053)

Docker Environment Variables (already configured in .env):
- IDENTITY_CERTIFICATE_PATH=/app/ssl/identity.pfx
- IDENTITY_CERTIFICATE_PASSWORD=lau2962003
- PROPERTY_CERTIFICATE_PATH=/app/ssl/property.pfx
- PROPERTY_CERTIFICATE_PASSWORD=lau2962003
- PAYMENT_CERTIFICATE_PATH=/app/ssl/payment.pfx
- PAYMENT_CERTIFICATE_PASSWORD=lau2962003

Security Notes:
- All certificates are signed by the custom CA
- Certificates include Subject Alternative Names (SAN) for proper validation
- PKCS#12 files are password protected
- Private keys have restricted permissions (600)
- Certificates are valid for $CERT_VALIDITY_DAYS days

To trust the CA certificate on your system:
- Linux: sudo cp ./nginx/ssl/ca.crt /usr/local/share/ca-certificates/ && sudo update-ca-certificates
- macOS: sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain ./nginx/ssl/ca.crt
- Windows: Import ./nginx/ssl/ca.crt to "Trusted Root Certification Authorities"
EOF

print_status "Certificate generation completed successfully!"
print_status "Certificate information saved to: ./certificate_info.txt"

echo ""
echo -e "${GREEN}Summary:${NC}"
echo "✓ Root CA certificate and key generated"
echo "✓ Nginx SSL certificate with SAN extensions generated"
echo "✓ DH parameters for Nginx generated"
echo "✓ Identity service certificate and PKCS#12 generated"
echo "✓ Property service certificate and PKCS#12 generated"
echo "✓ Payment service certificate and PKCS#12 generated"
echo "✓ All certificates properly secured with appropriate permissions"

echo ""
echo -e "${YELLOW}Next Steps:${NC}"
echo "1. Review the certificate_info.txt file for detailed information"
echo "2. Consider installing the CA certificate in your system's trust store"
echo "3. Start your Docker services: docker-compose up -d"
echo "4. Test HTTPS endpoints: https://localhost"
echo "5. Test gRPC endpoints with proper TLS configuration"

echo ""
echo -e "${BLUE}Certificate Expiration Monitoring:${NC}"
echo "Set up monitoring for certificate expiration (365 days from now)"
echo "Run this script again before certificates expire to renew them"
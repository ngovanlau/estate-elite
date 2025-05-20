#!/bin/bash
# Script to generate SSL/TLS certificates for gRPC services
# This script should be run from the project root directory

# Create directories for certificates
mkdir -p ./services/identity/ssl
mkdir -p ./services/property/ssl
mkdir -p ./services/payment/ssl
mkdir -p ./nginx/ssl

# Generate CA key and certificate
echo "Generating CA key and certificate..."
openssl genrsa -out ./nginx/ssl/ca.key 4096
openssl req -new -x509 -days 365 -key ./nginx/ssl/ca.key -out ./nginx/ssl/ca.crt \
  -subj "/C=US/ST=State/L=City/O=Estate Elite/OU=IT/CN=estate-elite-ca"

# Generate DH parameters for Nginx
echo "Generating DH parameters for Nginx..."
openssl dhparam -out ./nginx/ssl/nginx.pem 2048

# Generate key and certificate for Nginx
echo "Generating Nginx key and certificate..."
openssl genrsa -out ./nginx/ssl/nginx.key 2048
openssl req -new -key ./nginx/ssl/nginx.key -out ./nginx/ssl/nginx.csr \
  -subj "/C=US/ST=State/L=City/O=Estate Elite/OU=IT/CN=estate-elite.com"
openssl x509 -req -days 365 -in ./nginx/ssl/nginx.csr -CA ./nginx/ssl/ca.crt \
  -CAkey ./nginx/ssl/ca.key -CAcreateserial -out ./nginx/ssl/nginx.crt

# Function to generate service certificates
generate_service_cert() {
  SERVICE_NAME=$1
  SERVICE_DIR=$2
  SERVICE_CN=$3

  echo "Generating certificates for $SERVICE_NAME service..."
  
  # Generate key
  openssl genrsa -out $SERVICE_DIR/ssl/$SERVICE_NAME.key 2048
  
  # Generate CSR
  openssl req -new -key $SERVICE_DIR/ssl/$SERVICE_NAME.key -out $SERVICE_DIR/ssl/$SERVICE_NAME.csr \
    -subj "/C=US/ST=State/L=City/O=Estate Elite/OU=IT/CN=$SERVICE_CN"
  
  # Create service specific config for SAN extension
  cat > $SERVICE_DIR/ssl/$SERVICE_NAME.ext << EOF
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names

[alt_names]
DNS.1 = $SERVICE_CN
DNS.2 = ${PROJECT_PREFIX}-$SERVICE_NAME-service-1
DNS.3 = ${PROJECT_PREFIX}-$SERVICE_NAME-service-2
EOF

  # Sign the certificate
  openssl x509 -req -days 365 -in $SERVICE_DIR/ssl/$SERVICE_NAME.csr \
    -CA ./nginx/ssl/ca.crt -CAkey ./nginx/ssl/ca.key -CAcreateserial \
    -out $SERVICE_DIR/ssl/$SERVICE_NAME.crt -extfile $SERVICE_DIR/ssl/$SERVICE_NAME.ext
    
  # Create PKCS#12 file with password protection
  openssl pkcs12 -export -out $SERVICE_DIR/ssl/$SERVICE_NAME.pfx \
    -inkey $SERVICE_DIR/ssl/$SERVICE_NAME.key \
    -in $SERVICE_DIR/ssl/$SERVICE_NAME.crt \
    -certfile ./nginx/ssl/ca.crt \
    -password pass:${SERVICE_NAME}password

  # Set permissions
  chmod 600 $SERVICE_DIR/ssl/$SERVICE_NAME.key
  chmod 600 $SERVICE_DIR/ssl/$SERVICE_NAME.pfx
}

# Generate certificates for each service
generate_service_cert "identity" "./services/identity" "identity.estate-elite.com"
generate_service_cert "property" "./services/property" "property.estate-elite.com"
generate_service_cert "payment" "./services/payment" "payment.estate-elite.com"

echo "All certificates have been generated successfully!"

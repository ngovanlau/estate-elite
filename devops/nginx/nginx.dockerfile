FROM nginx:alpine

# Install tools needed for environment variable substitution
RUN apk add --no-cache bash openssl

# Create directory for dhparam
RUN mkdir -p /etc/nginx/ssl

# Set working directory
WORKDIR /etc/nginx

# Copy the template configuration
COPY nginx.conf.template /etc/nginx/nginx.conf.template

# Create necessary directories
RUN mkdir -p /var/log/nginx && \
    mkdir -p /etc/nginx/conf.d && \
    touch /var/run/nginx.pid && \
    chown -R nginx:nginx /var/log/nginx /etc/nginx/conf.d /var/run/nginx.pid

# Generate a self-signed certificate for development if needed
RUN if [ ! -f /etc/nginx/ssl/domain.crt ]; then \
        openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
        -keyout /etc/nginx/ssl/domain.key \
        -out /etc/nginx/ssl/domain.crt \
        -subj "/C=US/ST=State/L=City/O=Organization/CN=localhost"; \
    fi

# Generate DH parameters for improved security (2048 bits is a good balance)
RUN openssl dhparam -out /etc/nginx/ssl/dhparam.pem 2048

# Copy the entrypoint script
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

# Expose ports
EXPOSE 80 443

# Set the entrypoint
ENTRYPOINT ["/docker-entrypoint.sh"]

# Command to run when container starts
CMD ["nginx", "-g", "daemon off;"]
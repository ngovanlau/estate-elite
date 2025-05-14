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

# Copy the entrypoint script
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

# Expose ports
EXPOSE 80 443

# Set the entrypoint
ENTRYPOINT ["/docker-entrypoint.sh"]

# Command to run when container starts
CMD ["nginx", "-g", "daemon off;"]
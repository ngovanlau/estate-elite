#!/bin/bash
set -e

# Substitute environment variables in nginx.conf template
envsubst '${PROJECT_PREFIX} ${IDENTITY_HTTP_PORT} ${FUNCTIONAL_HTTP_PORT} ${PROPERTY_HTTP_PORT} ${PAYMENT_HTTP_PORT}' < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf

# Check if the nginx configuration is valid
nginx -t

# Run cmd (in this case, nginx -g daemon off;)
exec "$@"
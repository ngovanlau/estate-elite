# Sử dụng image Nginx chính thức từ Docker Hub
FROM nginx:latest

# Sao chép file nginx.conf vào container Nginx
COPY ./nginx.conf /etc/nginx/nginx.conf

# Mở cổng 80 để Nginx có thể nhận yêu cầu
EXPOSE 80

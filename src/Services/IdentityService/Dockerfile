# Sử dụng image .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Đặt thư mục làm việc trong container
WORKDIR /app

# Sao chép các file csproj và phục hồi các gói
COPY src/Services/IdentityService/IdentityService.API/*.csproj ./IdentityService.API/
RUN dotnet restore ./IdentityService.API/IdentityService.API.csproj

# Sao chép tất cả các file nguồn vào container
COPY . .

# Build và publish ứng dụng trong chế độ Release
RUN dotnet publish src/Services/IdentityService/IdentityService.API/IdentityService.API.csproj -c Release -o /app/out

# Sử dụng image .NET Runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Đặt thư mục làm việc trong container
WORKDIR /app

# Sao chép các file đã được build từ bước trước vào container
COPY --from=build /app/out .

# Cấu hình container để lắng nghe trên cổng 5001
EXPOSE 5001

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]

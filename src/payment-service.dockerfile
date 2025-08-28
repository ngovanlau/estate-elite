# ===== STAGE 1: BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for better layer caching
COPY Services/PaymentService/PaymentService.API/PaymentService.API.csproj Services/PaymentService/PaymentService.API/
COPY Services/PaymentService/PaymentService.Application/PaymentService.Application.csproj Services/PaymentService/PaymentService.Application/
COPY Services/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj Services/PaymentService/PaymentService.Domain/
COPY Services/PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj Services/PaymentService/PaymentService.Infrastructure/

COPY BuildingBlocks/EventBus/EventBus.Abstraction/EventBus.Abstraction.csproj BuildingBlocks/EventBus/EventBus.Abstraction/
COPY BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj BuildingBlocks/EventBus/EventBus.RabbitMQ/
COPY BuildingBlocks/SharedKernel/SharedKernel.csproj BuildingBlocks/SharedKernel/
COPY BuildingBlocks/DistributedCache/DistributedCache.Redis/DistributedCache.Redis.csproj BuildingBlocks/DistributedCache/DistributedCache.Redis/

# Restore packages
RUN dotnet restore Services/PaymentService/PaymentService.API/PaymentService.API.csproj

# Copy everything else and maintain directory structure
COPY Services/PaymentService/ /src/Services/PaymentService/
COPY BuildingBlocks/ /src/BuildingBlocks/

# Build the project
WORKDIR /src/Services/PaymentService/PaymentService.API
RUN dotnet publish -c Release -o /app/publish

# ===== STAGE 2: RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

# Copy build output including appsettings.json
COPY --from=build /app/publish .

# Create directory for logs
RUN mkdir -p /app/logs

# Expose both HTTP and HTTPS ports
EXPOSE 5002

ENTRYPOINT ["dotnet", "PaymentService.API.dll"]
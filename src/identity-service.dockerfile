# ===== STAGE 1: BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for better layer caching
COPY Services/IdentityService/IdentityService.API/IdentityService.API.csproj Services/IdentityService/IdentityService.API/
COPY Services/IdentityService/IdentityService.Application/IdentityService.Application.csproj Services/IdentityService/IdentityService.Application/
COPY Services/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj Services/IdentityService/IdentityService.Domain/
COPY Services/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj Services/IdentityService/IdentityService.Infrastructure/

COPY BuildingBlocks/EventBus/EventBus.Infrastructures/EventBus.Infrastructures.csproj BuildingBlocks/EventBus/EventBus.Infrastructures/
COPY BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj BuildingBlocks/EventBus/EventBus.RabbitMQ/
COPY BuildingBlocks/SharedKernel/SharedKernel.csproj BuildingBlocks/SharedKernel/
COPY BuildingBlocks/DistributedCache/DistributedCache.Redis/DistributedCache.Redis.csproj BuildingBlocks/DistributedCache/DistributedCache.Redis/

# Restore packages
RUN dotnet restore Services/IdentityService/IdentityService.API/IdentityService.API.csproj

# Copy everything else and maintain directory structure
COPY Services/IdentityService/ /src/Services/IdentityService/
COPY BuildingBlocks/ /src/BuildingBlocks/

# Build the project
WORKDIR /src/Services/IdentityService/IdentityService.API
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
EXPOSE 5001

ENTRYPOINT ["dotnet", "IdentityService.API.dll"]
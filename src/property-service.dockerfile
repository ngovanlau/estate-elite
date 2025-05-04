# ===== STAGE 1: BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for better layer caching
COPY Services/PropertyService/PropertyService.API/PropertyService.API.csproj Services/PropertyService/PropertyService.API/
COPY Services/PropertyService/PropertyService.Application/PropertyService.Application.csproj Services/PropertyService/PropertyService.Application/
COPY Services/PropertyService/PropertyService.Domain/PropertyService.Domain.csproj Services/PropertyService/PropertyService.Domain/
COPY Services/PropertyService/PropertyService.Infrastructure/PropertyService.Infrastructure.csproj Services/PropertyService/PropertyService.Infrastructure/

COPY BuildingBlocks/EventBus/EventBus.Infrastructures/EventBus.Infrastructures.csproj BuildingBlocks/EventBus/EventBus.Infrastructures/
COPY BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj BuildingBlocks/EventBus/EventBus.RabbitMQ/
COPY BuildingBlocks/SharedKernel/SharedKernel.csproj BuildingBlocks/SharedKernel/
COPY BuildingBlocks/DistributedCache/DistributedCache.Redis/DistributedCache.Redis.csproj BuildingBlocks/DistributedCache/DistributedCache.Redis/

# Restore packages
RUN dotnet restore Services/PropertyService/PropertyService.API/PropertyService.API.csproj

# Copy everything else and maintain directory structure
COPY Services/ /src/Services/
COPY BuildingBlocks/ /src/BuildingBlocks/

# Build the project
WORKDIR /src/Services/PropertyService/PropertyService.API
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
EXPOSE 5003
EXPOSE 5103

ENTRYPOINT ["dotnet", "PropertyService.API.dll"]
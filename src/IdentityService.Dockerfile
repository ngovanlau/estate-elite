# ===== STAGE 1: BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first for better layer caching
COPY Services/IdentityService/IdentityService.API/IdentityService.API.csproj Services/IdentityService/IdentityService.API/
COPY Services/IdentityService/IdentityService.Application/IdentityService.Application.csproj Services/IdentityService/IdentityService.Application/
COPY Services/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj Services/IdentityService/IdentityService.Domain/
COPY Services/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj Services/IdentityService/IdentityService.Infrastructure/

COPY BuildingBlocks/EventBus/EventBus.csproj BuildingBlocks/EventBus/
COPY BuildingBlocks/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj BuildingBlocks/EventBus.RabbitMQ/
COPY BuildingBlocks/SharedKernel/SharedKernel.csproj BuildingBlocks/SharedKernel/

# Restore packages
RUN dotnet restore Services/IdentityService/IdentityService.API/IdentityService.API.csproj

# Copy everything else and maintain directory structure
COPY Services/ /src/Services/
COPY BuildingBlocks/ /src/BuildingBlocks/

# Build the project
WORKDIR /src/Services/IdentityService/IdentityService.API
RUN dotnet publish -c Release -o /app/publish

# ===== STAGE 2: RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy build output from build stage
COPY --from=build /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001;https://+:5101
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]
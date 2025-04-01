# ===== STAGE 1: BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files first for better layer caching
COPY ["Services/IdentityService/IdentityService.API/IdentityService.API.csproj", "src/Services/IdentityService/IdentityService.API/"]
COPY ["Services/IdentityService/IdentityService.Application/IdentityService.Application.csproj", "src/Services/IdentityService/IdentityService.Application/"]
COPY ["Services/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj", "src/Services/IdentityService/IdentityService.Domain/"]
COPY ["Services/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj", "src/Services/IdentityService/IdentityService.Infrastructure/"]

COPY ["BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["BuildingBlocks/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/BuildingBlocks/EventBus.RabbitMQ/"]
COPY ["BuildingBlocks/SharedKernel/SharedKernel.csproj", "src/BuildingBlocks/SharedKernel/"]

# Restore packages
RUN dotnet restore "src/Services/IdentityService/IdentityService.API/IdentityService.API.csproj"

# Copy everything else
COPY . .

# Build the project
WORKDIR "/app/src/Services/IdentityService/IdentityService.API"
RUN dotnet publish -c Release -o /app/publish

# ===== STAGE 2: RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy build output from build stage
COPY --from=build /app/publish .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001 
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]
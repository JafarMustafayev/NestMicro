﻿# NotificationService.Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/NestStorageService/src/NestStorage.API/NestStorage.API.csproj", "Services/NestStorageService/src/NestStorage.API/"]
COPY ["BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj", "BuildingBlocks/EventBus/EventBus.Abstractions/"]
COPY ["BuildingBlocks/EventBus/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "BuildingBlocks/EventBus/EventBus.RabbitMq/"]
COPY ["Shared/Nest.Shared/Nest.Shared.csproj", "Shared/Nest.Shared/"]

RUN dotnet restore "Services/NestStorageService/src/NestStorage.API/NestStorage.API.csproj"

COPY . .

WORKDIR "/src/Services/NestStorageService/src/NestStorage.API"
RUN dotnet build "NestStorage.API.csproj" -c Release -o /app/build --verbosity normal

# Publish et
FROM build AS publish
RUN dotnet publish "NestStorage.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --verbosity detailed

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NestStorage.API.dll"]
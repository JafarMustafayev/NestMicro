FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Əsas layihə faylı
COPY ["Services/NestNotificationService/src/NestNotification.API/NestNotification.API.csproj", "Services/NestNotificationService/src/NestNotification.API/"]

# EventBus asılılıqları
COPY ["BuildingBlocks/EventBus/EventBus.Abstractions/EventBus.Abstractions.csproj", "BuildingBlocks/EventBus/EventBus.Abstractions/"]
COPY ["BuildingBlocks/EventBus/EventBus.RabbitMq/EventBus.RabbitMq.csproj", "BuildingBlocks/EventBus/EventBus.RabbitMq/"]

# Shared layihə
COPY ["Shared/Nest.Shared/Nest.Shared.csproj", "Shared/Nest.Shared/"]

# Bərpa edin
RUN dotnet restore "Services/NestNotificationService/src/NestNotification.API/NestNotification.API.csproj"

# Bütün qalan faylları köçürün
COPY . .

# Tikin
WORKDIR "/src/Services/NestNotificationService/src/NestNotification.API"
RUN dotnet build "NestNotification.API.csproj" -c Release -o /app/build

# Nəşr edin
FROM build AS publish
RUN dotnet publish "NestNotification.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Son təsvirinizi yaradın
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NestNotification.API.dll"]
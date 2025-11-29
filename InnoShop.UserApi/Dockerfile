# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InnoShop.UserApi/InnoShop.UserApi.csproj", "InnoShop.UserApi/"]
COPY ["InnoShop.Application/InnoShop.Application.csproj", "InnoShop.Application/"]
COPY ["InnoShop.Domain/InnoShop.Domain.csproj", "InnoShop.Domain/"]
COPY ["InnoShop.ProductApplication/InnoShop.ProductApplication.csproj", "InnoShop.ProductApplication/"]
COPY ["InnoShop.ProductDomain/InnoShop.ProductDomain.csproj", "InnoShop.ProductDomain/"]
COPY ["InnoShop.Shared/InnoShop.Shared.csproj", "InnoShop.Shared/"]
COPY ["InnoShop.Infrastructure/InnoShop.Infrastructure.csproj", "InnoShop.Infrastructure/"]
RUN dotnet restore "./InnoShop.UserApi/InnoShop.UserApi.csproj"
COPY . .
WORKDIR "/src/InnoShop.UserApi"
RUN dotnet build "./InnoShop.UserApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InnoShop.UserApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoShop.UserApi.dll"]
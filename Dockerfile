FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["InnoShop.UserApi/InnoShop.UserApi.csproj", "InnoShop.UserApi/"]
COPY ["InnoShop.Application/InnoShop.Application.csproj", "InnoShop.Application/"]
COPY ["InnoShop.Infrastructure/InnoShop.Infrastructure.csproj", "InnoShop.Infrastructure/"]
COPY ["InnoShop.Domain/InnoShop.Domain.csproj", "InnoShop.Domain/"]
RUN dotnet restore "InnoShop.UserApi/InnoShop.UserApi.csproj"

COPY . .
WORKDIR "/src/InnoShop.UserApi"
RUN dotnet build "InnoShop.UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InnoShop.UserApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoShop.UserApi.dll"]
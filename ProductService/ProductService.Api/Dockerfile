# =====================
# 1. Base (runtime)
# =====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# =====================
# 2. Build
# =====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProductService/ProductService.Api/ProductService.Api.csproj", "ProductService/ProductService.Api/"]
COPY ["SharedMessaging/SharedMessaging.csproj", "SharedMessaging/"]
COPY ["ProductService/ProductService.Application/ProductService.Application.csproj", "ProductService/ProductService.Application/"]
COPY ["ProductService/ProductService.Domain/ProductService.Domain.csproj", "ProductService/ProductService.Domain/"]
COPY ["ProductService/ProductService.Infrastructure/ProductService.Infrastructure.csproj", "ProductService/ProductService.Infrastructure/"]

RUN dotnet restore "ProductService/ProductService.Api/ProductService.Api.csproj"

COPY . .
WORKDIR "/src/ProductService/ProductService.Api"

RUN dotnet publish "ProductService.Api.csproj" -c Release -o /app/publish

# =====================
# 3. Final (runtime)
# =====================
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ProductService.Api.dll"]
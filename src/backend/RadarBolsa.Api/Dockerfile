FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/backend/RadarBolsa.Domain/RadarBolsa.Domain.csproj", "src/backend/RadarBolsa.Domain/"]
COPY ["src/backend/RadarBolsa.Application/RadarBolsa.Application.csproj", "src/backend/RadarBolsa.Application/"]
COPY ["src/backend/RadarBolsa.Infrastructure/RadarBolsa.Infrastructure.csproj", "src/backend/RadarBolsa.Infrastructure/"]
COPY ["src/backend/RadarBolsa.Api/RadarBolsa.Api.csproj", "src/backend/RadarBolsa.Api/"]
RUN dotnet restore "src/backend/RadarBolsa.Api/RadarBolsa.Api.csproj"

COPY . .
WORKDIR /src/src/backend/RadarBolsa.Api
RUN dotnet publish "RadarBolsa.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RadarBolsa.Api.dll"]

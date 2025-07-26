# Build application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app/src

COPY ["Identity/Api/Api.csproj", "Identity/Api/"]
COPY ["Identity/Dtos/Dtos.csproj", "Identity/Dtos/"]
COPY ["Shared/Shared.csproj", "Shared/"]

COPY . .

WORKDIR "/app/src/Identity/Api"
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Run application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR "/app"
COPY --from=build /app/publish .
RUN mkdir -p /app/media

ENTRYPOINT ["dotnet", "Api.dll"]

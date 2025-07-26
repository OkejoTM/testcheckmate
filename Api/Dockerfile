# Build application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app/src

COPY ["Api/Api/Api.csproj", "Api/Api/"]
COPY ["Api/Application/Application.csproj", "Api/Application/"]
COPY ["Api/Domain/Domain.csproj", "Api/Domain/"]
COPY ["Api/Infrustructure/Infrustructure.csproj", "Api/Infrustructure/"]
COPY ["Api/Dtos/Dtos.csproj", "Api/Dtos/"]
COPY ["Shared/Shared.csproj", "Shared/"]

COPY . .

WORKDIR "/app/src/Api/Api"
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# Run application
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR "/app"
COPY --from=build /app/publish .
RUN mkdir -p /app/media

ENTRYPOINT ["dotnet", "Api.dll"]

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY MockApi.sln .
COPY src/MockApi.Api/*.csproj src/MockApi.Api/
COPY tests/MockApi.Api.Tests/*.csproj tests/MockApi.Api.Tests/
RUN dotnet restore

COPY . .
WORKDIR /src/src/MockApi.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "MockApi.Api.dll"]
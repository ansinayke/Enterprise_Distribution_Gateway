# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm64v8 AS build
WORKDIR /src
COPY ["api-gateway.csproj", "./"]
RUN dotnet restore "api-gateway.csproj"
COPY . .
RUN dotnet build "api-gateway.csproj" -c Release -o /app/build
RUN dotnet publish "api-gateway.csproj" -c Release -o /app/publish

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-arm64v8
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "api-gateway.dll"]

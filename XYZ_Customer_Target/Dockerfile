# Use the official .NET 7.0 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY eShopLegacyMVC_Core/*.csproj ./eShopLegacyMVC_Core/
RUN dotnet restore eShopLegacyMVC_Core/eShopLegacyMVC_Core.csproj

# Copy the rest of the application code
COPY eShopLegacyMVC_Core/ ./eShopLegacyMVC_Core/
WORKDIR /app/eShopLegacyMVC_Core
RUN dotnet publish -c Release -o out

# Use the .NET 7.0 runtime image for final execution
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/eShopLegacyMVC_Core/out ./ 

# Expose ports
EXPOSE 5001

# Set the entry point
ENTRYPOINT ["dotnet", "eShopLegacyMVC_Core.dll"]


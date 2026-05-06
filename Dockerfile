# 1. Use the .NET 10 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy just the csproj first to cache dependencies (makes future builds much faster)
COPY ["InventoryManagementSystem.csproj", "./"]
RUN dotnet restore "./InventoryManagementSystem.csproj"

# Copy the rest of the code and build the release
COPY . .
RUN dotnet publish "InventoryManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Use the lighter .NET 10 ASP.NET runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# 3. Configure Render-specific settings
# Render dynamically assigns a port, but defaults to 10000 for web services
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

# 4. Start the app
ENTRYPOINT ["dotnet", "InventoryManagementSystem.dll"]
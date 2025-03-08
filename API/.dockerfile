# Build stage: use the .NET 8 SDK image to build and publish the API project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and the project files for each project
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]
COPY ["Persistance/Persistance.csproj", "Persistance/"]

# Restore dependencies for the entire solution
RUN dotnet restore

# Copy the rest of the solution
COPY . .

# Build and publish the API project (adjust the path if your startup project is different)
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

# Runtime stage: use the .NET 8 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port (default 80)
EXPOSE 80

# Start the API application
ENTRYPOINT ["dotnet", "API.dll"]

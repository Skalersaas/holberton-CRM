# Build stage: use the .NET 8 SDK image to build and publish the API project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files (only necessary files for restoring dependencies)
COPY ["holberton-CRM.sln", "./"]
COPY ["API/API.csproj", "API/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistance/Persistance.csproj", "Persistance/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

# Restore dependencies for the entire solution
RUN dotnet restore

# Copy the entire solution to build
COPY . .

# Build and publish the API project
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

# ---------------------------------
# Final stage: create a smaller runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN useradd -m appuser
USER appuser

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose necessary ports (avoid exposing unnecessary ports)
EXPOSE 8080

# Set the entrypoint
ENTRYPOINT ["dotnet", "API.dll"]

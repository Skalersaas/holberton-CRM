# Build stage: use the .NET 8 SDK image to build and publish the API project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["holberton-CRM.sln", "./"]
COPY ["API/API.csproj", "API/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistance/Persistance.csproj", "Persistance/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

# Restore dependencies for the entire solution
RUN dotnet restore

# Copy the entire solution; ensure your build context is the root of your solution
COPY . .

# Build and publish the API project
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

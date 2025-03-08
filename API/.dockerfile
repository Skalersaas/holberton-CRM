# Build stage: use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project file(s) and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of your source code and build the application
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Runtime stage: use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .

# Optionally expose the port your application listens on (default 80)
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "YourAppName.dll"]

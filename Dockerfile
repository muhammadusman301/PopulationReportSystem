# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build the application in Release mode
RUN dotnet build --configuration Release --no-restore

# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore

# Use a smaller .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Command to run the application
CMD ["dotnet", "PopulationDataApp.dll"]

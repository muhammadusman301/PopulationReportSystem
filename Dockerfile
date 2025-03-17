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

# Use ASP.NET runtime image instead of basic runtime
# This provides additional components needed for web applications
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Add environment variables for MySQL connection
ENV DB_HOST=db
ENV DB_PORT=3306
ENV DB_USER=root
ENV DB_PASSWORD=root
ENV DB_NAME=world

# Explicitly expose the port
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Command to run the application - updated with correct DLL name if needed
CMD ["dotnet", "PopulationReportSystem.dll"]
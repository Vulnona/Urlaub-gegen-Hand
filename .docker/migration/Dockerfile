# AS Build-env
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set PATH to include global tools
ENV PATH $PATH:/root/.dotnet/tools

# Install dotnet-ef tool globally
RUN dotnet tool install -g dotnet-ef --version 7.0.10

WORKDIR /app

# Copy backend source code
COPY ./Backend /app/Backend

WORKDIR /app/Backend

# Restore NuGet packages and build the backend project
RUN dotnet restore
RUN dotnet build --no-restore

# Run dotnet tool restore to ensure dotnet-ef command is available
RUN dotnet tool restore

# Entry point to execute database migrations
ENTRYPOINT ["dotnet", "ef", "database", "update"]

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS backend-build
WORKDIR /app

# Copy ports configuration for build-time access
COPY scripts/infrastructure/ports.config /app/ports.config

# Copy project files and restore with custom NuGet config
COPY Backend/UGHApi.csproj Backend/

# Create clean NuGet config to avoid Windows path issues
RUN echo '<?xml version="1.0" encoding="utf-8"?>' > /tmp/nuget.config && \
    echo '<configuration>' >> /tmp/nuget.config && \
    echo '  <packageSources>' >> /tmp/nuget.config && \
    echo '    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />' >> /tmp/nuget.config && \
    echo '  </packageSources>' >> /tmp/nuget.config && \
    echo '</configuration>' >> /tmp/nuget.config

# Restore with clean NuGet configuration
RUN dotnet restore Backend/UGHApi.csproj --configfile /tmp/nuget.config --disable-parallel

# Copy source code
COPY Backend/ Backend/

# Build and publish with clean NuGet config (removed --no-restore as requested)
RUN dotnet publish Backend/UGHApi.csproj \
    --configuration Release \
    --output /app/binaries \
    --verbosity minimal

# Copy Template folder
COPY Backend/Template /app/Template

# Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS backend-runtime
WORKDIR /app

RUN apk add --no-cache \
    dotnet7-sdk \
    chromium \
    chromium-chromedriver \
    bash \
    icu-libs \
    tzdata \
    font-noto \
    msttcorefonts-installer \
    mysql-client \
    && update-ms-fonts \
    && fc-cache -f

# Install EF Core Tools globally (compatible with .NET 7.0)
RUN dotnet tool install --global dotnet-ef --version 7.0.20

# Set PATH environment variable correctly for EF tools (consolidated)
ENV PATH="/root/.dotnet/tools:/usr/lib/chromium/:/usr/local/bin/:$PATH"

# Copy appsettings.json
COPY Backend/appsettings.json /app

# Copy the published binaries
COPY --from=backend-build /app/binaries /app/binaries

# Copy Backend source code for EF migrations
COPY ./Backend /app/Backend

# Copy Template folder
COPY --from=backend-build /app/Template /app/Template

# Copy ports configuration for runtime access
COPY --from=backend-build /app/ports.config /app/ports.config

# Create startup script to read port configuration
RUN echo '#!/bin/sh' > /app/start.sh && \
    echo 'set -e' >> /app/start.sh && \
    echo '' >> /app/start.sh && \
    echo '# Read port configuration from ports.config' >> /app/start.sh && \
    echo 'if [ -f /app/ports.config ]; then' >> /app/start.sh && \
    echo '  export $(grep "^BACKEND_PORT=" /app/ports.config | xargs)' >> /app/start.sh && \
    echo '  export $(grep "^BACKEND_API_PORT=" /app/ports.config | xargs)' >> /app/start.sh && \
    echo '  # Use BACKEND_API_PORT if available, otherwise fallback to BACKEND_PORT' >> /app/start.sh && \
    echo '  PORT=${BACKEND_API_PORT:-${BACKEND_PORT:-8080}}' >> /app/start.sh && \
    echo 'else' >> /app/start.sh && \
    echo '  echo "Warning: ports.config not found, using default port 8080"' >> /app/start.sh && \
    echo '  PORT=8080' >> /app/start.sh && \
    echo 'fi' >> /app/start.sh && \
    echo '' >> /app/start.sh && \
    echo 'echo "Starting backend on port $PORT"' >> /app/start.sh && \
    echo 'export ASPNETCORE_URLS="http://+:$PORT"' >> /app/start.sh && \
    echo 'exec dotnet /app/binaries/UGHApi.dll' >> /app/start.sh && \
    chmod +x /app/start.sh

# Set environment variables for template paths
ENV TemplateSettings__SuccessTemplate=/app/Template/confirmation.html
ENV TemplateSettings__FailedTemplate=/app/Template/DeclinedConfirmation.html

# Set working directory to app root for EF commands
WORKDIR /app

# Expose port dynamically (will be read from ports.config at startup)
EXPOSE 8080

# Set ASP.NET Core environment variables
ENV ASPNETCORE_ENVIRONMENT=Development

# Start the application using startup script that reads port configuration
CMD ["/app/start.sh"]

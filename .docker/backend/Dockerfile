# Backend build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-build
WORKDIR /app

# Copy Backend source code
COPY ./Backend /app/Backend/

# Copy Template folder
COPY ./Backend/Template /app/Template/

# Restore dependencies using the solution file
RUN dotnet restore /app/Backend/UGHApi.sln

# Publish the Backend project
RUN dotnet publish /app/Backend/UGHApi.csproj --use-current-runtime --self-contained false -o /app/binaries
RUN rm -r /app/Backend

# Copy chromedriver from the root directory
COPY ./chromedriver /app/binaries/chromedriver

# Make chromedriver executable
RUN chmod +x /app/binaries/chromedriver

# Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS backend-runtime
WORKDIR /app

# Copy appsettings.json
COPY Backend/appsettings.json /app

# Install nginx, network tools, and procps
RUN apk add --no-cache nginx iputils procps

# Install Python
RUN apk add --no-cache python3

# Copy the published binaries and chromedriver
COPY --from=backend-build /app/binaries /app/binaries

# Copy Template folder
COPY --from=backend-build /app/Template /app/Template

# Set environment variables for template paths
ENV TemplateSettings__SuccessTemplate=/app/Template/confirmation.html
ENV TemplateSettings__FailedTemplate=/app/Template/DeclinedConfirmation.html

# Set environment variables for chromedriver.exe and facebookcrawler.py paths
ENV ChromeDriverPath=/app/Template/chromedriver.exe
ENV FacebookCrawlerScriptPath=/app/Template/facebookcrawler.py

# Expose port 80
EXPOSE 80

# Start chromedriver and the .NET application, and print success message
CMD ["sh", "-c", "\
    echo 'Starting ChromeDriver...' && \
    /app/binaries/chromedriver --port=9515 --log-path=/app/chromedriver.log --verbose & \
    CHROMEDRIVER_PID=$! && \
    sleep 2 && \
    if pgrep -f chromedriver > /dev/null; then \
        echo 'ChromeDriver was started successfully'; \
    else \
        echo 'Failed to start ChromeDriver. Check /app/chromedriver.log for details.'; \
        exit 1; \
    fi && \
    dotnet /app/binaries/UGHApi.dll --urls http://+:80"]

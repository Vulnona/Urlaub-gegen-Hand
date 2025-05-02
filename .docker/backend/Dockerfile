# Backend build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-build
WORKDIR /app

# run the dotnet restore as a seperate step to allow docker to cache the result
COPY ./Backend/UGHApi.csproj /app/Backend/UGHApi.csproj
RUN dotnet restore /app/Backend/UGHApi.csproj
    
# Copy Backend source code
COPY ./Backend /app/Backend/

# Copy Template folder
COPY ./Backend/Template /app/Template/

# Restore dependencies using the solution file
RUN dotnet restore /app/Backend/UGHApi.sln

# Publish the Backend project
RUN dotnet publish /app/Backend/UGHApi.csproj --use-current-runtime --self-contained false -o /app/binaries
RUN rm -r /app/Backend

# Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS backend-runtime
WORKDIR /app

# Install dependencies for Chrome and ChromeDriver
RUN apk add --no-cache \
    chromium \
    chromium-chromedriver \
    bash \
    icu-libs \
    tzdata \
    font-noto \
    msttcorefonts-installer \
    && update-ms-fonts \
    && fc-cache -f

# Set the path environment variable to include ChromeDriver
ENV PATH="/usr/lib/chromium/:/usr/local/bin/:${PATH}"

# Copy appsettings.json
COPY Backend/appsettings.json /app

# Copy the published binaries
COPY --from=backend-build /app/binaries /app/binaries

# Copy Template folder
COPY --from=backend-build /app/Template /app/Template

# Set environment variables for template paths
ENV TemplateSettings__SuccessTemplate=/app/Template/confirmation.html
ENV TemplateSettings__FailedTemplate=/app/Template/DeclinedConfirmation.html

# Expose port 80
EXPOSE 80

# Start the .NET application
CMD ["dotnet", "/app/binaries/UGHApi.dll", "--urls", "http://+:80"]

# AS Build-env
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install -g dotnet-ef --version 7.0.10

WORKDIR /app

# Backend-Quellcode kopieren
COPY ./Backend /app/Backend

WORKDIR /app/Backend

# Wiederherstellen und Bauen des Backend-Projekts
RUN dotnet restore 
RUN dotnet build --no-restore

# Ausführen der Migration beim Start des Containers
ENTRYPOINT ["dotnet", "ef", "database", "update"]
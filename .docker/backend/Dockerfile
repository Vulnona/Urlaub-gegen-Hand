FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-build
WORKDIR /app
# run the dotnet restore as a seperate step to allow docker to cache the result
COPY ./Backend/UGHApi.csproj /app/Backend/UGHApi.csproj
RUN dotnet restore /app/Backend/UGHApi.csproj

COPY ./Backend /app/Backend
RUN dotnet publish /app/Backend/UGHApi.csproj --use-current-runtime --self-contained false -o /app/binaries
RUN rm -r /app/Backend

# Laufzeitumgebung
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS backend-runtime
WORKDIR /app

# Nginx installieren und Netzwerktools hinzufügen
RUN apk add nginx iputils

# Backend kopieren
COPY --from=backend-build /app/binaries /app/binaries

#PORT
EXPOSE 80

# Nginx und Backend starten
CMD ["sh", "-c", "dotnet /app/binaries/UGHApi.dll --urls http://+:80"]
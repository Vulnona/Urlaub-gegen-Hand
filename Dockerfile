# Backend-Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS backend-build
WORKDIR /app

# Backend Stuff
COPY ./Backend /app/Backend
RUN dotnet publish /app/Backend/UGHApi.csproj --use-current-runtime --self-contained false -o /app/binaries
RUN rm -r /app/Backend

FROM node:alpine AS frontend-build
WORKDIR /app

#Frontend Stuff
COPY ./Frontend-Vuetify /app
RUN npm install
RUN npm run build

# Laufzeitumgebung
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app

# Nginx installieren
RUN apk add nginx

# Backend kopieren
COPY --from=backend-build /app/binaries /app/binaries

# Frontend kopieren
COPY --from=frontend-build /app/dist /www

# Nginx config hinzufügen
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

# Nginx und Backend starten
CMD ["sh", "-c", "nginx && dotnet /app/binaries/UGHApi.dll --urls http://+:80"]

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine

RUN apt-get update && apt-get install -y mysql-server

# Codes in das Image kopieren
COPY ./Backend /app/Backend
COPY ./Frontend /app/Frontend

# Backend aus Quellcode bauen
RUN dotnet publish /app/Backend/UGHApi.csproj --use-current-runtime --self-contained false -o /app/binaries

# Quellcode aus Image entfernen
RUN rm -r /app/Backend

EXPOSE 80

# Backend starten
CMD ["dotnet","/app/binaries/UGHApi.dll","--urls", "http://+:80"]

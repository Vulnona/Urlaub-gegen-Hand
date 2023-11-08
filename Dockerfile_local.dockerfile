FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine

COPY ./Backend /app/Backend
COPY ./Frontend /app/Frontend

EXPOSE 80

CMD ["dotnet","run", "--project", "/app/Backend/UGHApi.csproj","--urls", "http://+:80"]

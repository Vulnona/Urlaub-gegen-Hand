FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

#RUN apt-get update && apt-get install -y mysql-server

#RUN mysql -u root -p < /docker-entrypoint-initdb.d/create-user.sql

# EF Migration
#RUN dotnet ef migrations add InitialCreate
#RUN dotnet ef database update
# Build application
RUN dotnet publish ./Backend/UGHApi.csproj
# Copy application
#COPY /bin/Release/netcoreapp3.1/ugh-api /app/ugh-api
COPY /Backend/bin/Release//net7.0/publish /app/ugh-api

# Start application
CMD ["/app/ugh-api"]

EXPOSE 80


FROM ubuntu:latest

RUN apt-get update && apt-get install -y mysql-server

RUN mysql -u root -p < /docker-entrypoint-initdb.d/create-user.sql

# EF Migration
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

# Copy application
COPY bin/Release/netcoreapp3.1/ugh-api /app/ugh-api

# Start application
CMD ["/app/ugh-api"]

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

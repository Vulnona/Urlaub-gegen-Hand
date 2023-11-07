FROM ubuntu:latest

RUN apt-get update && apt-get install -y mysql-server

RUN mysql -u root -p < /docker-entrypoint-initdb.d/create-user.sql

# EF Migration
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

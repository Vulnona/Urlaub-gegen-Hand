FROM ubuntu:latest

RUN apt-get update && apt-get install -y mysql-server

COPY ./Backend /app/Backend
COPY ./Frontend /app/Frontend

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

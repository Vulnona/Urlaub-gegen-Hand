FROM ubuntu:latest

RUN apt-get update && apt-get install -y mysql-server

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

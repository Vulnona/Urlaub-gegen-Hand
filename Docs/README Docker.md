## Setup and Start Docker
# Docker builden
docker build -t ugh .

# Docker starten
docker run -d -p [Port:Port]] ugh

# ggf. Logs prüfen
docker logs [containername]

# lokaler Aufruf
http://localhost:[Port]]/

# Inkl "allem":
docker-compose up -d --build

# Herunterfahren der Container:
docker-compose down

##### Datenbank

## Migration

docker-compose up migration

# Mysql Container erstellen
docker run -d --name silly_einstein -e MYSQL_ROOT_PASSWORD=passwort -e MYSQL_DATABASE=db mysql/mysql-server

# Verbindung erfolgreich?
docker ps

# Mit dem Container verbinden
docker exec -it silly_einstein /bin/sh
-> mysql -u root -p
-> Passwort eingeben


# ggf. Datenbank migrieren, wobei dies auch durch einen eigenständigen Docker erledigt werden kann, s. "Migration"
cd UGH/Backend 
dotnet ef database update

# In die Db, wo ganz normale Datenbankabfragen und -manipulationen getätigt werden können
docker exec -it ugh-db-1 mysql -u user -p
-> Passwort ("password")

# Testnutzer

Datenbank touchen
USE db;

# (weitere) Testnutzer erstellen
INSERT INTO Profiles (NickName) VALUES ('TestNickName');

bzw.
docker exec -it ugh-db-1 mysql -u user -p db < /docker-entrypoint-initdb.d/init_db_test.sql

# Hat es geklappt?
select * from Profiles;

# 'Workaround' solange wir noch keine Auth haben:
docker exec -it ugh-db-1 mysql -u root -p
GRANT ALL PRIVILEGES ON db.* TO 'user'@'localhost' IDENTIFIED BY 'password';
FLUSH PRIVILEGES;


## Troubleshooting
Windows User:  
If you get an error `ERROR: error during connect: in the default daemon configuration on Windows, the docker client must be run with elevated privileges to connect: ... The system cannot find the file specified.`  

make sure Docker Desktop is running.  
Proof this by checking your status in Docker Desktop's left bottom corner, should display
> Engine running

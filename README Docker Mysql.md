Mysql Container erstellen
docker run -d --name silly_einstein -e MYSQL_ROOT_PASSWORD=passwort -e MYSQL_DATABASE=db mysql/mysql-server

Verbindung erfolgreich?
docker ps

Mit dem Container verbinden
docker exec -it silly_einstein /bin/sh
-> mysql -u root -p
-> Passwort eingeben


ggf. Datenbank migrieren
cd UGH/Backend 
dotnet ef database update

In die Db:
docker exec -it ugh-db-1 mysql -u user -p

Testnutzer

Datenbank touchen
USE db;

(weitere) Testnutzer erstellen
INSERT INTO Profiles (NickName) VALUES ('TestNickName');

bzw.
docker exec -it ugh-db-1 mysql -u user -p db < /docker-entrypoint-initdb.d/init_db_test.sql

Hat es geklappt?
select * from Profiles;
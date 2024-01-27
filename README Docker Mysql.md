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


Testnutzer

Datenbank touchen
USE db;

Tabelle erstellen
CREATE TABLE TestUser (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    VisibleName VARCHAR(100),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DateOfBirth DATE,
    Gender CHAR(1),
    Street VARCHAR(100),
    HouseNumber VARCHAR(10),
    PostCode VARCHAR(10),
    City VARCHAR(100),
    Country VARCHAR(100),
    Email_Address VARCHAR(100),
    IsEmailVerified BOOLEAN
);

User erstellen und einfügen
INSERT INTO TestUser (VisibleName, FirstName, LastName, DateOfBirth, Gender, Street, HouseNumber, PostCode, City, Country, Email_Address, IsEmailVerified)
VALUES ('Testuser', 'Vorname', 'Nachname', '2000-01-01', 'M', 'Musterstraße', '123', '12345', 'Musterstadt', 'Deutschland', 'testuser@example.com', 1);


Datensätze anzeigen lassen
SELECT * FROM TestUser;
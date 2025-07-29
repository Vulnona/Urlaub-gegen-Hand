-- MySQL dump 10.13  Distrib 8.0.42, for Linux (x86_64)
--
-- Host: localhost    Database: db
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `DeletedUserBackups`
--

DROP TABLE IF EXISTS `DeletedUserBackups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `DeletedUserBackups` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FirstName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LastName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Gender` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DateOfBirth` datetime(6) DEFAULT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Skills` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Hobbies` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Latitude` double DEFAULT NULL,
  `Longitude` double DEFAULT NULL,
  `ProfilePicture` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DeletedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DeletedUserBackups`
--

LOCK TABLES `DeletedUserBackups` WRITE;
/*!40000 ALTER TABLE `DeletedUserBackups` DISABLE KEYS */;
/*!40000 ALTER TABLE `DeletedUserBackups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20241016071620_InitialMigration','7.0.0'),('20250725223915_InitialCreate','7.0.20'),('20250725231705_RemoveCountryFields','7.0.20'),('20250726154812_AddDeletedUserBackup','7.0.20'),('20250727164854_AddOfferIdToPictures','7.0.20');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accommodationsuitables`
--

DROP TABLE IF EXISTS `accommodationsuitables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accommodationsuitables` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accommodationsuitables`
--

LOCK TABLES `accommodationsuitables` WRITE;
/*!40000 ALTER TABLE `accommodationsuitables` DISABLE KEYS */;
INSERT INTO `accommodationsuitables` VALUES (3,'Kinderfreundlich'),(4,'Hundehalter'),(5,'Alleinreisend'),(6,'Paare'),(7,'Senioren'),(8,'Gruppen'),(9,'Barrierefrei');
/*!40000 ALTER TABLE `accommodationsuitables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accomodations`
--

DROP TABLE IF EXISTS `accomodations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accomodations` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accomodations`
--

LOCK TABLES `accomodations` WRITE;
/*!40000 ALTER TABLE `accomodations` DISABLE KEYS */;
INSERT INTO `accomodations` VALUES (1,'Zimmer'),(2,'Wohnung'),(3,'Haus'),(4,'Wohnwagen'),(5,'Zelt'),(6,'Hütte'),(7,'Bauernhof'),(8,'Schloss'),(9,'Villa'),(10,'Apartment'),(11,'Studio'),(12,'Loft'),(13,'Penthouse'),(14,'Bungalow'),(15,'Ferienwohnung');
/*!40000 ALTER TABLE `accomodations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `addresses`
--

DROP TABLE IF EXISTS `addresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `addresses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DisplayName` longtext NOT NULL,
  `Latitude` double NOT NULL,
  `Longitude` double NOT NULL,
  `Country` longtext,
  `State` longtext,
  `City` longtext,
  `PostalCode` longtext,
  `Street` longtext,
  `HouseNumber` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addresses`
--

LOCK TABLES `addresses` WRITE;
/*!40000 ALTER TABLE `addresses` DISABLE KEYS */;
INSERT INTO `addresses` VALUES (1,'Default Address',0.000000000000000000000000000000,0.000000000000000000000000000000,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `addresses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `coupons`
--

DROP TABLE IF EXISTS `coupons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `coupons` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Code` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `DiscountPercentage` int NOT NULL,
  `ExpiryDate` datetime(6) NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `IsUsed` tinyint(1) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) DEFAULT NULL,
  `UsedAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_coupons_UserId` (`UserId`),
  CONSTRAINT `FK_coupons_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `coupons`
--

LOCK TABLES `coupons` WRITE;
/*!40000 ALTER TABLE `coupons` DISABLE KEYS */;
/*!40000 ALTER TABLE `coupons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `emailverificators`
--

DROP TABLE IF EXISTS `emailverificators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `emailverificators` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` longtext NOT NULL,
  `Token` longtext NOT NULL,
  `ExpiryDate` datetime(6) NOT NULL,
  `IsUsed` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailverificators`
--

LOCK TABLES `emailverificators` WRITE;
/*!40000 ALTER TABLE `emailverificators` DISABLE KEYS */;
/*!40000 ALTER TABLE `emailverificators` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `memberships`
--

DROP TABLE IF EXISTS `memberships`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `memberships` (
  `MembershipID` int NOT NULL AUTO_INCREMENT,
  `Description` longtext NOT NULL,
  `DurationDays` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Name` longtext NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  PRIMARY KEY (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `memberships`
--

LOCK TABLES `memberships` WRITE;
/*!40000 ALTER TABLE `memberships` DISABLE KEYS */;
INSERT INTO `memberships` VALUES (1,'Testmitgliedschaft - Nur während der Testphase freigeschaltet',3,1,'Testmitgliedschaft',0.500000000000000000000000000000),(2,'1 Monat Mitgliedschaft - Perfekt für den Einstieg',30,1,'1 Monat',9.990000000000000000000000000000),(3,'3 Monate Mitgliedschaft - Ideal für längere Aufenthalte',90,1,'3 Monate',24.990000000000000000000000000000),(4,'1 Jahr Mitgliedschaft - Für regelmäßige Nutzer',365,1,'1 Jahr',79.990000000000000000000000000000),(5,'3 Jahre Mitgliedschaft - Langfristige Mitgliedschaft',1095,1,'3 Jahre',199.990000000000000000000000000000),(6,'Lebenslang Mitgliedschaft - Einmalig zahlen, lebenslang nutzen',36500,1,'Lebenslang',299.990000000000000000000000000000);
/*!40000 ALTER TABLE `memberships` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `offerapplication`
--

DROP TABLE IF EXISTS `offerapplication`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `offerapplication` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OfferId` int NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `ApplicationDate` datetime(6) NOT NULL,
  `Message` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_offerapplication_OfferId` (`OfferId`),
  KEY `IX_offerapplication_UserId` (`UserId`),
  CONSTRAINT `FK_offerapplication_offers_Id` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_offerapplication_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offerapplication`
--

LOCK TABLES `offerapplication` WRITE;
/*!40000 ALTER TABLE `offerapplication` DISABLE KEYS */;
/*!40000 ALTER TABLE `offerapplication` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `offers`
--

DROP TABLE IF EXISTS `offers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `offers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `Location` longtext,
  `GroupProperties` longtext,
  `AdditionalLodgingProperties` longtext,
  `LodgingType` longtext,
  `Skills` longtext,
  `Requirements` longtext,
  `SpecialConditions` longtext,
  `PossibleLocations` longtext,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `FromDate` datetime(6) NOT NULL,
  `ToDate` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `AccommodationSuitableId` int DEFAULT NULL,
  `AccommodationId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_offers_AccommodationId` (`AccommodationId`),
  KEY `IX_offers_AccommodationSuitableId` (`AccommodationSuitableId`),
  KEY `IX_offers_UserId` (`UserId`),
  CONSTRAINT `FK_offers_accomodations_AccommodationId` FOREIGN KEY (`AccommodationId`) REFERENCES `accomodations` (`Id`),
  CONSTRAINT `FK_offers_accommodationsuitables_AccommodationSuitableId` FOREIGN KEY (`AccommodationSuitableId`) REFERENCES `accommodationsuitables` (`Id`),
  CONSTRAINT `FK_offers_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offers`
--

LOCK TABLES `offers` WRITE;
/*!40000 ALTER TABLE `offers` DISABLE KEYS */;
/*!40000 ALTER TABLE `offers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `passwordresettokens`
--

DROP TABLE IF EXISTS `passwordresettokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `passwordresettokens` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Email` longtext NOT NULL,
  `Token` longtext NOT NULL,
  `ExpiryDate` datetime(6) NOT NULL,
  `IsUsed` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `passwordresettokens`
--

LOCK TABLES `passwordresettokens` WRITE;
/*!40000 ALTER TABLE `passwordresettokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `passwordresettokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pictures`
--

DROP TABLE IF EXISTS `pictures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pictures` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Url` longtext NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `OfferId` int DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_pictures_OfferId` (`OfferId`),
  KEY `IX_pictures_UserId` (`UserId`),
  CONSTRAINT `FK_pictures_offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_pictures_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pictures`
--

LOCK TABLES `pictures` WRITE;
/*!40000 ALTER TABLE `pictures` DISABLE KEYS */;
/*!40000 ALTER TABLE `pictures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Rating` int NOT NULL,
  `Comment` longtext,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `ReviewerId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReviewedUserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_reviews_ReviewedUserId` (`ReviewedUserId`),
  KEY `IX_reviews_ReviewerId` (`ReviewedUserId`),
  CONSTRAINT `FK_reviews_users_ReviewedUserId` FOREIGN KEY (`ReviewedUserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_reviews_users_ReviewerId` FOREIGN KEY (`ReviewerId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `shopitems`
--

DROP TABLE IF EXISTS `shopitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shopitems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `Price` decimal(18,2) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `MembershipId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_shopitems_MembershipId` (`MembershipId`),
  CONSTRAINT `FK_shopitems_memberships_MembershipId` FOREIGN KEY (`MembershipId`) REFERENCES `memberships` (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shopitems`
--

LOCK TABLES `shopitems` WRITE;
/*!40000 ALTER TABLE `shopitems` DISABLE KEYS */;
INSERT INTO `shopitems` VALUES (1,'Testmitgliedschaft','Testmitgliedschaft - Nur während der Testphase freigeschaltet',0.500000000000000000000000000000,1,1),(2,'1 Monat Mitgliedschaft','1 Monat Mitgliedschaft - Perfekt für den Einstieg',9.990000000000000000000000000000,1,2),(3,'3 Monate Mitgliedschaft','3 Monate Mitgliedschaft - Ideal für längere Aufenthalte',24.990000000000000000000000000000,1,3),(4,'1 Jahr Mitgliedschaft','1 Jahr Mitgliedschaft - Für regelmäßige Nutzer',79.990000000000000000000000000000,1,4),(5,'3 Jahre Mitgliedschaft','3 Jahre Mitgliedschaft - Langfristige Mitgliedschaft',199.990000000000000000000000000000,1,5),(6,'Lebenslang Mitgliedschaft','Lebenslang Mitgliedschaft - Einmalig zahlen, lebenslang nutzen',299.990000000000000000000000000000,1,6);
/*!40000 ALTER TABLE `shopitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skills`
--

DROP TABLE IF EXISTS `skills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `skills` (
  `Skill_ID` int NOT NULL AUTO_INCREMENT,
  `SkillDescrition` longtext,
  `ParentSkill_ID` int DEFAULT NULL,
  PRIMARY KEY (`Skill_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=141 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skills`
--

LOCK TABLES `skills` WRITE;
/*!40000 ALTER TABLE `skills` DISABLE KEYS */;
INSERT INTO `skills` VALUES (1,'Handwerk',NULL),(2,'Haushalt',NULL),(3,'Holz',1),(4,'Metall',1),(5,'Tapezieren',1),(6,'Wände streichen',1),(7,'Putzen',2),(8,'Aufräumen',2),(9,'Einkaufen',2),(10,'Kochen',2),(11,'zur Hand gehen - keine speziellen Kenntnisse notwendig',NULL),(12,'Tiersitting',NULL),(13,'Haussitting',11),(14,'Pferde',12),(15,'Kleintiere',12),(16,'Vögel oder Amphibien',12),(17,'Gartenarbeit',2),(18,'Trockenbau',1),(19,'Estrichleger',1),(20,'Fliesen verlegen',1),(21,'Installationsarbeiten Bad/Heizung/Sanitär',1),(22,'Installationsarbeiten Elektro',1),(23,'Zimmermann',1),(24,'Tischler',1),(25,'Dachdecker',1),(26,'Maurer',1),(27,'KFZ Reparatur',1),(28,'Informatiker',1),(29,'Backen',2),(30,'Kreativ',NULL),(31,'Marketing',30),(32,'Social Media',30),(33,'Texter',30),(34,'Grafiker',30),(35,'Fotografen',30),(36,'Journalisten',30),(37,'Webseite - Blog schreiben',30),(38,'Finanzen/Buchhaltung',NULL),(39,'Körpertherapie',NULL),(40,'Führerschein',NULL),(41,'Führerschein PKW',40),(42,'Führerschein LKW',40),(43,'Kettensägenschein vorhanden',1),(44,'Brennholz hacken',1),(45,'Landwirtschaft',NULL),(46,'Obst- und Gemüseanbau',45),(47,'Weinbau',45),(48,'Imkerei',45),(49,'Tierhaltung',45),(50,'Pferdepflege',49),(51,'Kuh- und Schafhaltung',49),(52,'Geflügelhaltung',49),(53,'Tourismus & Gastronomie',NULL),(54,'Rezeption',53),(55,'Kellner',53),(56,'Koch',53),(57,'Reiseführer',53),(58,'Sprachunterricht',53),(59,'Deutsch',58),(60,'Englisch',58),(61,'Französisch',58),(62,'Spanisch',58),(63,'Italienisch',58),(64,'Sport & Outdoor',NULL),(65,'Wandern',64),(66,'Klettern',64),(67,'Schwimmen',64),(68,'Skifahren',64),(69,'Surfen',64),(70,'Segeln',64),(71,'Fahrradreparatur',64),(72,'Bildung & Unterricht',NULL),(73,'Nachhilfe',72),(74,'Mathematik',73),(75,'Sprachen',73),(76,'Musikunterricht',72),(77,'Gitarre',76),(78,'Klavier',76),(79,'Gesang',76),(80,'Kunst & Design',30),(81,'Malen',80),(82,'Zeichnen',80),(83,'Fotografie',80),(84,'Videografie',80),(85,'Webdesign',80),(86,'Handwerk Spezial',1),(87,'Schmuckherstellung',86),(88,'Töpfern',86),(89,'Holzschnitzerei',86),(90,'Lederarbeiten',86),(91,'Nähen & Schneidern',86),(92,'Stricken & Häkeln',86),(93,'Gesundheit & Wellness',NULL),(94,'Yoga',93),(95,'Pilates',93),(96,'Meditation',93),(97,'Erste Hilfe',93),(98,'Pflege',93),(99,'Seniorenbetreuung',98),(100,'Kinderbetreuung',98),(101,'Bau & Renovierung',1),(102,'Dachbodenausbau',101),(103,'Kellerausbau',101),(104,'Balkonbau',101),(105,'Terrassenbau',101),(106,'Zaunbau',101),(107,'Carportbau',101),(108,'Gartenhausbau',101),(109,'Technik & IT',NULL),(110,'Computerreparatur',109),(111,'Smartphone-Reparatur',109),(112,'Netzwerk-Installation',109),(113,'Smart Home',109),(114,'Solaranlagen',109),(115,'Transport & Logistik',NULL),(116,'Umzugshilfe',115),(117,'Lieferfahrten',115),(118,'Möbeltransport',115),(119,'Veranstaltungen',NULL),(120,'Eventplanung',119),(121,'Dekoration',119),(122,'Musik bei Events',119),(123,'Fotografie bei Events',119),(124,'Catering',119),(125,'Reinigung & Wartung',NULL),(126,'Poolreinigung',125),(127,'Saunawartung',125),(128,'Kaminreinigung',125),(129,'Dachrinnenreinigung',125),(130,'Winterdienst',125),(131,'Buchhaltung',38),(132,'Steuererklärung',38),(133,'Lohnbuchhaltung',38),(134,'Controlling',38),(135,'Massage',39),(136,'Jin Shin Jyutsu',39),(137,'Fußreflexzonenmassage',39),(138,'Shiatsu',39),(139,'Thai-Massage',39),(140,'Exoten',12);
/*!40000 ALTER TABLE `skills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transactions`
--

DROP TABLE IF EXISTS `transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ShopItemId` int NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Status` int NOT NULL,
  `PaymentIntentId` longtext,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_transactions_ShopItemId` (`ShopItemId`),
  KEY `IX_transactions_UserId` (`UserId`),
  CONSTRAINT `FK_transactions_shopitems_ShopItemId` FOREIGN KEY (`ShopItemId`) REFERENCES `shopitems` (`Id`),
  CONSTRAINT `FK_transactions_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transactions`
--

LOCK TABLES `transactions` WRITE;
/*!40000 ALTER TABLE `transactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usermembership`
--

DROP TABLE IF EXISTS `usermembership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usermembership` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `MembershipId` int NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_usermembership_MembershipId` (`MembershipId`),
  KEY `IX_usermembership_UserId` (`UserId`),
  CONSTRAINT `FK_usermembership_memberships_MembershipId` FOREIGN KEY (`MembershipId`) REFERENCES `memberships` (`MembershipID`),
  CONSTRAINT `FK_usermembership_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usermembership`
--

LOCK TABLES `usermembership` WRITE;
/*!40000 ALTER TABLE `usermembership` DISABLE KEYS */;
/*!40000 ALTER TABLE `usermembership` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userprofiles`
--

DROP TABLE IF EXISTS `userprofiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userprofiles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `User_Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserPic` longblob,
  `Options` int NOT NULL,
  `Hobbies` longtext,
  `Token` longtext,
  `Skills` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_userprofiles_User_Id` (`User_Id`),
  CONSTRAINT `FK_userprofiles_users_User_Id` FOREIGN KEY (`User_Id`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userprofiles`
--

LOCK TABLES `userprofiles` WRITE;
/*!40000 ALTER TABLE `userprofiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `userprofiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `User_Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `FirstName` longtext NOT NULL,
  `LastName` longtext NOT NULL,
  `DateOfBirth` date NOT NULL,
  `Gender` longtext NOT NULL,
  `Email_Address` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `SaltKey` longtext,
  `IsEmailVerified` tinyint(1) NOT NULL,
  `MembershipId` int DEFAULT NULL,
  `Facebook_link` longtext,
  `Link_RS` longtext,
  `Link_VS` longtext,
  `VerificationState` int NOT NULL,
  `About` longtext,
  `Hobbies` longtext,
  `ProfilePicture` longblob,
  `Skills` longtext,
  `UserRole` int NOT NULL DEFAULT '0',
  `IsTwoFactorEnabled` tinyint(1) DEFAULT '0',
  `TwoFactorSecret` text,
  `BackupCodes` text,
  `AddressId` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`User_Id`),
  KEY `IX_users_MembershipId` (`MembershipId`),
  CONSTRAINT `FK_users_memberships_MembershipId` FOREIGN KEY (`MembershipId`) REFERENCES `memberships` (`MembershipID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (User_Id, FirstName, LastName, DateOfBirth, Gender, AddressId, Email_Address, Password, SaltKey, IsTwoFactorEnabled, IsEmailVerified, MembershipId, Facebook_link, Link_RS, Link_VS, Hobbies, Skills, ProfilePicture, About, VerificationState, UserRole) VALUES
('00000000-0000-0000-0000-000000000001','Holger','Admin','1990-01-01','Male',1,'admin@alreco.de','tgfj5VRwZEDOvr4YsCeu9KrVS052yz2E9h+fTmweRew=','0SHED4v9YfP/mpFDVu1isKK6LMTB6z2oBicgfRkAR1M=',0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_SQL_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-28 15:56:53 
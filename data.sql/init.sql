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
  `NameAccommodationType` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accomodations`
--

LOCK TABLES `accomodations` WRITE;
/*!40000 ALTER TABLE `accomodations` DISABLE KEYS */;
INSERT INTO `accomodations` VALUES (1,'Hütte'),(4,'Zelt'),(5,'Wohnmobil'),(6,'Zimmer'),(7,'Bauwagen'),(8,'Tiny House'),(9,'Boot'),(10,'Baumhaus'),(11,'Scheune'),(12,'Wohnwagen'),(13,'Scheune'),(14,'Wohnwagen');
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
  `Latitude` double NOT NULL,
  `Longitude` double NOT NULL,
  `DisplayName` varchar(255) NOT NULL,
  `HouseNumber` varchar(50) DEFAULT NULL,
  `Road` varchar(255) DEFAULT NULL,
  `Suburb` varchar(255) DEFAULT NULL,
  `City` varchar(255) DEFAULT NULL,
  `County` varchar(255) DEFAULT NULL,
  `State` varchar(255) DEFAULT NULL,
  `Postcode` varchar(50) DEFAULT NULL,
  `Country` varchar(255) DEFAULT NULL,
  `CountryCode` varchar(10) DEFAULT NULL,
  `OsmId` bigint DEFAULT NULL,
  `OsmType` varchar(20) DEFAULT NULL,
  `PlaceId` varchar(50) DEFAULT NULL,
  `Type` int DEFAULT '0',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addresses`
--

LOCK TABLES `addresses` WRITE;
/*!40000 ALTER TABLE `addresses` DISABLE KEYS */;
INSERT INTO `addresses` VALUES (1,52.520008,13.404954,'Brandenburger Tor, Pariser Platz, 10117 Berlin, Deutschland',NULL,NULL,NULL,'Berlin',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:30:26',NULL),(2,48.137154,11.576124,'Marienplatz, 80331 M????nchen, Deutschland',NULL,NULL,NULL,'M????nchen',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:31:54',NULL),(3,50.110924,8.682127,'R????merberg, 60311 Frankfurt am Main, Deutschland',NULL,NULL,NULL,'Frankfurt',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:33:30',NULL),(4,51.4940976,10.3490711,'Hauptstra├ƒe, Brehme, 37339, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 08:55:39',NULL),(5,50.7048536,11.9319282,'L 2331, W├Âhlsdorf, 07955, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 08:56:17',NULL),(6,49.7263755,6.9410144,'K 95, Rascheid, 54413, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 08:58:18',NULL),(7,50.7220839,9.5800173,'L 3140, Rimbach, 36110, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 08:59:58',NULL),(8,50.7078267,8.5491813,'L 3047, Wilsbach, 35649, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:04:19',NULL),(9,49.6079511,11.3401361,'Am Hopfenbeet, B├╝hl, 91245, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:08:00',NULL),(10,46.7544268,1.7360079,'All├®e Foresti├¿re de Brocart, Le Poin├ºonnet, 36330, France',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:09:30',NULL),(11,50.666543,10.0412099,'Kastanienweg, Theobaldshof, 36142, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:11:27',NULL),(12,50.8470699,10.4389168,'Rennsteig, Brotterode, 99880, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:13:59',NULL),(13,51.1137915,10.0425207,'B 7, R├Âhrda, 37296, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:17:34',NULL),(14,51.2884595,8.986093,'K 14, H├Âringhausen, 34513, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:21:48',NULL),(15,49.3922485,20.1348744,'Trybsz, 34-405, Polska',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:28:17',NULL),(16,49.4398264,7.90674,'Am Meisenkopf, Hochspeyer, 67691, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 09:39:11',NULL),(17,49.5808281,9.2137779,'MIL 42, Preunschen, 63931, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 11:44:45',NULL),(18,48.3926687,9.3219298,'Hasenbergweg, 72829, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 12:15:58',NULL),(19,50.5975074,10.1737221,'Alte Chaussee, Kaltennordheim, 36452, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 15:05:03',NULL),(20,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 16:18:03',NULL),(21,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 16:30:31',NULL),(22,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 16:34:51',NULL),(23,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 16:35:04',NULL),(24,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 16:52:20',NULL),(25,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:10:18',NULL),(26,49.7155794,13.7210872,'Horsk├¢ sjezd z Bahen, Dob┼Ö├¡v, 338 44, ─îesko',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:10:28',NULL),(27,51.6325181,9.7551652,'B 241, Hardegsen, 37181, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:44:25',NULL),(28,51.6325181,9.7551652,'B 241, Hardegsen, 37181, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:45:02',NULL),(29,51.6325181,9.7551652,'B 241, Hardegsen, 37181, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:49:05',NULL),(30,51.6325181,9.7551652,'B 241, Hardegsen, 37181, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:49:13',NULL),(31,51.6325181,9.7551652,'B 241, Hardegsen, 37181, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,2,'2025-07-27 17:53:25',NULL),(32,50.9924496,9.7711081,'Hinter den Z├ñunen, Lispenhausen, 36199, Deutschland',NULL,NULL,NULL,'Lispenhausen',NULL,NULL,'36199','Deutschland',NULL,NULL,NULL,NULL,0,'2025-07-28 08:33:00',NULL);
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
  `Name` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `CreatedBy` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `Duration` int NOT NULL DEFAULT '0',
  `MembershipId` int NOT NULL DEFAULT '0',
  `IsEmailSent` tinyint(1) NOT NULL DEFAULT '0',
  `EmailSentDate` datetime DEFAULT NULL,
  `EmailSentTo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_coupons_CreatedBy` (`CreatedBy`),
  KEY `IX_coupons_MembershipId` (`MembershipId`),
  CONSTRAINT `FK_coupons_memberships_MembershipId` FOREIGN KEY (`MembershipId`) REFERENCES `memberships` (`MembershipID`) ON DELETE CASCADE,
  CONSTRAINT `FK_coupons_users_CreatedBy` FOREIGN KEY (`CreatedBy`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `coupons`
--

LOCK TABLES `coupons` WRITE;
/*!40000 ALTER TABLE `coupons` DISABLE KEYS */;
INSERT INTO `coupons` VALUES (14,'01JY41P08Z4XX96ZRRFHKBPF74','Admin Issued Coupon','','2025-06-19 12:12:40.613457','08dcd23c-d4eb-45db-88e4-73837709fada',0,1,0,NULL,NULL),(15,'01K0H48SRHYSAE3EZY0P6MB5ES','Admin Issued Coupon','','2025-07-19 10:39:24.950469','08dcd23c-d4eb-45db-88e4-73837709fada',365,1,0,NULL,NULL),(16,'TEST-COUPON-2025','Test Coupon for Maps','Test coupon for Maps functionality testing','2025-07-24 11:01:31.000000','08dcd23c-d4eb-45db-88e4-73837709fada',365,1,0,NULL,NULL);
/*!40000 ALTER TABLE `coupons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `emailverificators`
--

DROP TABLE IF EXISTS `emailverificators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `emailverificators` (
  `verificationId` int NOT NULL AUTO_INCREMENT,
  `user_Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `verificationToken` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `requestDate` datetime(6) NOT NULL,
  PRIMARY KEY (`verificationId`)
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailverificators`
--

LOCK TABLES `emailverificators` WRITE;
/*!40000 ALTER TABLE `emailverificators` DISABLE KEYS */;
INSERT INTO `emailverificators` VALUES (70,'08ddaf2a-6964-47e5-8289-e714c41cd281','3f777390-713b-41fe-a733-876e18af9a43','2025-06-19 12:11:25.132245'),(71,'08ddb25b-e299-4f51-85fc-8f2b4137e075','926ceaea-0a90-45d6-bc1c-678c30ba2093','2025-06-23 13:43:07.183591'),(72,'08ddb25d-1ddf-467b-803a-18787c1aabdf','d521594f-9e17-44d6-83d5-b282c69a3ce3','2025-06-23 13:51:56.161974'),(73,'08ddbf1b-fb2a-4348-84e2-6b8f1aa93ef0','9206aa05-35f4-4cb7-b745-b4be0e21336b','2025-07-09 19:08:25.733209'),(74,'08ddbf22-79b7-4538-8827-fe47428fadbf','4f914999-030b-4edc-9c29-270f533cc5b4','2025-07-09 19:54:54.838002'),(75,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','f0292fd4-3e3c-4339-ace3-18fc26381ef7','2025-07-24 08:47:02.467658'),(76,'08ddcaa2-1746-48b7-8bc9-bd480646c637','11fc1342-f522-4af8-a19a-979e0509ecf1','2025-07-24 11:06:08.217272'),(77,'08ddcaa2-29e6-43ad-82e3-10ef2dd5cbac','b4c2cca3-07f7-4577-9c08-7e1b27180593','2025-07-24 11:06:39.304954'),(78,'08ddcaa2-3c58-4141-8163-7522d982cecf','5b85d471-1d1a-45e0-987b-4bdee02dfccb','2025-07-24 11:07:10.248777'),(79,'08ddcaa2-4a7d-4c0c-8263-cbd60d39f367','313dc087-b822-4f36-b8b2-efbd133201e1','2025-07-24 11:07:33.981640'),(80,'08ddcaa2-9305-43b5-8e84-b51d1aa9685f','c06500b0-ceff-4ca5-bf72-aac69e218b66','2025-07-24 11:09:35.669016'),(81,'08ddcdb1-5ca1-4e25-8271-0fc1c3b3601b','1d590b7e-e90c-49e8-9944-0cd3676fbc64','2025-07-28 08:33:00.523627');
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
  `IsActive` tinyint(1) NOT NULL DEFAULT '0',
  `Name` varchar(100) NOT NULL DEFAULT '',
  `Price` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `memberships`
--

LOCK TABLES `memberships` WRITE;
/*!40000 ALTER TABLE `memberships` DISABLE KEYS */;
INSERT INTO `memberships` VALUES (1,'Perfect for individuals or small businesses just starting out. The Basic Package provides essential access to our services',30,1,'Basic',4.990000000000000000000000000000),(2,'N/A',0,0,'Default',0.000000000000000000000000000000),(3,'Ideal for growing businesses that need more support and flexibility. The Standard Package offers enhanced features',60,1,'Standard',7.990000000000000000000000000000),(4,'Designed for established businesses looking for comprehensive support and maximum benefits. The Premium Package includes',120,1,'Deluxe',9.990000000000000000000000000000);
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
  `HostId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_offerapplication_HostId` (`HostId`),
  KEY `IX_offerapplication_OfferId` (`OfferId`),
  KEY `IX_offerapplication_UserId` (`UserId`),
  CONSTRAINT `FK_offerapplication_offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_offerapplication_users_HostId` FOREIGN KEY (`HostId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_offerapplication_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=131 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `Title` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Location` longtext COLLATE utf8mb4_unicode_ci,
  `GroupProperties` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `AdditionalLodgingProperties` longtext COLLATE utf8mb4_unicode_ci,
  `LodgingType` longtext COLLATE utf8mb4_unicode_ci,
  `Skills` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Requirements` longtext COLLATE utf8mb4_unicode_ci,
  `SpecialConditions` longtext COLLATE utf8mb4_unicode_ci,
  `PossibleLocations` longtext COLLATE utf8mb4_unicode_ci,
  `UserId` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreatedAt` date NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `FromDate` date NOT NULL DEFAULT '0001-01-01',
  `GroupSize` int NOT NULL DEFAULT '0',
  `Mobility` int NOT NULL DEFAULT '0',
  `ModifiedAt` date NOT NULL DEFAULT '0001-01-01',
  `OfferType` int NOT NULL DEFAULT '0',
  `Status` int NOT NULL DEFAULT '0',
  `ToDate` date NOT NULL DEFAULT '0001-01-01',
  `AddressId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_offers_UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=129 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offers`
--

LOCK TABLES `offers` WRITE;
/*!40000 ALTER TABLE `offers` DISABLE KEYS */;
INSERT INTO `offers` VALUES (125,'Ferienhaus am See','Sch├â┬Ânes Ferienhaus direkt am See mit Bootsverleih',NULL,'Familie mit Kindern',NULL,NULL,'W├â┬ñnde streichen, Gartenarbeit','Mindestens 2 Wochen Aufenthalt',NULL,NULL,'08dcd23c-d4eb-456b-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',4,0,'2025-07-27',1,0,'2025-08-26',NULL),(126,'Alpine H├â┬╝tte','Traditionelle Bergh├â┬╝tte mit Panoramablick',NULL,'Wanderer, Bergsteiger',NULL,NULL,'Holz hacken, Kamin reinigen','Erfahrung im Umgang mit Holz├â┬Âfen',NULL,NULL,'08dcd23c-d4eb-45db-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',6,1,'2025-07-27',1,0,'2025-09-10',NULL),(127,'Stadtwohnung Zentrum','Moderne Wohnung im Herzen der Stadt',NULL,'Paare, Singles',NULL,NULL,'Putzen, Einkaufen, Haustiere versorgen','Keine Haustiere erlaubt',NULL,NULL,'08dcd23c-d4eb-45db-88e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',2,2,'2025-07-27',1,0,'2025-08-16',NULL),(128,'Mein tolles Haus','Kommt mich besuchen',NULL,'','Boot',NULL,'Metall','Senioren, Gruppen',NULL,NULL,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','2025-07-27','OfferTypeLodging','2025-01-01',0,0,'0001-01-01',0,0,'2025-08-08',31);
/*!40000 ALTER TABLE `offers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `passwordresettokens`
--

DROP TABLE IF EXISTS `passwordresettokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `passwordresettokens` (
  `TokenId` int NOT NULL AUTO_INCREMENT,
  `user_Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Token` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `requestDate` datetime(6) NOT NULL,
  PRIMARY KEY (`TokenId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `Hash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Width` int NOT NULL,
  `ImageData` longblob NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `OfferId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_pictures_UserId` (`UserId`),
  KEY `IX_pictures_OfferId` (`OfferId`),
  CONSTRAINT `FK_pictures_offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`),
  CONSTRAINT `FK_pictures_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pictures`
--

LOCK TABLES `pictures` WRITE;
/*!40000 ALTER TABLE `pictures` DISABLE KEYS */;
(12,'0728722FC56E6BB2DA0FEC7CD7B546BC',100,_binary ' \Ï \Ó\0JFIF\0\0\0\0\0\0 \█\0C\0	\n\n			\n\n		\r\r\n \█\0C	 └\0yÉ\0 \─\0\0\0\0\0\0\0\0\0\0\0\0\0	 \─\0O\0	\0\0\0Wv\È!67TÆôòû┤Á\Ë1QUVauæ\ÐA\"#23Bq$&\'5Cbtü│%EFRSí \─\0\Z\0\0\0\0\0\0\0\0\0\0\0\0\0 \─\0%\0\0\0\0\0\0\0\0\0!1Aq\"2æ▒ \┌\0\0\0?\0\´[Gh\Ý\\]½úóó¡Ñ(\§\¾\┌~\\\█Ãì\╔!Uª\┌Xgj¬¬«\±UUU\▀À3vôE\¶ÅQ\┬¨`37i4_H\§/û3vôE\¶ÅQ\┬¨`37i4_H\§/û3vôE\¶ÅQ\┬¨`37i4_H\§/ûmª┤Â▓2ô|·*\┌ÊÅ[\¦\╔\█9MI!UrYÜE2\╩w\ÞDDDO\┘,\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥X\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥X\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥X\È5ñÁq%|\Ý\§ÁÑe\┼F\Ý█ñjI\n¿\├;ò/k■×\‗b\ËK■j½¹ücÖ╗Ió·G¿\ß|░ø┤Ü/ñzÄ\╦Ö╗Ió·G¿\ß|░ø┤Ü/ñzÄ\╦Ö╗Ió·G¿\ß|░ø┤Ü/ñzÄ\╦BYh\Ý[Wjñäj\┌Êè\Õ\¦;$x├ÁÆB\õ▓\ËQ34iQ?/UFYE_¨S\È~f\Ý&ï\Úúà\‗└fn\Êh¥æ\Û8_,f\Ý&ï\Úúà\‗└fn\Êh¥æ\Û8_,f\Ý&ï\Úúà\‗└fn\Êh¥æ\Û8_,		ØúÁl¦¬nøkJ#ùö\ý\Ý\Òn\ÊIÆ\ËL\─\╦òT³╝Q\Zi\µ_X¨ø┤Ü/ñzÄ\╦Ö╗Ió·G¿\ß|░ø┤Ü/ñzÄ\╦Ö╗Ió·G¿\ß|░ø┤Ü/ñzÄ\╦Ö╗Ió·G¿\ß|░#½øIj\ß\ÛJ█økJ0\╦·ìÒÀ¿╠ÆÂw*`\Í²=\¶┼ûW³\Ð\÷\Ã3vôE\¶ÅQ\┬¨`37i4_H\§/û3vôE\¶ÅQ\┬¨`37i4_H\§/û3vôE\¶ÅQ\┬¨`37i4_H\§/ûm┘┤Â▓ôr·\┌ÊÄø\¦\╔#9L\╔!QrZÜB▓\Êw\ÞTUEO\¦P,\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥X\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥X\═\┌M\Ê=G\ÕÇ\╠¦ñ\Ð}#\Èp¥XWr\Ð┌©KWX\┼B\█JQ\Ë\þT³┼ÀoÆBúL4É\¤Ôèèë¥mÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@Y«)(¡]ûwW@XÇ\0\0\0\0!\ý\´ƒ|·{\Ô\Ð`\\\0\0\0\0\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿`\0\0\0\0╔»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢f©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ð#m$▓\´Àj\Þ\µí+\n9█òº\Õ\╩\Ýå\ÚÏÂÜeƒ\Ë;┴ñÄDU\├\r\¶D \0$│r\´?¥ö_fó\÷\­ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/o╣wƒ\▀J/│Q{xÍ×[wøñ\▀5	WQ╬Ø\¯\õ\´¨[ºb\┌\\¡Èè\╩\\\\ø\╩\Í*ë¹\"ó*«¿X\¯]\þ\¸Êï\ý\È^\Ìr\´?¥ö_fó\÷\­ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/oBåû\¦\µ¬:¨U\¶s\r│Q╗G¬\ı;\Ê4\Ì\Õ└o▓ƒ«Lö\╔\╔L\¶U\Ã\n²╦╝■·Q}Üï\█└n]\þ\¸Êï\ý\È^\Ìr\´?¥ö_fó\÷\­ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\ÓH\╦%ù}n\ıH\╦5àÅÊ×Æ+m¡;¼½?®Ö\õó3·\ýQQQ¼W\Ã\ÌL7┬╗r\´?¥ö_fó\÷\­ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/o╣wƒ\▀J/│Q{x39e\▀KÁM▓\ıaG½\§ººj\├iN┼ú(\¤\ÛeÖH¼■╗UUg\┼0┴wù\Ó«▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/o╣wƒ\▀J/│Q{x\r╦╝■·Q}Üï\█└n]\þ\¸Êï\ý\È^\Ìàs-╗\╠\Èt\n?½\Þ\µ\█júxÄòÜv-öe¢╦Å\▀i?\\╣Iôöÿ&ÛïÄ\÷\n¹ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/o╣wƒ\▀J/│Q{x\r╦╝■·Q}Üï\█└Ä╗█╝\┼&Õ¿║║Äz\´w$ƒ\╩\┼;\╩\Õnñ.J\Ò·\Õ\ÌF░UO\¦Q1\┼\Ãr\´?¥ö_fó\÷\­ùy²\¶ó¹5ÀÇ▄╗\¤\´Ñ┘¿¢╝\Õ\Ì}(¥\═E\Ý\Ó7.\¾¹\ÚE\÷j/o╣wƒ\▀J/│Q{xwnYwÏÁuïQuà\±\╩S\¾x\├\ý[-4\¤\Ú×bê\Ê\Ã*\"\ßÄ·ó \0Æüef©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@Y«)(¡]ûwW@XÇ\0\0\0\0!\ý\´ƒ|·{\Ô\Ð`\\\0\0\0\0\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿`\0\0\0\0╔»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢f©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶{Iz-\╠¼ú\ß¤×ú\Ã2	sÂ\Ð%\ÐkâI\Ý}`╗\Þf|mºÀ\▀ul_ö>6\Ë\█´║Â/\╩ƒi\Ý\¸\¦[\Õ\0¤ì┤\÷¹¯¡ï\‗Çg\ã\┌{}\¸V\┼¨@3\Òm=¥¹½b³á#¡E\µÀ0Tú\þº\´2Ànv\┌d╦ó\ıûªæM2┐\¦zò\0▒¤ì┤\÷¹¯¡ï\‗Çg\ã\┌{}\¸V\┼¨@3\Òm=¥¹½b³á\±Â×\▀}ı▒~P°\█Oo¥\ÛÏ┐(|mºÀ\▀ul_öày\Ý\╠=G^╝y>yôQ;z\´	tZ\Ô\╬\Õ└3 \0Õ¢¥╩ü_ƒi\Ý\¸\¦[\Õ\0¤ì┤\÷¹¯¡ï\‗Çg\ã\┌{}\¸V\┼¨@3\Òm=¥¹½b³á\±Â×\▀}ı▒~P°\█Oo¥\ÛÏ┐(	e\ÞÀ,¦ÜÄ1g»)\Ý=%v\╩\¯t^9L\─\╠\ıw┐+\±▓v|mºÀ\▀ul_ö>6\Ë\█´║Â/\╩ƒi\Ý\¸\¦[\Õ\0¤ì┤\÷¹¯¡ï\‗Çg\ã\┌{}\¸V\┼¨@3\Òm=¥¹½b³á$fwó▄Ávi\╚─×¢³ºT\¶\Ú\█K╣\Ðx\Õ5,T\Ì³¼└\ð\┘\±Â×\▀}ı▒~P°\█Oo¥\ÛÏ┐(|mºÀ\▀ul_ö>6\Ë\█´║Â/\╩ƒi\Ý\¸\¦[\Õ\0¤ì┤\÷¹¯¡ï‗ÇÉ«/=╣ê¿\Þ\'Ä\þ\¤2a\Û\'Å^c.ïL▄©\÷³À\¸\┌@+\¾\Òm=¥¹½b³á\±Â×\▀}ı▒~P°\█Oo¥\ÛÏ┐(|mºÀ\▀ul_ö>6\Ë\█´║Â/\╩ƒi\Ý\¸\¦[\Õu\´5╣ìÑ©s?yò╗rF\Î*]êî│4àiÑ■\ÙÈèÄ|mºÀ\▀ul_ö>6\Ë\█´║Â/\╩ƒi\Ý\¸\¦[\Õ\0¤ì┤\÷¹¯¡ï\‗Çg\ã\┌{}\¸V\┼¨@3\Òm=¥¹½b³á$\¯\¦\ÞÀ1û▓▒äq>z»H&.\ÏEùEª-,3\─M\§uéo¿\÷q2¡%¬¬┐\Û\ý│\¸^J\ýö°²@dº\Ã\Û%>?P)\±·Ç\╔OÅ\ÈJ|~áD\┘\§i║A¹M┤\ËJ│\Ú\÷*¡/Áó└Â\╔OÅ\ÈJ|~á2S\Ò\§Æƒ¿ö°²@dº\Ã\Û]ïU=\├┼ªù\nÖ\┌zW\┘2\Ó-2S\Ò\§Æƒ¿ö°²@dº\Ã\Û%>?P)\±·ü+O\÷\┼Sª+ç\­▄ï\¸^S5\¤%>?P)\±·Ç\╔OÅ\ÈJ|~á2S\Ò\§Æƒ¿ôD \0lT\┬b©\r\¤u\Õ2á,\‗S\Ò\§Æƒ¿ö°²@dº\Ã\Û%>?P)\±·ü_b\═Oo0iñãªx×ò\÷L\─Lö°²@dº\Ã\Û%>?P)\±·Ç\╔OÅ\ÈJ|~áD\ÌièA\├L4\Ë*ô\Ú\nì/Áí\0Â\╔OÅ\ÈJ|~á2S\Ò\§Æƒ¿ö°²@dº\Ã\Ûu\ÒLøIZ¬*º·╗3²ÎÆ╝\µ\═qIEjý│║║\─\0\0\0\0gx¹\þ\Ë\▀ï\Ó\0\0\0\0@p×\Ôk3┐	ùj\0\0\0\0\0îòq\┼S\Û▄ï╝\═@│\0\0\0\0\0FMx\Ôª5n{\Ìe@YÇ\0\0\0\0\"½■█¢fy\ß3-@\0\0\0\0\¸ïü\╬~}\"\±h@.\0\0\0\0\0ÎùèJ\ÎWf}\ı\Þ5\┼%½▓\╬\Û\Þ\0\0\0\0=Ø\Ós\´ƒO|Z,Ç\0\0\0\0U\┬{ë¼\╬³&\\¿\0\0\0\02U\ÃO½r.\¾5\╠\0\ı\õÆP\ı$\"\¦7TO&▒É-╠ÖçÆJ[îiêv^#Ám╝òLö\╩i■\µ&~ep^1¹ffc}F·t>/ËÖ³º\Z\▄\╚╦Å:\┌+╝ùè■\Úì\Û7\¾\È3%─ª«$T\\ü\ýSÀ\‗Þàäÿ@\ÃB╝àîü~îú_û¨\╦\─Fÿ\\ûæQp┴ñTTUM\‗\╠èr\"fƒ]LOSÿay_\╩\­¨+NDD┼ú\¦[Vbı┤|nÂì\─\¸\È²\─\§1º/j└#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\rV\Í4à°ØûL+*ªO\"à}A┼╣t■g\Û\█o7E\╩\õ▓\Ëãæ¼W\▀┴\ı\µ═Å:\'-ó#\┘?3»©w^?\Ãs<ùÑ▓b\ßbÂKG\"│1J═ª#\¶\ÝÌó\'º▓\Ê\═%\§ÑÌ©7ö|\─U3(öC\╠\\\´\├\╠b\ßÆ!º\¤5\Þx\╦¨█╝┤\┼YTE\\Æ\\KFnFL\Ï¹¼\┼cS1¢\Ù¹\Í\È¨³<oå\ßx\¯\\k<[-\µ│\¾J\▀\┘ïG\È\╠\Ím®\´S1\█r\'5Òèÿı╣\´yòf\0\0\0\0\0è»°On\§Ö\þä\╠@Á\0\0\0\0\0C\Ì.9¨\¶ï┼í\0©\0\0\0\0\0G^^)+]]Ö\¸Wá,\ÎöV«\╦;½á,@\0\0\0\0\÷wü¤¥}=\±h░.\0\0\0\0\0T	\¯&│;\­Öpá\0\0\0\0\╔WU>¡╚╗\╠\È0<0S╣D\┼\┬D└╠íƒ║iû×#L=EEek³▒OOá,C4£\‗)\õ<[2¿°êG,>mçî╗zËºMÔ¼┤©\ÒÆ\╩õ¬ó·6Ñm³úk▒rs`ìb╝\Î\±3\Ò\¶ç×\Ë\ý\┼3\'äÿBú\Îl╗F\\╗i0eûÖUcÐ╝ê¿\╩\ß■[─ó5\È*ÁªË╗OoDT\ÔU\╩70çt╩┤\ÝöV×\'ÑÂÐå>¡¬2ƒ\Ó\±\ý2k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@Y«)(¡]ûwW@XÇ\0\0\0\0!\ý\´ƒ|·{\Ô\Ð`\\\0\0\0\0\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿/Gî4\ÝUQ\ZELQpT\─\rSbØ¼?Ùºê\─[¿gP\═6\µ\ZeÂ]8a\ËÊ«\n\Ë¨l┤\Ë╝©┤ƒ32\ÙR─▓y\r	=Wm\¤`Öçx\±ê6W/\Ò\µ\ı]ª8#┐\Ù+(\´\÷FQá0Éûaûñzèî=³ªØBú?É\±Y{¨m░èÊ¬+\r=FÖ\┼W¹┤\┼w\ı@²_\Ï\╚g?£▓\┘\├_Îäçâ_\╬eñWl▓«Yz\█*\╦Xb\Ë.U¼òg¹m*\Õ\"Áx┘»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢*\Ê\█\Ú\õE¼úƒ╗╗5øû^H%\═#ª@d░ï\ÝrSE\\7\ıT\n\╠\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\÷ª\▀\╬\ÔiG\´]è\Ð\├;╣;g!ùÉ\nø\ËHñU▀ä]\§T\┼lUpDM\Ó,3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`HP\÷■x·ú»Xb\ý\Ín\ı\ıDÝåÜe\õ/r\Ó)¼a=8*&\÷	â)¹\Ô¬¨©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0$Ñû·x\ı┘¿\ß\Ê\ý\Íh\Ë\¶òÁzÅ 2ÜEëÖ\Ó\╩ \0óaéd¬ª	Å\¾.8\´`╣©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0$ªv·x\═┘º!\Í\ý\Íj\Ët\¶\Ú┤z» 2ÖDëûb\╩óaé\Õ\"«)Å\‗ªo\Ô╣©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0©ƒÚéÂ\þ\╦\÷0$+ï<sQ\ðL7vk7è\÷óx\├-4\‗k╣q\Ùö\╬×£S┴Ñ²\­T\n³\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\▄O\¶┴[s\Õ¹\¸Z\▀\╬\ß®G¦è\Ð¹;╣$g!ºÉø\¾HTEÌäM\¶U\┼?lQ1EM\Ó,3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`3q?\Ëm¤ù\ý`I¦½}<çÁòï\¸ùf│|╦╣┼Ñt\█\╚û\Ð!×.J\ßïé\ßå\‗óükf©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇj\Ô\\JN\ıRQu¢m■Q\±├º»A¥ïz¡¥z├ºL0\Õ\├\r╝mª×<aöFYU\┼@└P\÷\┘\\Ö\¾\ÛVØÖ\═aºnaÜì\▄\╔▄å>O\‗ûæåƒ;u\Z\Õ\ËOXeªÖeZaVæP\rê\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇC■6ò·~ú \0K5s+³AM■Ts\þl╝w\n\Ì\ý┴\õ¢iå\ıi\\\ZVUQS\Ê;1®e┐è+u\rY^)ÎïÖJg░p,\╩\Õ\¯ S¼#ÀO_F<b\¾\Èz\Ú\Û╣u¬┌│ÆÊ▒ôÄS@u¿\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüå╝\Î\÷ô▓╗ÿ\µ{0ÅèÜ+m;q\╦L:aQxÊÂ\Ê\'ÑñDD\▀U·üåñ┐ûÌ┤©p\÷■J\µd\┌ãªL$┴ÀL│¨\¯BÂ¼`½ûø╚®èª\n¿®\ÛU\Ý■«-\Õ@╝n\õ\Ë.j9\\\═¹i)}\Û%\▄[\═\÷\Ð\Za\¸\¶\­g#+¢&°®¿+ \0°cóþ▓®=ÀÂPT\ÔTh\ßêÏëLÄêw¡ÁÆ├À\Þ\ÚV\ı\Z\¶¬e\"\"óª?░t\r└«dÂÔæÿVS \0\╬X){½L9e\Zx\±ªÜFXaöUD\┼Zi}Q7\¸└Ê»?V╣È¬_▓i\ÙQ/[b*çNı©FT■vòZ\╔mQ\\W\Ã7\├f\¦8\ÞiØ1éy¨É\±Sè}\¾ª\­■\Ë\rM`\┌e~èÇ_Ç\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿü?°é▒\‗¹\═L0\Ú\╦ÈàƒJæ\Ò\╔dJ»\‗+M\"e:y\Ùa¼ö\▀\¶▓¿èƒ║(k_\┬\´\ß×:îìf\ß\▄8$s9t¡▒-ù┤\Ê5·D\\Yi\ÙjÿóÂ®è2ëè2\╩\Ò\Ú]\Ó\¦wj\ÎH«\Õ■öØÂ█à\╦G\­æN\Ë\ßóEF[D \0o¬++\ÚET\Ì\ÌTr░ƒäë▄▓┤{Q\¦GLBHc?\ð!mf9\¾\nè\├\§\├■.\nî«¹JøÞêÿ(u=eHI+\╩baITP\Ý>ù╠Ø~S\Èa¼ûô}ûÖ_┘ªZDT_Z\╚toÓÂá[Ø.½\▀#tî▒\Ò/Öïañe╣øÁUV]2ê©░╗\Ï<_\█\ð\╬8óát\ı┌çq	C\┬B\├9aËù3╣Àn\ÏLaöÜ\┬\"\"\'ýêëÇÇ\0\0\0\0\Ù\╦\┼%k½│>\Û\¶\█G]\ÈN-]\Õ¦í¼_▓\┼?.eù¼<òú-óC;Di2úQp_N·\"³\n\▄\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀiÙ¬à\┼&¨çVÄ▒~\╬\¯N\┌\╦a╣Z&+4èUO\µìE\┼U\÷\┼T┴@▓\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\¶5uQ;¿\Ù\µÏ┤Uï\ıyQ╗mªYnVè\Ýw.2Z\ã7pD]\ýSôQ\├86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└H\╦+║ì.\ıH²-\r`¡ÀOIWH\‗Wö\╩$L\¤ù²7)Q0U_\Õ\\Q7▒\n\ý\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ#3«\Û5╗T\█\§┤5é6\┼=;e+\╔^SH▒2\╠ZO\¶\▄0LöE\┼QÖ0E\▀└+│üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄}s]TO*:Â\Ýb\Ú]\Èoeûøò¬╝]╦ÅLûpì\├W┴ò\▀\ÃP░\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üR\Þj┤\Úe[pÀb║¿_\ÊnX{h\Ù\¯õæ¼Â█ò¬bôHUD■X\ı\\UQ?lU1TLT,\ÓT\┌\Z¡:YV\▄86å½NûUÀ\0\╬Mí¬ËÑòm└3üShj┤\Úe[p\ÓT\┌\Z¡:YV\▄86å½NûUÀ%w+║ë²½¼\\╝┤5ïå[º\µ,┤\§ÀÆÁeäXgê¡.Lj«	\Ú\ÌE_éüef©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\ãM*jrG,ûNº\‗\Þ\╔\ÈB\┬KaÔóÿt\‗5·0¡½À,┤¿»Fi¼ûq\\WðÇd└\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\ror.\ým+SJ\Ý\ıF┐½½9╝Úô®s1¼ACBA:mû\Zëèëm\Z³ºj\±ÂXeaÂ\█iW%òFZTU░╣3\┌\╩2uM\Ít}%RS\Ý8X©V▀ñdCº╠┤Ëºðæî▓\╦/\Ï\\å\┘TVXmåÖTiä┼òP¥\0dÎÄ*cV\þ¢\µTÿ\0\0\0\0*┐\ß=╗\Íg×1\È\0\0\0\0x©\þ\þ\Ê/ä\Ó\0\0\0\0yxñ¡uvg\¦^Ç│\\RQZ╗,¯«Ç▒\0\0\0\0\0C\┘\Ì>¨\¶\¸┼ó└©\ÒÐ¥~#g\§-┼║U\­uÂ«¬X╗o-åù\Ês)║&\nºG«føx\█\Ã\¯\┌eQ@8ie»\õx²?|É:\Ì\┘WÆïío®\Ùà#UH*é\\\µ=\█²ºJ\█(¡;k\È\Ë\re0®¹+*8\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿`\0\Ðw\È·\┌▀©+\┌\═#=¿Ú╣Ñ+³/5IqÐ▓Ã«b┌ëç·Wx¢|\Õ┤z¨å┐)ûÜeª]«Jó¬áW\█\n·Â©s®\ý\Ì6àÄª\Þ\þ,\├8æ59â{5ÿ>■uê~\‗\Ô\Õ8q¥\Úùl╝eùè¼╝iQ$\rè\0╔»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢f©ñóÁvY\¦]b\0\0\0\0\0ä┤oØC\ÐQYv\Ú\▄\¯~\█m┤©#,ñ\┌-UUdD}Å\┼MÄy,ÿ\═]\Í(Ë®k\µÂ\Ã\Ú^ú\Î\╩┌¬2«ÿVqx\╩\Ó╗Ú╝ÿb©o▒ijóEZH \Ûzj=ÿ\┘l{┐\╠p¨ûU£ñ\┼QQQQ1E@5e?w┐V·6ªÑñs\ÏYs\╚(\Ï\Ú\╠\ıUç\ÝñDc\Î\╩\▄Kl╝o)_<Wìeò_\┘L┴×\ÃGZ¿\┌)Àûyå\\\╚┌ÿ\ãD<åg\¾Y³êÀ\´Z~²2¬½╝ªÌ┤\ÌJ`\╩e \0*\"\÷Ø▀ïWNVƒ└3¬¡\─$┘û2Ì▓\±\█h\Õ\Ê\õeú\r¢\├!ûòØ³}_║á\█sw\Þ¼\¯9║&v▒½.mûbm\├\Ã-▓ìcÆ\┌2\┌\"½-`©*z┐`>┤	\¯&│;\­Öpá\0\0\0\0\╔_U>¡╚╗\╠\È\µ²┌Ü~│yA\╬*\Ã│W.ıÀ\╚\±\█h\ÕÊúy\r=\├!\╔\▀\╔\ÃBzU}╣╗┤\Íq\Z■ë£¼f\þ╝eêåp█û\Ï\╩\Ã%¼û\ÐYk┴~¹ü\þ¡o]ÁÀ\ı║ù¬\Û7psûKNØ■Sm▓ÝûÜ\╔eÀì2èÄ\┘V▒DVò=\n¥äUE\Ì\╦kpj)ì+JTlF\╠e¿\ËOG-░\╦\ãiiÀm┤ê\╦\ãQÑD┼ò²\Ð}\Ó]\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@Y«)(¡]ûwW@XÇ\0\0\0\0 -l4╬ÇÅûã╗³\╚x®\┼@\ß\Ù\ßö\├SX\ãZOó¿oX■+¨%╠àí®\╚Gôl\ÐZy4màG.£2®ù¨\Ý&\¾-0èÿº°▒g%7\­@\Ý¹kAKmØ+óÑOÌ┐s.v¿\Ë\þ┐\┌z\§ªòÀì¬~Ï┤Ê«▓`Çq\¤\Ô\­\ÃUHkÂ&,«&o+®\Ò\█³çNÿ┼¿8ùì+J\Ý\Ô·\Ì¹J\╦ké\"\"ó·1P\Ûkh£Y\╩\¦>\▄ZE\╠c¼d\┼\¾8\õ+\÷ÖFrXO■î▓\╩2ƒ║Ó¬¥£4\Ô\█\­\¸9y<ë║öt,D┴\▄\┼\ÒÂfp.]½Ã«×\Ó\╦¢a}ªZ┴öi=(╗■à\\j■l\\UÑºbg_\Ù\§çkÕû▒b\Ë8½qM\µÜ┼ÑVùÐÄ\‗o&*ö	\¯&│;\­Öpá\0\0\0\0\╔_U>¡╚╗\╠\Èh³Y~gi?ê║dL\╔\╠\ı\¾┤Ö@©t»╣~¿\╦\§äM\÷ÿiQöT\¶▓\Ê\Ò\Þ]\Ó\█▀åh®¿Öì@\Ì5\§7\Úû\‗ÿàv\ã*├öT\ÌiñVÜVÜ\¶b©&\‗bí!°║░s\Z\╔\Ísi6DLÕ░ê\Ô:årÜêçaZTm\┌&·Â\╬R\Ô\¤°Ö\¶oª\n\┬5éÖQÄ\Í\Õ\ı\¯×CM&¬\µ¡\µí\ß\█\╔Um\Ô~═ÁÆ\╬ \0à=;\ÙéMÇ2k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@î┤ùjÆäÁtt3\╚zò[u ù0Ê▒J\═[gåvïâL\├**ozQU\n\╠\±Q▄û¿\ýö\█f×*;Æ\ıÆøl└3\┼GrZú▓Smÿx¿\¯KTvJm│\0\¤\╔jÄ\╔MÂ`Ôú╣-Q\┘)Â\╠uº╗4ö%&¨\╦\Ã#K╗sÂ\±wKM[L\ZÜE4ø\Ú\r\Ú┴S\¶óÔïéóá9ßú╣-Q\┘)Â\╠<Tw%¬;%6┘ÇgåÄ\õÁGdª\█0\±Q▄û¿\ýö\█f×*;Æ\ıÆøl└3\┼GrZú▓Smÿ	\n\Z\ý\ÊN*:¨\Òp\§\"ú·ì\█\ãrii½Jë╣pÓ¿ÉÏ▓©▓╗╦é\ßé·B┐<Tw%¬;%6┘ÇgèÄ\õÁGdª\█0\±Q▄û¿\ýö\█f×*;Æ\ıÆøl└3\┼GrZú▓Smÿx¿\¯KTvJm│#,╗Tô7jñèXjù!\Õ=$vêö┤\ıZ┼ÿÖÜ«,■øO\µLS\▀\├\0«\¤\╔jÄ\╔MÂ`Ôú╣-Q\┘)Â\╠<Tw%¬;%6┘ÇgèÄ\õÁGdª\█0\±Q▄û¿\ýö\█f×*;Æ\ıÆøl└H\╠\¯\ı$\ı┌ª\ÔÆ\ZÑ\╚wON¦¬--5F▒j&X®â?ª\┼S¨WD┴7▒\├\─+│\┼GrZú▓Smÿx¿\¯KTvJm│\0\¤\╔jÄ\╔MÂ`Ôú╣-Q\┘)Â\╠<Tw%¬;%6┘ÇgèÄ\õÁGdª\█0\═┘ñƒ\Èt\ã!\ÛDGÃìeR\ËVUSr\Ò\┘┴a▒iqi7ô\├\¶\"¬~x¿\¯KTvJm│\0\¤\╔jÄ\╔MÂ`Ôú╣-Q\┘)Â\╠<Tw%¬;%6┘ÇgèÄ\õÁGdª\█0\±Q▄û¿\ýö\█f:\ý]ÜJ.ôr\Õ█èæò¦╣#x╝Ñª¼&\═!Z]\§å\¶\ÓïézUpD\┼U\±Q▄û¿\ýö\█f×*;Æ\ıÆøl└3\┼GrZú▓Smÿx¿\¯KTvJm│\0\¤\╔jÄ\╔MÂ`Ôú╣-Q\┘)Â\╠Ø█╗Tö]½¼aØ\├È¿\█\┘┼åU║Vj\├8¼3\─LZjÊ¬êÇY┘ÁU┤öV>\¯\╦;½░,@\0\0\0\0yUh\þÏ»■²>\±h░-└\0\0\0\0T¬\È\¸ \0ô;\­Öpá\0\0\0\0\╔Z«x¬}[æw®¿`\0\0\0\0╔¬«x®ì[×\¸®P`\0\0\0\0¬²U*{w┐ \0╔×xL\─P\0\0\0\0E\ßUJ9\╬ \0┐H|Zp\0\0\0\0u\õUKIZ\ß\¯\ý¤║╝f©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶ÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\0\0\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@Y«)(¡]ûwW@XÇ\0\0\0\0!\ý\´ƒ|·{\Ô\Ð`\\\0\0\0\0\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿`\0\0\0\0╔»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢f©ñóÁvY\¦]b\0\0\0\0\0ç│╝}\¾\Ú\´ïEüp\0\0\0\0\0èá8Oq5Ö▀ä╦ÇÁ\0\0\0\0\0FJ©\Ô®\§nE\ÌfáYÇ\0\0\0\0#&╝qS\ZÀ=\´2á,└\0\0\0\0U \0	\ÝÌ│<\­Öêá\0\0\0\0{\┼└\þ?>æx┤ \0\0\0\0\0\Ù\╦\┼%k½│>\Û\¶	;Gv\Ýd%½úaónU(\Ú\¾¬~\\\├\Ãm╬íQªZHWh¿¿¡\´**.\­╣Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└#-5┘Á░tø\þ1W&öt\Ì\¯O\Z\╔jw\nïÆ\È\Í)ªW¹\¤Bóóó■Þ¿áY\þÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0Äí«═¼qRW\¤\\¬Qå_\Èn\Ì:i®\▄*#l\¯T¢£S·ø\Úï-\'¨óº\ýÄx\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0		e█Áî¦║Æ)½òJúùö\ýæ\█uÆ\ËL\─\╠ıñE\╦\▀TFÖ\┼?\µOX¨Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└$&wn\Í5vÚ©ªnU*«]Ë│Àm╝I\È.K-5,VQW/yUk \0ò}@W\þÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0Ä«n═¼RP/▄¬QÂ\\To=iÖ\▄*ú\¯T┴£W·ø╔ïLº¨¬\'\¯Äx\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0x\Ý.ö).╝à\¾\0gÄ\Ê\ÚBÆ\Ù\╚_0╦│vmle&\Õ\╠-╔ÑÀ╗ÆF▓YØ┬¬\õ│5àiÑ■\¾ðêè¬┐▓\"¿yÒ┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└Ò┤║Pñ║\‗\╠×;KÑ\nK»!|└$«\Õ█Áæv«▓åå╣Tú\Î\¤i¨ï¦▒:àVÜia^\"\"\"7¥¬¬ø└RY\╔|Vûèi¿uUºeÿ»\Õ3\╔]³\0░\▄\Ú ç\ÞÖ¹\0\▄\Ú ç\ÞÖ¹\0\▄\Ú ç\ÞÖ¹\0\▄\Ú ç\ÞÖ¹\0\▄\Ú ç\ÞÖ¹\0\▄\Ú ç\ÞÖ¹g\Ó \Zú\▀+P0Û╗╗= \0ä¤Áó■\0[nt┐ÉC\¶L²Çnt┐ÉC\¶L²Çnt┐ÉC\¶L²Çnt┐ÉC\¶L²Çnt┐ÉC\¶L²Çnt┐ÉC\¶L²Çïá` \Z®«\Z,:óT\╬\Ð?ñ\╬\¸■ô.°i╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷6W×\ZÖƒ\ð\├\Óö▄ëQ?)×S4°e╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷6i×\Zeƒ\ð\├\Ó┤\▄\§U?)×S+°e╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷╣\Ê■A\Ð3\÷.¥ÇÇfªÀêÉ0\ÞïS<E■ô; \0·L\Ã\Óª\þK¨?D\¤\Ï\þK¨?D\¤\Ï\þK¨?D\¤\Ï\þK¨?D\¤\Ï\þK¨?D\¤\Ï\þK¨?D\¤\Ïø┴\0\═\ÕYüçE\¦\┘³&}¡	\­\█sÑ³éóg\ýsÑ³éóg\ýsÑ³éóg\ýsÑ³éóg\ýsÑ³éóg\ýsÑ³éóg\ý}\Òù└3ikVÖüçEJvgé■S<ò\þ└EÜÔÆè\ı\┘gutê\0\0\0\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð \0\Êq_îƒ\├\▄$|lÀ°▓q\÷]■!¿:Nq\Úù\¯^4\Ý\Ù\§\È+L4¼Â\╦L«KJÿóü╣\ßb\\\ã\├:ïçiÑt²å^0¡2¼¬▓\Êbÿóó*o/íS?P\0C\┘\Ì>¨\¶\¸┼ó└©\0\0\0\0\0EP\'©Ü\╠\´\┬e└ZÇ\0\0\0\0#%\\qT·À\"\´3P,└\0\0\0\0ô^8®ì[×\¸ÖP`\0\0\0\0¬ \0ä\÷\´Y×xL\─P\0\0\0\0=\Ô\ÓsƒƒH╝ZÇ\0\0\0\0u\ÕÔÆÁ\ı┘ƒuz\═qIEjý│║║\─à\¶\▄@³<\╩\´îU4êñ\´ÁO\╔î\ÛÅ╔ûLh\µ\Òó]\"Nó\‗æº\Ú1tìb\ÍRº\¶\ËiT:\§\ËYN\Ïk-û\±e)ƒB³P©\0!\ý\´ƒ|·{\Ô\Ð`\\\0\0\0\0\0\"¿\▄Mfw\ß2\Ó-@\0\0\0\0Æ«8¬}[æwÖ¿`\0\0\0\0╔»Tã¡\¤{╠¿0\0\0\0\0U\┬{w¼\¤<&b¿\0\0\0\0\±p9\¤¤ñ^-└\0\0\0\0:\‗\±IZ\Û\ý¤║¢f©ñóÁvY\¦]b\0\rO4³2~\þ\±\¾\┘═àÂq▒æ\ÃE\─\Ëp/6¨Á\╩mÀ¡┤\´iUUUZ\\UWhKáá%Æ°it¬\─,+ûC8p\┬0\ÚËªYFXaåSyFQ7æ\0\¶\0\╬\­9\¸¤º¥-└\0\0\0\0*Ç\ß=\─\Íg~.\È\0\0\0\0*Òèºı╣yÜüf\0\0\0\0\0îÜ\±\┼Lj\▄\¸╝╩Ç│\0\0\0\0\0EW³\'Àz\╠\¾\┬f ZÇ\0\0\0\0!\´£³·E\ÔðÇ\\\0\0\0\0\0#»/ò««\╠¹½\ðkèJ+WeØ\ı\ð yfp\¤\Ò%\ÐPÉ▒mB¥~\ÕÀn▀▓ø\¯ÜiòDm>(½Å²Ç■aÈ┤¢┼ó*	ì▓ø;ÿñl\┬%\╩?âr\█OÖ<\╩_\╔x\╩\'\¸╣J¬¿¥£Uq\▀E└?í&Å¿h;U!ÑÛêñ}1ärÊ¢e\╦G(\█m6╦ök\¸\╚Fæ£}\█\█\Ï|\0{;└\þ\▀>×°┤X\0\0\0\0\0¬ä\¸YØ°L©P\0\0\0\0d½Ä*ƒV\õ]\µjÿ\0\0\0\02k\Ã1½s\Ì\¾*\╠\0\0\0\0_\­×\¦\Ù3\¤	ÿüj\0\0\0\0\0ç╝\\s\¾\ÚïBp\0\0\0\0\0Ä╝╝RV║╗3\¯»@°│ìd\┌J)ûæçeƒ\ß^J\ý┤\§5\═Pi\ÛkÜáb\Ò)¬ra=ü®úd░»ª▓\ÃoA\ã6\Ò«Yy²┤e¼7▒\├ \0\Û·\Èª[>ª╣¬-=MsTZzÜ\µ¿6â)\ýÂ\├l¬Oº╚¿¼»Áó└Â\╦OS\\\ıû×ª╣¬-=MsTZzÜ\µ¿┤\§5\═Pi\ÛkÜáE\ð*¼\È\Î)ûô\ZÖ┌º\‗»▓e└Zeº®«jÇ\╦OS\\\ıû×ª╣¬-=MsTZzÜ\µ¿┤\§5\═P#emÂ\ZØrZ\├°nE■\Õ3@,▓\Ë\È\Î5@eº®«jÇ\╦OS\\\ıû×ª╣¬-=MsTZzÜ\µ¿│F┐\█\r0╣-a³7= \0\n\‗ÖXYi\ÛkÜá2\Ë\È\Î5@eº®«jÇ\╦OS\\\ıû×ª╣¬-=MsT║¨U¬Ü\Ìd▓\Ê\ßS<U■U\÷L\─L┤\§5\═Pi\ÛkÜá2\Ë\È\Î5@eº®«jÇ\╦OS\\\ıû×ª╣¬M\▀┼║A\├,0\█J│\Ú\n\"#+\Ýh@-▓\Ë\È\Î5@eº®«jÇ\╦OS\\\ıû×ª╣¬-=MsTZzÜ\µ¿\¸ì¼½IZó2Ê»\­\ý\¤³+\╔^³áñ©-\' \0áç \0\─\╚`\0\0\0\0\Zô \0tÀ \0[Ì×üÖ\0\0\0\0\0b$\´\¸\╠Y\¯«\0╦Ç\0\0\0\01\▄,ÿ³¥ \0$Hp\0\0\0\0\"\'àÆ\´ù\ã \0\õå.\0\0\0\0\0\─O?\Ì2ÿÁ\¦_üù\0\0\0\0\0a¬\¤\¸K\§░]\Ú\ðÉ\0\0\0\0&¡\Ó┤\Ò■é# \0@ \┘','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',NULL),(13,'B6684DD18AF36A16135364DED8B365B2',100,_binary ' \Ï \Ó\0JFIF\0\08\08\0\0 \█\0C\0	\n\n			\n\n		\r\r\n \█\0C	 └\0HÉ\0 \─\0\0\0\0\0\0\0\0\0\0\0\0\0	 \─\0P\0\r\n\0\0\0Uö!4VæÆò\Ð\Ê\Ë1STWs▒\Ô\"Ar▓2DQa#$Bq5ERü36cäívé│┤┴ \─\0\Z\0\0\0\0\0\0\0\0\0\0\0\0\0 \─\0?\0		\0\0\0\0QRSæí\Ð!1A2TqüÆ▓┴\Ê\"4Ca\ßB\­#3b▒\┬\±$ré \┌\0\0\0?\0²¿│,\ÛUkk\´MtR91Mû\ðdY\▀\¦ép▒~nò}®|EgÏ│\╩\Ï`¢ur\╚ \0\ßc-F9\╦■╚£*3tØ®rvÀ8zf\Î\Ã·y\þ\┌3tØ®6ÁùÂ5¤┤f\Ú;Rmf\r/lkƒh\═\ÊvÑì¼┴ª-ìs\Ý║NÈøWº\Ê\÷ã╣\÷î\¦\'jX\┌\┼>ÿÂ5¤┤f\Ú;R\╬\ı\Ú\¶┼▒«}ú7I┌û6¡MÑ\Ýìs\Ý║NÈ│Ázm/lkƒh\═\Êvì½\Ëi{c\\¹Fnô┤\ãı®ù■olk┐h\═\Êvÿ┌¡&ûÂ5▀┤f\Ú;RmRôK[\Z\´\┌3tØ®6®IÑ¡ìw\Ý║NÈøTñ\Ê\Íã╗\÷î\¦\'jM¬Rikc]¹FnôÁ&\ı)4Á│«²ú7I┌û6ºIÑ¡Øw\Ý║NÈøSñ\Ê\Í╬╗\÷î\¦\'jM®\Êikg]¹FnôÁ&\È\Ú4Á│«²ú7I┌ôjtÜZ\┘\Î~Ðøñ\ÝIÁ:M-l\Ù┐h\═\Êvñ┌Ø&ûÂu▀┤f\Ú;RmNôK[:\´\┌3tØ®6ºIÑ¡Øw\Ý║NÈøSñ\Ê\Í╬╗\÷î\¦\'jM®\Êikg]¹Fnö\÷ñ┌Ø&ûÂu▀┤Ä\┼&Ygjöë \06Â5▀┤v!9e\Ð\Ì\\É];▄┤¯À¬m¬àÑG$XZ\n\▄\▄\ý1\§7¹!}àÁwl╣╣╔ò\Ô¬b┐Ø\Ì├Æ»cnuú╗ª\¦,º┐#@░┴\ÏX¥O¨>╗\§.½▓Øm\┼+█ÿ¬ÂôØ┴ \0vûY\ÏQeù▓║\╩\´gaù▒/D╠×Ï▒\­2ÐÂS \0|¢\Ê\Õ\┘%ã▓Ëü-+c]^\Þ2>Æ\õ┘® \03Â5▀┤\┌UØÑ-ìw\ÝCiVvöÂu▀┤#!┤╗;I\█\Z\´\┌ND3┤\╦?J[\Z\´\┌N@\┌eƒ\ÛKN\Ï\Î~ÐÉen]}vØ▓ƒ¹¤┤îâMáÊûã╗\÷îâ;NáÊûã╣\÷ôÉ6ØCÑmìw\Ýi\¶:R\Ï\Î>ÐÉgjZR\Ï\Î~\Ê;&W\nı│&╗\ð%ºMh\ıTSF\÷Âó\Zù#\ı\þ#s\┌\ýQQUQqEL}Bc ¼|úm[\¾eU\ð\╔q|\§*%|l¿u-\"N╗\Z2Uj;;\Ã\ı\Û\ß,ª\Í\╩╩Ü¬┤º.\r?ªoï¢ò3v╦ûg\╩2¨=Ñ=\ruEÅq¡iúè*åKB¹IVÖ▒\╔+\õdhÿ■\\RLW\├Í┐\ýcQ^Y£æ\Ò╔▒│\╬WEØSç\Û\ß\ı\Ó\´ì\±\╩-▀¥\Î\‗\§\Ï\±\█àôr\'º®¿ú[AëG5\Z\Ï\ýæÈ¼º\ßv╩Á▓Uø53ZÎ«zßÿ¥╝\‗▓<ôKù\Ù\´O│\╦vlR┘┤ÑUKk\Ú[M|4\╚\ÞedJ\µ\╩\¸~9¡tX*+_ïff`\╚·│2\§~¡\ÙV;2╦║6M/ñ-\Þ\ýZy+\Ûq}\Íz©£╩ÿcòeY3iD³17\±╣ƒ\ÚG╣5L\¦/([f\ðu▄û\¸\Ï\ÍòKo\ÃdV\╦Slë\r=}%í#[$ÆúQ\Ê$\Í{XÄ³-rNëé99OÖæ&N2²x»²\Ò░l\ã\¦\Z:jKJ╬á¬¬_;cebT┘®Y\þ0ú\õG\╔^®	òU\ý\¶\╠Vô3&GØ®\╦Ú╗À\‗²:\‗Zu\r│\ý+Z║;▒H¨Â:[M\╦2t¿ò¡┼îúl╗6└ê«X\Íi▒~fWµ£Ä\┬\▄\‗å¥vc-$▓¼;▒k2├ª«¬Ü║*¬å\Ê\┌q\Ë\ı\Ð@ïJ\õ\╬\╠E\¾\Ã#òVDlö\¯jg\"\Ô\▀]®2%Á▓¨~\ý¨¬h\Ú«\¦¦¿×Ãûó\n\¸I5Ll×H\Ý\ãYìXQ3û6¬HÆ«vzóÁXÿú│\┌\ÝIæ╔ï/À▒o-ƒuÂúA5Tuuò\‗6Ñ░\ÃV░\┌\Ë\┘\‗-&\═+Uìâgs%v2>r=f*ödu\¶╣l\╩-░ù2I\÷Áb6Ï®╗VÑlîIÑà,\█N*\õu3Ø\"Á[\"MJ\─IS9dcQ¿┐\Ãd\¯r.Gö=\ß┐W¬å\Ù\┘6=å¼«┤áFÍ║g¬6éZ\n¬Â¬\─\╔¡ØæX¼{ÿ®▓\"╣îV½{Rdvù\█)v²òòÜ9,\÷┌ït«\┼Môxdàæy×\¤i.\¾¢\╬Gú®æ\¶/LÍ¬\"TIÅ\÷L\¸É\Þc\‗Å¢p¦èkFÏ▒.\§ºlX\Ím│fFôI\µ\═\¾ò½GS\¤$▓DÍ╣®F\þ$Ö\ÝEW\µú\\\õj>;RdEs\‗\Ýy\'×{MÂ7ØC}¡jH,(j*dzRZMæQ\Z\Óÿ$;EdÛ®é \0ùòp\┼\─\÷ñ╚Ü│\╩R\¾EN\┌z+ÁcUZkW\\\ÃF\╩\ã%6bû*f\Í:téYfd\═V\╩\Ã`\─k\¾í\╬jÁ®26	ÄW1«s\┼r\"½\\¿¬\▀\ý©pb×ó\─>Ç\0\0\0\0\0\0\0\0<\╩`! \0\0\0\0\0\0û▄»¢öò▓\´\¦z;R3$×\ÏJ5c\±■j\┬³\õ\├\Ã\§\ßç\µD\Õ\‗╗ª┌¬úYm\Ù6ºdTHa«JÂ½80vz1£+\├┴ç¥`s└\ÓÅuùj┤ª╣ûl\█Ve%ízl>║óÜ¥J)<\ÊkB&L═×75±úÜ¬\ıV╣½é\ßÅ	J!\µ\ý¹>\Ù]l«▄¬æ|mZ\Ï\Ýz[iÂì,ÎÂ«ıÄFEO\Òr\Ã<\‗Á¬Î»æS;p\\3=²\¤Y;ùIk\╚\0á\‗ç}oà█¢\Í\Õòr/íUv\õtr\Ì\ÙU Z\Ã▄ç=ƒ%.8\ý«|ì\È°?\═S³┬ó▒v\'U?\Ë\Ê\­▒bíåãáå╦«}m)bm5KÛû®\Ë┼ÿÖÆ,╩¬▓½øéþ¬«v8\Ò\┬Y.º(¥w┤ø[\╠dFO▒Ãÿ\Õfz\'°\Ð\Ò┴¨\­c\§\'╗/é\'├╣\┌\ÎXû=Ñ+g┤,\Ï*$ks\´on8\ßÅ\¶\ß^r2%=┘╗┤Á\ıSXöæ\═│\┘\"G\┬\ÎT■\Ò \ý░D\ÃD\Ã\Î┴\Ù$|GOQñP\Ë\┼mLîì\Z\ÈLq\├L=dÂZ\¸\╩\ÏcG╚¿\þ╣ê\þ*p\"¬·\ıp³\È\ßåF,rCÏ©\"Á\╠EE┴qN\Ó\Ó^;#f\Ïc\┘\Z\ıc_ÿÖ\╚\▀\═}hƒ\█\ÈUî_[\Z╝9\▄-O^c■°*º²\Èdd0\ÃbÄ\┘S5¼k\Z\È■êë└ë²åA£\ã/¡ì\ß\§\­\'\þ\§$a\ð\┬\¸1\´é7:7+ÿ«b*▒WÍ¿┐Æ»\§B\ÐD\µ\µ:(\▄\Ì┴Zè£èpe\0\╚bÅ \0(┘è½┐8WÍ╝ÜÔ©»\¸$g1èèè\ã\Ó\´Z`£?\´²@├íà\Ý\╠|1╣╝à\╠EN\┼8·/\nr_[w,øBÏ│m\┌\╚$û¬\╚Y_Dï+Æ(ñæïÑ\Ï\ÐsVL\┼sÛè¡kÌëårîØ¨G7\═)4ç\═ \Ï\Ð¹\"3bnn~8\þaå\Ï■~▒ÉLH\0\0\0\0\0\0\0\0#¢=┴ÿ\0\0\0\0\0\0\0\0\0\0┼┤¼╗2┌íÜ╦Â,\┌J·:äF\╦MU&èDE\Ã1\Þ¡w\n\"\­º\õ<|G]b▄ïòv\ÛØ]w.uâdÈ¢ïªá│ ºæ\╠UEV½úb*ª(ïå8p!1▄ëwgñ\0\0Ä\ZjzdzS┴I,ÄÖ¹╣\Ê9qs\Î[ù\¾U\ß_╠îÉ\¶\¶\¶ö\±\ÊRS\┼▒#Ä(ÿîc\Zëé5¡NDOR\'\0E\÷ \0╩ûƒ\╩o \0æó|¿¼\Ê\"b│9·½ö\Ý▓5\µ\╦*\­ñ»\Ú(\╚,¥\ı²% iàfz\'\¸rîÇôH¥®£┐\ý\Õeù┌┐ñú$\╦/ÁIFH\rÜOl■Æîü▓\╦\Ý_\ÊQÆeù┌┐ñú$\╦/ÁIFH\rû_j■Æîü▓\╦\Ý_\ÊQÆeù┌┐ñú l▓¹W\¶ödÇ\┘e\÷»\Ú(\╔▓\╦\Ý_\ÊQÆeù┌┐ñú$\╦/ÁIFH\rû_j■ÆîÉ,¥\ı²% 6Y}½·J2@l▓¹W\¶ödÇ\┘e\÷»\Ú(\╔▓\╦\Ý_\ÊQÆeù┌┐ñú$\╦/ÁIFH\rû_j■ÆîÉ,¥\ı²% 6Y}½·J2@l▓¹W\¶ödÇ\┘e\÷»\Ú(\╔▓\╦\Ý_\ÊQÆeù┌┐ñú$\╦/ÁIFH\rû_j■ÆîÉ,¥\ı²% 6Y}½·J2@l▓¹W\¶ödÇ\┘e\÷»\Ú(\╔▓\╦\Ý_\ÊQÆeù┌┐ñú$\╦/ÁIFH\rû_j■ÆîÉ,¥\ı²% 6Y}½·J2@l▓¹W\¶ödÇ\┘e\÷»\Ú(\╔▓\╦\Ý_\ÊQÆ.\╔|ÄÁ(┌▓┐Øƒ\Û_·É─┐\õÐ½\¸=\┘·\­\µ\¦┘®ßå¢ó(+\\╚╝\┌I\Í*5■+X\¸■=[å\n┐Æ*c\├\├m┤L\═8e\´\ß\¦´î»4¨¹┐\▀\'úçj4▓¥jz\Þ*\ÛdÄ¡Ä¿×f\─\Î\╚\µL\ÈkÜÄL\Z\õX\­VÁ_\┼├ébU×¬2d\╔\¦\▀\þ\Ò\þ\ß\õÂ;9fP▓Æ\ÓÁcW¼2c*\ý\ÏT¬1ÄFÁsZ╣¨╬ëW9Qq\├\±&>¬¬\¾▀ô\¯\´ÅwÅå\õSwv┐¼╝┐╦®ª×éÆ\┌t\ÍtTï┤\\Ræ$2¥&+\÷7╔è#\┌\§r&v?Üc¨ù\ıUD\┼Y|p\±ê×\ý▒S92\ãXÿ├×GqZ╦Ø4r¼\÷ÜZ3Eú_×Ïò°\╔:╣\╔\┬\Í\þó\ý_\§bïè5qÓóÖÀÅV2\Û2c\¦\Ò\¯\┼g\Þ\╦\▀9\\;¡Á╚¿ÊòÐÑT2\├X\¸J®3eï5▒│R▒¬©*\­/\n`êÂ^3│W\ÞÅôy°\‗WFH\´ƒ\´■\ÌEí\r\¤m%C)\ÒºeB\Ã;æ═®Wf\╚\Ï\ÒXÐÖ»ss\\\Õ½ùÍÿªD\█Õî¥\Ì_\▀~^\Þ\­ç║óÅ/\¸\¦?\ß=ƒ5Ï¿▒,\╩*è¬x\'j/ƒ½\ıeçgæ[\\ëïcs┐71SL\▄═ñZ┼ÑUSc\╦▀Æ;²\Ì1²O┐║)\ýõêƒ\¸{■(deÐºúW-=ÁïO+\õcjetlØ░ã¡c°Ü▓+\ËÍ©\­ó/)\Û&\┌k╔ûreÅ(\­\╦▀ù╗ªJb;³\─ \0ç!\ð\\H¬|¦îñû-Ø®│:ªLsV\µ/®╚£`\õ\Ó■è©·ù\─M\Ôc,\Õ╦ô\┘\╦\§w\'\¶G¹■\Ò³ód7\"\nHñFCQ\"S▒\Ï\╦P®ƒ\"║4z9¡~v)îè£\rL²^│\Í[z¬\╔\ßp\‗\ß\Ã\Ã\ÓÅ\Ð \0¹ \0S\Ó╣\ıNc%¿│Æ××æ\È\±╣│+%sæ\¾\ß\"¬╗àS┐Ê¬\ý\¶³æp\±où$ÕÖë■╝#╗Ä_s\Î\Þ╦ô\╦³\¶t\±\ÃvÑ¢5╣ÉGf\─\╔Rø	¼æ\Þ\▀\­\¾£\þº»»\±5=\\(_û\Í,r \0½³\¸¨a\´W=×\ÈGù°\Û\ýjÆ\µK#kí_\­\Í&+\Û_QòN«kù=!LW\ÔO\ÛS°\╦?oW³\Ó\¸·2 \0┐\´³<òb5│\"1É168\Î$W│\─ÃàUW}i¨.)¨┤\§U\Õ@\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0.\╚\\-Z5_o\¯C \0¹j²\¤v~╝;¹ìp*\´ÈòR[6}á|Q5*▄®│>DzúYç¡\ÏF\§\├Îé*■Jy¥_¿╣v{q3ù\Ú6vSiù#\┌\´i¢£í▓yÑ\ý0Á\ÕÄ\─\‗[úUë¢ª\÷ré\╔\Þ\╦\Ï5\ÕÄ\─\‗4j▒7┤\Ì\╬PY={╝▒Ï×FìV&\÷ø\┘\╩\'ú/`Îû;\╚Ð¬\─\Ì\Ë{9Ad\¶e\ý\Z\‗\Ãby\Z5Xø\┌og(l×i{╝▒Ï×FìV&\÷ø\┘\╩\'ú/`Îû;\╚Ð¬\─\Ì\Ë{9Ad\¶e\ý\Z\‗\Ãby\Z5Xø\┌og(,×î¢â^X\ýO#F½{M\ý\ÕôÐù░k\╦ë\õh\ıboi¢£í▓yÑ\ý\Z\‗\Ãby\Z5Xø\┌og(,×î¢â^X\ýO#F½{M\ý\ÕôÐù░k\╦ë\õh\ıboi¢|á▓z2\÷\ryc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\─\Ì\Ëz¨Ad\¶e\¯ìyc▒<ì\Z¼M\Ý7»öOF^\ÞÎû;\╚Ð¬\┼\¾/ôm\Ûè7\╩\Ù┴d\Óã½ù\╦\ÛD \0a£▒ƒ\¶O$h\ıb¿]A\ÚEeØ\´\ã\╠sıèƒì½è9Uâ\├\¾6\┘\╔w«»║Ü;\ÛêXÖ  \0ïY\▀²Ig \0·vë®\¶\´\±³~╠ø»øi\Ë\Ès\ýª@\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0Wq9■S■èLxó|\ZKd«Ñ\" \0\Ù│\¸!\┌_ \0m_╣«│\§\ß5æz\Ý\Ù¿\¸UÏû\þús£\Î9╬å×F½█£ìw°\╠r5╚Åz\"ªâ£ƒÖ}w[+\ÍJm(\Ýd\­\±¹L(«¬¿²Q\\\Ë\├\'8ùu╗ÂR>%7W│³#ã¿╗n>¥¬┤Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØë╗ÂR>%7W│³!¬.█Å»®ñ¤┤sú\±7v\╩G─ª\Û\÷ä5E\█q\§\§4Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØë╗ÂR>%7W│³!¬.█Å»®ñ¤┤sú\±7v\╩G─ª\Û\÷ä5E\█q\§\§4Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØë╗ÂR>%7W│³!¬.█Å»®ñ¤┤sú\±7v\╩G─ª\Û\÷ä5E\█q\§\§4Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØë╗ÂR>%7W│³!¬.█Å»®ñ¤┤sú\±7v\╩G─ª\Û\÷ä5E\█q\§\§4Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØë╗ÂR>%7W│³!¬.█Å»®ñ¤┤sú\±7v\╩G─ª\Û\÷ä5E\█q\§\§4Ö\÷Ät~&\¯\┘H°ö\¦^\¤\­å¿╗n>¥ªô>\Ð╬Å\─\¦\█)ø½\┘■\ım\Ã\Î\È\Êg\┌9\Ð°ø╗e#\ÔSu{?\┬\Zó\Ý©··ÜL¹G:?wlñ|Jn»g°CT]À_SIƒh\þG\ÔnÝöÅëM\ı\ý \0jïÂ\Ò\Ù\Ûi3\Ý\Þ³M¦▓æ\±)║¢ƒ\ß\rQv\▄}}M&}úØï\╦vRj\┌\ÛT\╩;ƒ▓ÁYø=rÔƒûcÅ¹\rUvú\§Mçç \0~®ïj½²1o\┬h\╦\¶║-\ÊTê╝)│F£<?\ÛB╗ \0\Ý½\¸2\ý¢xz£Ä\Ê\Ê\ı\Õ\ı\ËC;c│¬\õceììvt)£ê┐×rc²\Î·û_k¬ïÑSL\õ\´º \0&Á4\Îx│èú,d½\Ã \0\¤U\§\Þ\╦3FQj\╠\ý9³\§ª\È\±ò¨ø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßú,\═E½3░g¡6ºîÖø-ÿ\ßYvkò\Zûe*©qfv\§ª\È\±ô3e│!_P\Õ{&\ËUM\r▒f¡à\n6«JZ½Jè\├Z\╩Z┼ñÖ\ÐlN{▒l┘®Ü\µÁ\╩\Î#æ3░¬ï\ÕU\Ë┌èº\┬\'\ã|*ëÿƒ×9{¹▓w¡«\ÚE5v{>1\ßY2¨q\´\‗w,┐\┘&æÍï\"╝wrGY)ØXæÁÅX┐Y¨5s\Î=\±Àg.sÏÿb\µ\Ò\´H»$\¤jp\±£f1\ã\'ä\Ó\±{9\╔·#┐┐\┬<2D \0\┌b~.,ÖQ╚┤5~c%\´║\╔>k_ÿÖï°\\\╚Ìïè73&ë\Ù\├°[#\\\ýqx«g│N3ÄLq\¯\¸\╠c\¦\ý\Ô2\═\┬:S\┬pùcY|reA\ıû═üp#ûE\╠cò3g};┐Z¬¬ôG$x\"*\þ1\╔¨)\ZMY2\÷º\╦\╬|\‗\╠y\ß>\ÞÖNìGçb8G\§\Í8\├\µ\‗_×]K=Âò▒QeÁÆ\ð\¤iS▓\ZfJ·èxú\¤s\ÒFÁQ[éÁ╩¿\ıW51\┼Pï[\ıvQ35Ote×¨\ÛY\¦l\Ýf\"Üc¥rGt8\┘N\╔\¶4ÍäÂ\ÕïGU$▒Â▓ù`z\┼2\ã\Í`Åbg.t\Ð5Q╣╩Ä{S\¾Ll¬\┌ÊÜªë¬r\─\õ\±×»1ag4\┼]ê\╔\¯Åz*¨!À\ÞÖ]Oj\┘T¼Æ«Âìî¡ñH\þ\Ê\╩\Þ\Õvo1b\Ó\Õ┴O\Ô³)\\^½\ý\┼sT\─Le\±\‗\Ò²O	‗ç║«öST\ËÏî▒9<#Ã╗¼=█Á\¯E\­│¢-u¬,{Vï?cY\ÚícÜÄ\═k░\ßj/\­¢«O\Û\Î5STR\╠\Ý»ØS\ã}\Ï\Ô»3eù\'f8CÁ\¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│ \¶eÖú(Áfv\§ª\È\±ô3e│!µ▓ùfYø×^GÑÖFÄÄ╠×F9)Ïè\Î5©Á╚¿ÿóóó*/\÷3}ki7\╦(\ÝO}Q\þ,[\§òØ7j\µ)î▒C_®þÄûÂ:®Q\Û\╚eIîb¢\Ï\"\Ô©5¿¬½²3»\¾Æ\Ú_╣││Å°æ\´zlö\┌\÷Uç~!¡Âm*j\nw\┘\§Pñ\ı2Â8\¾\ıð¬5\\\ýQÄ\├\ß└║\¸e]ÁÍ¬,\Ò,\ÕªrGÄN■░\Î[\ÎMò¢Øu\╬H\╔Teƒ│\┘\╔ \0i][©|À╗²i|\ÊhW¡\¦_,\¶Yª]Àö\±Äª\▀\¯-\¯ \0ZA\▀\Z\ÙwW\╦=\r2\Ý╝ºîu6 \0p¨ow·\Ê°ð»[║¥Y\Þiùm\Õ<c®À¹ç\╦{┐ÍÉwãàz\¦\ı\‗\¤CL╗o)\ÒM┐\▄>[\¦■┤â¥4+\Í¯»ûz\Ze\█yO\Ûm■\ß\‗\Ì\´\§ñ\±í^Àu|│\ð\Ë.\█\╩x\ÃSo\¸û\¸¡ \´ì\n\§╗½Õ×åÖv\ÌS\ã:ø©|À╗²i|hW¡\¦_,\¶4╦Â\‗×1\È\█²\├\Õ¢\▀\ÙH;\ÒB¢n\Û¨gíª]Àö\±Äª\▀\¯-\¯ \0ZA\▀\Z\ÙwW\╦=\r2\Ý╝ºîu6 \0p¨ow·\Ê°ð»[║¥Y\Þiùm\Õ<c®À¹ç\╦{┐ÍÉwãàz\¦\ı\‗\¤CL╗o)\ÒM┐\▄>[\¦■┤â¥4+\Í¯»ûz\Ze\█yO\Ûm■\ß\‗\Ì\´\§ñ\±í^Àu|│\ð\Ë.\█\╩x\ÃSo\¸û\¸¡ \´ì\n\§╗½Õ×åÖv\ÌS\ã:ø©|À╗²i|hW¡\¦_,\¶4╦Â\‗×1\È\█²\├\Õ¢\▀\ÙH;\ÒB¢n\Û¨gíª]Àö\±Äª\▀\¯-\¯ \0ZA\▀\Z\ÙwW\╦=\r2\Ý╝ºîu6 \0p¨ow·\Ê°ð»[║¥Y\Þiùm\Õ<c½-\╩\├kæ\█w╗\▄Å³R°\ðo[║¥Y\Þiùm\Õ<aWVd\▀ \ı+¼\Þ/òâ{¡y-ƒI¡},ô║g\¤Q6\ã\¶Wó:$\¾®[▒ªjpó \0bSî╝\ËM4┼Ø]Ðôı×°\ý\═3ù╗	ƒr\┘\¶òàUMSiO²Q\¦\▀\¦\▀²G¢\rfM2+UC2ùg@\╩Iº®úlVàcºûJ\╩ZÂ½cED\═döqÁ└ÖÄrcÄjº®\¶}\Ù.X│½,cL\Ò\\\ß \0\\\Ã7ÿ┐\¦\Ô×\╬r£×°┘è\±ë\¸¥\Ë\'YeùUd┴ö\█6Ü\n©-\╩Z·(úì+(®®$╠ìó2ò«jpªs¦Ä)ü1\Þ\Ù\╠N\\\¦\\\'nÜ\­ãÿÅsÍ▒░\╔ \02×1àQÅ²Ré\Ð╔×Hm*╩è╩î¡\Ð9|\Új\Þ\"}uúóz\Í\ÈU╣0G\"╠è·╣┌¿\¸ÜëéÀ\±î╝\─╦½\╦²3\ßU9<0¬yN$·F\├&L\Õ<c\Ã\¶\╬^4\Ã?\Ù\'{y«\¯I/4zÄ\\óX\Í}-▀ñum│\Ù®)ÕÆØÐñnà%kæ\Ð\┬\µÁ\Ð79¿®°QOV¥Ä╝\┌\ı36uw\─Ãä¨\─ \0]■9{³\Ô%\µ\╬ \0w│êê┤º║b|c\╩bq\¯\­╔ù	ùƒç&Y,[\Ãf\Ì{K-T\ııÂc®\▀Ug▒╩░:ìZ\ız~$Å³äXF\ıFÁdÖ╚ÿ┐â\È\\/Y╔┤ø*▓\╠\╠°O£\Õƒ,g╗\¯EW\Ù┤Ðø\╬Sô&OZ0ÿ\Ã	\´\ã{\ËZy6\╚\Õ½iMh\ı\ÕB╦Ö&\¾\÷$S\ı\ÐJæ\├UT·Ác\╬\Ó|u\╩\÷H¿«L\õE\Ã5«Jc\Ð7èh\ýEØ^<\'/tU\Õ\ÒT\Ã\÷▓}\'a5EY\╩{º/îY|³¹1\¯{{í[ô[øíQ,èƒH\╦M4ï=®MïV\Z*zFó`\õ\ÓVS1╦Å·£\ý80C&nw®Ö£\ı]\¾T·│■®╦éØ2\Ý\¦ \0×\Þê\±Å/ï\ðm■\ß\‗\Ì\´\§ñ\‗4+\Í¯»ûz\Ze\█yO\Ûm■\ß\‗\Ì\´\§ñ\±í^Àu|│\ð\Ë.\█\╩x\ÃSo\¸û\¸¡ \´ì\n\§╗½Õ×åÖv\ÌS\ã:ø©|À╗²i|hW¡\¦_,\¶4╦Â\‗×1\È\█²\├\Õ¢\▀\ÙH;\ÒB¢n\Û¨gíª]Àö\±Äª\▀\¯-\¯ \0ZA\▀\Z\ÙwW\╦=\r2\Ý╝ºîu6 \0p¨ow·\Ê°ð»[║¥Y\Þiùm\Õ<c®À¹ç\╦{┐ÍÉwãàz\¦\ı\‗\¤CL╗o)\ÒM┐\▄>[\¦■┤â¥4+\Í¯»ûz\Ze\█yO\Ûm■\ß\‗\Ì\´\§ñ\±í^Àu|│\ð\Ë.\█\╩x\ÃSo\¸û\¸¡ \´ì\n\§╗½Õ×åÖv\ÌS\ã:ø©|À╗²i|hW¡\¦_,\¶4╦Â\‗×1\È\█²\├\Õ¢\▀\ÙH;\ÒB¢n\Û¨gíª]Àö\±Äª\▀\¯-\¯ \0ZA\▀\Z\ÙwW\╦=\r2\Ý╝ºîu6 \0p¨ow·\Ê°ð»[║¥Y\Þiùm\Õ<c®À¹ç\╦{┐ÍÉwãàz\¦\ı\‗\¤CL╗o)\ÒM┐\▄>[\¦■┤â¥4+\Í¯»ûz\Ze\█yO\Ûm■\ß\‗\Ì\´\§ñ\±í^Àu|│\ð\Ë.\█\╩x\ÃSo\¸û\¸¡ \´ì\n\§╗½Õ×åÖv\ÌS\ã:╝■P»¢╠¡©ùéåè\÷ÏÁ556t\ðC\±I$Æ9╣¡kZ\Î*¬¬¬zÉ\╦\¶}\Ê\±E\Û╬║\ý\µ\"&&fbb\"!ì|╝\Ï\┌\¦Ùóè\Ôfc$DLL\╠³Lö\┌\╩¹.YÑè:\┼X$N┴\ÝG.Á»	òÅ■-~¯ìÁ£õ┤ë■Ì«\Ó\õ\Ô\Ï\╩ò▒\┘5öt■b\Ï\ÍE¿W`\ý\§\\0\═E \0ÑH¥_¿╣\õ\Ý\─\╬\\;)┤╦æ\ý7┤\▀\r5b\¾\╦\▄0Á\ÕÄ\╠\‗Yóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV&\÷øßª¼^y{â^X\ý\¤#E½{M\­\ËV/<¢┴»,vgæóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV&\÷øßª¼^y{â^X\ý\¤#E½{M\­\ËV/<¢┴»,vgæóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV&\÷øßª¼^y{â^X\ý\¤#E½{M\­\ËV/<¢┴»,vgæóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV&\÷øßª¼^y{â^X\ý\¤#E½{M\­\ËV/<¢┴»,vgæóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV&\÷øßª¼^y{â^X\ý\¤#E½{M\­\ËV/<¢┴»,vgæóıë¢ª°i½×^\ÓÎû;3\╚\Ðj\─\Ì\Ë|4ıï\¤/pk\╦Ö\õhÁboi¥\Zj\┼þù©5\ÕÄ\╠\‗4Z▒7┤\▀\r5b\¾\╦\▄\Z\‗\Ãfy\Z-Xø\┌oåÜ▒y\Õ\¯\ryc│<ì¼M\Ý7\├MX╝\‗\¸╝▒┘×FïV/ë|ø»ä1>oL\Ï╦ÿ\ıv\ÚQW\§\0ÅM\Ï\¤·gæúUj¥\═\\\█Fò╦Å\╠\§~ñ3\´ \0Â»▄¬\╦Îà\Ð\õ\┼\ã/\ÞÑ·\╚j¢;³▓¹»Ü·9\÷X\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0å│èM\‗\¦\¶QH\ý\´°ì7\╬g\¯CÁ┐~Í┐s[e\Ù\┬\Þ\‗b\Òç\¶R²d5^Ø■?Å\┘}\Î\═}¹,\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0CY\┼&¨n·(üú2\ıTð▒ıötkW<\nÅèvj\╩\¶^\Ò²\È\Ý/ \0Á»\¦\¸k¼¢x^^L\\b\­■è_¼å»Ë┐\Ã\±¹.║¨»úƒeÇ\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0k8ñ\▀-\▀E4z\Zjj\┘\┘GY\'éwÂ9#æ©Á\ÝUDTT■çi\´║\Î\¯klº%p╗|ÿ©\┼\ß²┐Y\r_ºÅ\Ò\÷_u\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è i>}Oép\ý¼²\╚vÀ\´\┌\Î\¯kl²x]>L\\b\­■è_¼å½Ë┐\Ã\±¹/║¨»úƒeÇ\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0k8ñ\▀-\▀E4èçÅ\Ë³\µ~\õ;[\¸\Ýk\¸5Â^╝.ƒ&.1xE/\ÍCU\Ú\▀\Ò°²ù\¦|\Î\Ð¤▓└\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\05£Roû´óê\ZkU\ÈP\ð\╔WI\"\Ã4OëX\õD\\1òê╝┴\ÛU;k\õe╗\ıç¦¼ú\Í \0x/\¤&.1xE/\ÍCS\Ú\▀\Ò°²Ö_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\ÞóÅS\┼\§P\┼4mæÄÜ<Z\õ\┼óº \0(çk~£ùZ\µ0kl\Ò-p╗|ÿ©\┼\ß²┐Y\rWºÅ\Ò\÷_u\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è i\r\ZcY\"\ß■+8¹É\Ý/ \0Á»\▄\Í\┘z­║╝ÿ©\┼\ß²┐Y\r_ºÅ\Ò\÷_u\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è h²3\÷:ÿñ\╠s│^\ÎfÁ1rÓ¥ñO\═N\Ê \0\¦u»\▄\ÎYw\Î╗<ÿ©\┼\ß²┐Y\r_ºÅ\Ò\÷]u\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è hËí}C]O\"¥TX█è\Óÿ»\‗ºe\Ú(Ü«V┤ÃØ2\Î\¦\µ)Áª®\‗ÿ^>L\\b\­■è_¼åÀË┐\Ã\±¹-║¨»úƒeÇ\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0k8ñ\▀-\▀E4éÅÄA\¾Y¹É\Ýo▀Á»\▄\Í\┘z\­║³ÿ©\┼\ß²┐Y\rWºÅ\Ò\÷_u\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è i*\"\ı┬èÿª\╩\¤▄çk~²¡~\µÂ\╦Îà\Î\õ\┼\ã/\ÞÑ·\╚j¢;³▓¹»Ü·9\÷X\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0å│èM\‗\¦\¶QF$»Ñ│-\n\Úv*x\Ã\╚³\ıv	£ë\ÛDU__\õçk~îÀj²\═mù»\¤╔ïî^\ÐK\§É\ızw°■?e\¸_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\ÞóïTðÂËì(_+úIeï51T\┬F╗ \0\ß\█_\'│v«ª▓ê\╦TB\§\‗b\Òç\¶R²d5>Ø■?Å┘æu\¾_G>\╦\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\ÍqI¥[¥è h²2óUD½¨H\▀▄çk~²¡~\µÂ\¤Îà\┘\õ\┼\ã/\ÞÑ·\╚j¢;³▓¹»Ü·9\÷X\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0å│èM\‗\¦\¶QG\Úx\È?5┐╣\Í²¹Z²\═mƒ»│╔ïî^\ÐK\§É\ızw°■?e\¸_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\Þóî¢´ì«Æ5Ts\\\ıO\╔SÈºe\Ú9ÿ╣Z\╠l\╦\´m®ë\ãùô╝?óù\Ù!¡\¶\´\±³~\╦.¥k\Þ\þ\┘`\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\Z\╬)7\╦w\ÐD\ràîûvG#sÜ\¸Á«OÛè¿èvù°è«ûæ85\Í31i╗╔ïî^\ÐK\§É\ı·w°■?e\Î_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\ÞóÅS\Ôò1`ÿ«\╚\¤▄çk~²¡~\µÂ\¤Îà\█\õ\┼\ã/\ÞÑ·\╚j¢;³▓¹»Ü·9\÷X\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0å│èM\‗\¦\¶QG\Ú°\╠_5ƒ╣\Í²¹Z²\═mƒ»│╔ïî^\ÐK\§É\ızw°■?e\¸_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\ÞóêZ\ı3Q┘Á5T\§,ºû+\┘+\ı¼r*`½è*a \0e\Ã\Èvù³▒u┤╔â]cÆm#*¹\‗b\Òç\¶R²d5~Ø■?Å\┘u\Î\═}¹,\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0CY\┼&¨n·(üú\¶³jÜ\¤▄çk~²¡~\µÂ\¤Îà\┘\õ\┼\ã/\ÞÑ·\╚j¢;³▓¹»Ü·9\÷X\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0å│èM\‗\¦\¶QG\Ú°\╠_5ƒ╣\Í²¹Z²\═mƒ»│╔ïî^\ÐK\§É\ızw°■?e\¸_5\¶s\ý░\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\rgø\Õ╗\ÞóÅ\Ë\±ÿ¥k?r¡¹\÷Á¹Ü\█?^gô╝?óù\Ù!¬\¶\´\±³~\╦\¯¥k\Þ\þ\┘`\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\Z\╬)7\╦w\ÐD\rº\Ò1|\Í~\õ;[\¸\Ýk\¸5Â~╝.\¤&.1xE/\ÍCU\Ú\▀\Ò°²ù\¦|\Î\Ð¤▓└\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\05£Roû´óê\Z?O\ãb¨¼²\╚vÀ\´\┌\Î\¯kl²x]×L\\b\­■è_¼å½Ë┐\Ã\±¹/║¨»úƒeÇ\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0k8ñ\▀-\▀E4zE®ë8F~\õ;[\¸\Ýk\¸5Â~╝.\▀&.1xE/\ÍCU\Ú\▀\Ò°²ù\¦|\Î\Ð¤▓└\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\05£Roû´óê\Z?O\ãb¨¼²\╚vÀ\´\┌\Î\¯kl²xY╣┐Wb\õIlIy+\▀Lòia╠ºÆUrÁ_Å\Z©z\Ë\Î²LK\¦\Ým¹¬r\õ\╦\÷[w«Ü2\÷Ñk\¯\¸Æ\Ý;S\ı\ı>Ñ\ıÎ¢▄▓sÂxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×&\´y.ËÁ=]S\ßì]{\¦╔Ø│\─\¦\´%\┌vº½¬|1½»{╣3Âxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×&\´y.ËÁ=]S\ßì]{\¦╔Ø│\─\¦\´%\┌vº½¬|1½»{╣3Âxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×&\´y.ËÁ=]S\ßì]{\¦╔Ø│\─\¦\´%\┌vº½¬|1½»{╣3Âxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×&\´y.ËÁ=]S\ßì]{\¦╔Ø│\─\¦\´%\┌vº½¬|1½»{╣3Âxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×&\´y.ËÁ=]S\ßì]{\¦╔Ø│\─\¦\´%\┌vº½¬|1½»{╣3Âxø¢\õ╗N\È\§uOå5u\´w&v\¤w╝ùi┌×«®\­ã«¢\¯\õ\╬\┘\Ôn\¸Æ\Ý;S\ı\ı>\ıÎ¢▄Ö\█<M\Ì\‗]ºjz║º\├\Z║\¸╗ô;gë╗\ÌK┤\ÝOWT°cW^\¸rgl\±7{\╔vØ®\Û\Ûƒj\Ù\Ì\¯L\Ý×(\þ╦¥LféHÖn\Èg=¬\È\ã¤®D\┼S·\µÄ¢n\õ\¤Y\Ô\ı·~3\═g\¯C¿┐~Í┐s\¤ÎäÆYûäNV║æUS\¾Iç\È─ÅLX¨\Ë<║¢\µ\'Ãÿ\Î\'\‗oÚ│┤Øoa│<║Öè▒<╩╗\▄\▀\Ëoh\Í\÷3╦®ÿ½Æ▒8|\Ð²6\÷ìqa│<║Öè▒c═½=\═²6\÷ìoc│<║Öè▒c\═\Û\Ë¨G\¶\█\┌5┼å\╠\‗\Ûf*┼ìå½\¦\Êoh\Î3╦®ÿ½*ñ■NNô{F©░┘×]L\┼X░¼®O\õ\õ\Ú7┤ìqa│<║Öè▒3j=\╬Nô{F©░┘×]L\┼X▒■c\▄\Õ\Ú7┤kï\rÖ\Õ\È\╠Uï\¤\¯r\¶ø\┌5┼å\╠\‗\Ûf*\─\ãtôñ\Ì\Êu┼å\╠\‗\Ûf*┼î¨¢\ÊNô{H\Î3╦®ÿ½6I¢\╬^ô{I\Î3╦®ÿ¿\┘&\¸9zM\Ý#\\Xl\¤.ªb¼Mû_sùñ\Ì\Êu┼å\╠\‗\Ûf*┼ìÜ_sùØ¢ú\\Xl\¤.ªb¼E×D■Nnv\÷æ«,6gùS1V,l\‗{£\▄\Ý\Ý\'\\Xl\¤.ªb¼O8ô\▄\µ\þoi\Z\Ô\├fyu3by─×\þ7;{F©░┘×]L\┼X×p \0søØ¢ú\\Xl\¤.ªb¼O8╣\═\╬\ÌÐ«,6gùS1V\'£?\▄\µ\þoh\Î3╦®ÿ½\╬\¯ssÀ┤kï\rÖ\Õ\È\╠Uë\þ\¸9╣\█\┌5┼å\╠\‗\Ûf*\─\¾ç¹£\▄\Ý\Ý\Z\Ô\├fyu3by─×\þ7;{F©░┘×]L\┼X×q\'╣\═\╬\ÌÐ«,6gùS1V&\¤\'╣\═\╬\Ì\Êu┼å\╠\‗\Ûf*\─\┘\õ\¸9╣\█\┌F©░┘×]L\┼X││K\¯r¾À┤Øqa│<║Öè▒geòôùñ\Ì\Ê5┼å\╠\‗\Ûf*\─\┘&\¸9zM\Ý\Z\Ô\├fyu3b\╩:u■N^ô;I\Î3╦®ÿ½\Ã\¸9:L\Ý\Z\Ô\├	\Õ\È\╠Uï?\µ=\╬^ô{F©░┘×]L\┼Xƒ\µ=\╬^ô{F©░┘×]L\┼X▓ì®^3ôñ\ÌÐ«,0×]L\┼XøW╣\╔\Êgh\Í\÷3╦®ÿ½v*»sôñ\ÌÐ¡\ý6gùS1V,\ý5K³ú·l\Ý\Z\Ô\├	\Õ\È\╠T¤ø\ı¹ø·m\Ý\Z\Ô\├	\Õ\È\╠Uï)MX¥¬7\¶\█\┌5¢å\╠\‗\Ûf*\─JZ\ı\Ó\¾7\¶\█\┌5┼å╦®ÿ½ÎÖ\Î{ø·m\Ý\Z\Ô\├	\Õ\È\╠Uï)C\\┐╔┐ª\ÌÐ«,0×]L\┼X│\Þ· \0twM¢ú\\Xa<║Öè▒vÂ\r▀×zÞª¡F\┼nG½U╚«vè\'®;þñó\Ì\╬l\ý\Ò$OÄW║,╗3û_ \┘','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',NULL),(21,'real_image_1',400,_binary 'ëPNG\r\n\Z\n\0\0\0\rIHDR\0\0\0\0\0\0\0\0\0─ë\0\0\0\rIDATx\┌cd`°_\0çÇ\ÙG║Æ\0\0\0\0IEND«B`é','08dcd23c-d4eb-456b-87e4-73837709fada',125),(22,'real_image_2',400,_binary 'ëPNG\r\n\Z\n\0\0\0\rIHDR\0\0\0\0\0\0\0\0\0─ë\0\0\0\rIDATx\┌c³ ƒí\0é=\╚H\´\0\0\0\0IEND«B`é','08dcd23c-d4eb-45db-87e4-73837709fada',126),(23,'real_image_3',400,_binary 'ëPNG\r\n\Z\n\0\0\0\rIHDR\0\0\0\0\0\0\0\0\0─ë\0\0\0\rIDATx\┌c³ ƒí\0é=\╚H\´\0\0\0\0IEND«B`é','08dcd23c-d4eb-45db-88e4-73837709fada',127),(24,'real_image_1b',400,_binary 'ëPNG\r\n\Z\n\0\0\0\rIHDR\0\0\0\0\0\0\0\0\0─ë\0\0\0\rIDATx\┌cd`°_\0çÇ\ÙG║Æ\0\0\0\0IEND«B`é','08dcd23c-d4eb-456b-87e4-73837709fada',125),(25,'real_image_1c',400,_binary 'ëPNG\r\n\Z\n\0\0\0\rIHDR\0\0\0\0\0\0\0\0\0─ë\0\0\0\rIDATx\┌cd`°_\0çÇ\ÙG║Æ\0\0\0\0IEND«B`é','08dcd23c-d4eb-456b-87e4-73837709fada',125),(26,'AA1350B937719558DC5F6BD46998D097',100,_binary ' \Ï \Ó\0JFIF\0\0\0\0\0\0 \█\0C\0		\n !%0)!#-$*9*-13666 (;?:4>0563 \█\0C			3\"\"33333333333333333333333333333333333333333333333333 └\0\0¨É\"\0 \─\0\0\0\0\0\0\0\0\0\0\0\0\0\0 \─\0P\0	\0\0!1\"AQa2qüæ#BRí▒┴3br\Ð\­45sté$6Æ▓\ß\±%CDSc\'7ETdó│\┬\Ê \─\0\0\0\0\0\0\0\0\0\0\0\0\0\0 \─\0%\0\0\0\0\0\0\0\0!1AQ\"2Baü \┌\0\0\0?\0õÜì╝Q\¦\┼-┐{Ky\Ô«:\¤r¹\Ô@Ø\┌■ô╣\Ù\¸½}\╚7Tc┤ê°\ÕMG\ÞèV~°æ┴?Z═ÆèÉ¥V|t\┌<Å\ËÍê\┌i \01~\þ║pTÆ»î»\Ù~uÞúèyî\Ô4èV\0▒S\þ³)ôC47®­╣î╝ä\¶\§\÷¿Nå\¤wSI5┤rJ\ÞF\­[çA£p\Ìur\¤J╝x\┌+FGe\╦~\╔\Þ\‗5K\Ôci\▀\Ô\Ï\õCî*\Ò£\¶■u\¶Èæìû│Jèé\0\±g\Î>ƒZÑN[\Ê5p_╔ªöÁrÆ\═¦│v\ð=¢\Ù=2ï¹{\╦└dy#\\\╚\‗\±▀«}=A²┬ød┌ªúw*[\╔vÀ\╔(>o\ýâ\ÙE\Òé\¦IvÆ\╔we(\▀VM└ÉÁ\ËK&ƒF.5▓┤v7pvy.l\‗É«\╚▄½<z\±Elno\¦\¯/iª1ü\"p> s\├OjÁ\┘×■\Ì\¯┴┬øt.Ð«dn\Ú\Ú┴½║ãà.í{ko3=\┬X╝ñàTaÐ¢\╔³½xbQÅ(ÿ\╦%¢ùmnÓ╣▒║©Ä7Æ+ÿ╗°\Ô\Ã\Õ█æ┴Ñ░\ÛÉY\¤uïq\╩+\­\╠G:6ZÖ\ÓÆ_àD°käW\r╣1Æ\Ò\¤×ò\Ú\"Á\Ë\§K5▒æ¥U\r░1`PÄOƒJ\Ë\ýÑl¤éoF×+kkta1\ãY\‗Jîµí┐ûx$I\'Ä6ölP\═\µx\Ú\þ\¶í\ÍwZ| \\ú\¤fîP\╩9h\▀\ıi5]*{½b%É\¦\╦q q└0?f┤\þp\Ê3Ó╣Ö=Q}F;uXa8n\┌:ƒj=ú\┘\▄\¤t.cø`CG┤\‗=sA.óÁ©\È\╩:øiT\ý,º*8\Ã_SG\¶9ùIè9X~Ém©`<ìqc²d\┘\Î4ötj· ×xºè`,A!JÇi\Ô¢$y╗Àg½\ı\Û\§1è)qIÊ£:PK^»PbÆƒèB9ñ└\­Ññ┤Ç\§xRf£(óö\n\­╣áhQ\┼{4Öñ\═!\Ï\Ô\ı\õb╗ì3®æs1Iìå	.øj&\¾£\Ò8ó\÷sSò┴\¯c\¯\§\r£\Ýg&\ßî	5»│uò^\§ÖH\þ5åLÆÄæÐïeÂ░\ý│wà\´/ó-C®\÷fHyg\Ô°ü\‗«B½\þ¤¿º\nÆ\'\¤Ì╣\ÍiY\Ë/Åî<:,q═Â\‗fÅî¿bƒ®iä\Íq│`r\─\ÐkÁmN\Ò╗\¯Ü%C─ç»┌½\╦\┘█╣b(ùøW\▀95┤r¨fS\┼Jæô\ÏS*sƒz\÷(\┼\▀go\Ý┴m½ \§J¬t½Èç¢h\0\Ò¡tG$Y\╚\±╚úè\÷)¹|D╦Å&\ÙIîu½┤\╚\┌\ýh\­)Æ╝ÜÆÖTüÙÜùE½eGCè▒\r\¤vÇe\┘s\‗└½BH\néâ\'\¯5Y¡\ı3┐|R¨®bª\Ë/ï8iÀ°╗Kø\█m\±Z\ã\ã1&O-TúÀ©e,Ã╝╣║ò¿\¶µÁÂ\ZÌùaáÑöS╝S@#f\ÝÝ×ñ¨²h>½{qt®u)åU`Jêó¨O®\§»;&8ÑªwFR}öb?Qöa!&6\Û~┐\ÒMÂY─ªFø*\¤\Ôà9r=x\╔ys\þÏ¼\0!wg\Òv*EÁØdæ	¦ág°I__c\\¡!À@]Z+\╚\Ãj¥$*╝»<T╬ƒ<\¤¦ÆvÉwÆ1ò\¤\╦\¸óB\ÝºÄIä@\0Cî\¯=9>©\¾½V&\µU(\Ú▓\Ú\╬\õr©c\Þ\Ý\ËÍ£\ÞM«\┘kJ\Zz┼ÿ\È\┼{\nô\'\n\‗\"ú[\raÁïï\Ý20\╠\±ƒ\Óe▒âƒ_j7a┘ï}J\Ï\¤v\─\Ã\Õ_q\‗ó\┌f¡¼34ÄA;|U\¦Y%«æÃô*O@\r>\µ¹MÂÄK½Y¡\õr!	 ×O▄×j\¦\═ã║Â\╦5\È/\­\Ù*s░z~\╚\¶\¾¡®º\ÚÍªK¨UBÇvyÀ\█Ê▓7wù}ú\È#é\╩w]<oC·C\Îw»Áj\±\­\‗Dd\µ\¯ê\§\´-»\Òè\ÝngòÆV]ø7\­AQ\ã0i\¸\Z}\¦µ×Æ█½I\▄2¿ÆäæWé=ê4\¦*kï[\█1#ø¨öx$\▄c#▀Äòó\Ð]\¶¥\¤I\─`5▒o\Ð(\ã\­O¼°\‗¢ù)qZ\'\Ê\´ Ä\─\█\╔!îùc\"║î»▒\§ó\"\Û\ÍUS\╠&2pHla¢fu═┤┤╗YnL▓H$v\råU=Tz²+\ÎqZ,0\¦w\­ÎâàêmET\þ\'\▄·zÍ▒\╦8~QƒÍñ\Ý░Äúñ\█\├a<░DèÐÀy│\µO»Ñ[K\█\Þkàê¡ó£╝ä\õ}³\┼j\¶\±\Í*\­,Ø▄Ç\ËzU¡╗I\'î`Ä=\ÙO®M\‗3¹t\ðu	.\ßhÕë│	\ã\Ò·\├\Î\ÚG5PF┼ùif\Óà└ºÔÁäh\╩N─»\nZ\­½$\§8tñ»PÎ®+┘ábµÉÜCI@ù4┴KÜT!┬öSAÑ\═1\ÓÎ│L\═.hhÄò\´:h4á\Ê\ÓS\├\ý\0ÁF\ZöI┤\‗2*Jï-¡¢\█Ã║;ve<g╩»E½\▄Ï¬[P<\¾Cß╝û6R▓m\\\¶®à\▄R^Æ=\Ò╬í½f\±u\õ┐o½M5\ÞT~(½_L	\┌²\µ8!:è¤øÈÄ°I\Z\─WÊî\█vé\ð\Z$î×©\¾¼\'H\Ì¥\┘~È╝½î▒\Ã##í½ê\ÕÄ0#╠èºm½[K&\Èf¼¿À \0X┐Ø`´úó-s×NsN¨å6Å¢W7ê¡LÄYdbªÜ)8░^¡ó[\▀!*r■\Ï\ÚX®!\"VÅô┤\Ó£zWKFWB\r┐Êúû7\┌└y\þ8¡▒e¡¨pª\ý\╠┴ú¢\▄yëù t>u5┐foÖ[¢ƒSG┤\Úß▒│`q\ß\Ûs£È®¿\█\╠ò£68\ÙU,▓D\Ã_E{?®îâ0\Û\═ÊïGd@║Hñlu\Ã5$\r\▀D	\Ô¼m\¶¼\\┘╝bæ\±³61B\Zaæ@I`s\´V$Ö`Â\ÌH▒m\─e\ãsV{▀è©]ós \0Wô\‗░\¸\¾ºI\Ì+Ej╔£Öd<:î{{W#ï~FÇÆ░Ät\±W\├\Þ}OÍêwHc©\´å\ý2#GÖ>¥\ıæA┴ \±ðÅj#ö		èf\r\╬\¸éá×Ö;FâK┴\Ô\¯\ã\÷%à\õƒ|Q;\Úm\╠qBÐÉ\Ð\µ7<×8\╚\§·UeÄ\ÌDìb┐]\Ð\╚ZI6\Ò+ÅOJí\ýh\\4KùF\'æé:Q4KVl,\´\ßèvå]B\Ô	ZC\Ô\┌\'\╠zU\ÞÁ\╦ky$\\\╚B\§ëKÀªJ\╩]\\^|43╝`n%U\÷éwy■43.\ÚaiåC\Õ\ÛMuC\Õ\╔*0û═û▒«├¬\┼\¦mç\ß\╦\r²\õgæÄÖ¬·¢\Ê\┘,QK┤-Á@Nöt3©[ê\Ík\ÚUm\╩\0░ú°\¸ytôi\Õ\¯ówiÑ┤\¯\­J\─	T\¾c\Þ?}Dº9ìF1T┤k}4àÑÖ\Ó]\­(\ã6£\§¨│VÁF\÷\Ôêÿ\ÔÜP%ä¿$Ào\þQÀgm¡\"Ä\ßg©Ü\▀?ñÖ\╬\ÈV\Ã\0·y*ú>\¶âHî,^<îéâ\µ\╔\¶&¡réªO\═fØ=ÑªØ┤vñ╣üH\0Ø▓,\µh¡ñ\Ó;Á2O¦£ùh\Û>\ıZ\Í[}Gî\"³<àVHS#\¶L<Ã░\¶ó\ßûLnSéÝÅÿÒ®»B*2VÄLì\┼\ÐGHé\÷	#¢\█²cw|\¶Rxó8ºÌ╝\0\Ã\╩\‗╔»Æ	\╔\Ù[E$î$\█=\═{Á\Û½P\▄W®Hñ\┼\0zûô┤\ýRR\Êbüí9ñ\µØIè\0J\§.+ÏáWà{áR░=^»b¢è\┼\Û\­\ýR┬¢ô\ÙH)qÜ\0U$â\ÕJ)Êüè<▒\┘\═.rp\'\ÈR\nÆ8ÜBíc×ÇÊ║\┘TÌÉ\Þªxü\nB·\¾N7Oƒ£\ı\█\█{mS\¶y©>ç;O¢\rX├£3\´P©À\Ðñ╗\'K\ÚH!d9_/Z\Ê\Þ\Î\ÊMjKÇñqî\§¼║åRÑUØ~lQØ>H&æHqû	¿\╦F\Ïd³Üêv0\¤A\ÚQ\▀D\‗\┘╚▒(n:P\Òt\Ð▄ê\ßæ0<ÿ\§½\±\¦┼╝│J\Ò\Ý\\|ZvvrÁFf+Y\Ý{*ØÇ\¾\¾ú\Í|EÂD\\.8¿\§G2┘▒M¼®\┬PnQ#\¯\'\÷░+G¹[2_ùúZæ░\\\ß\¶º½É°\╚¹TüGÿ¼Y▓>Fû\▄\Ì$\ß.ÑÄÏòPê╗â\­\§\¶\Ù½[©\Z(> 9H▓är#\\\‗¢[╝ûh\Ýú[_\Ð┬º\¶▓┴\╔ \0\n®ºmÁ\ÈR└%ÆlaÆA\╬SY\Û┤S/f0:Jd║ìÈà*x\┌8\Ò\Ú^Á║Æ\Ì└4à¹á\Ò;\Ã2\þ╔¢3M{ëvÑ\─(╗É\┬¨\¶úZç=\µê.\¯╗XGâ$R\Z¿<=	>UÜV┴░fòpn\Õ£\\lX░{\ãC║¿Ç\┬U[f+▀Âmá\'\Ëo\\\Ð─ùQIc?\┬\┼uz!hêx\Ò\ÞKJ·^│mz\÷»-Á╣\0`:æ\ß#\ÝT\±ð¼û9×ïç3\¸qåW\¯\├p}pkSªhÚ®Ö\Ì\¯Xa╗ÉÇoú\´\Ý\¸¼Dfê¿-\Ô\\É¥■ã»X\Û\‗G,!I3ùSä<t\þË¡	\┼=óemh\ËC\ÏãÂ╝â·H└\­;ÀxU©>cÅ2z\Ð+{¿┤\¾{qdÆN\┬8¹╚ÿmT8a\Û┤9u®5\r<¡\È\±\¸eH(N\Ôy\Óît½║lûCqr·\\û▓\ßí2,áx}@=>\§ÐÄP¢£\‗NïPu\╦´üû(\┬)\▀)Sÿ\Õb<ù\╠c╬½\Û║9\Ën-×Ðªke\╠	NUX\õ\õ¨\0kY\rñ_.\Òn\ý┤kL@\þ»J¼\Ï\╔q<SGk$\õº╗}╗\\¹\ÎC\Ãp│╬ÑE=;L\Ê4\õ	o$\Ê¯öîâ©½Åƒƒ!Ëè\Ê\0y\\uæÁ▒║╣hä│IfLàú-é[?88\§\ÔÁ└ä\nGE¨Íÿ$\Èjê\═\¦\┘\Ó8»bùnsU.\§Kì/.\"¨)©£u¡█«\╠T[ÞÁèmRMgMÿÀ░xN0[4w\Í/ìù░7ªRY\"\╩·\õï\0R\┼y^\'O\ã\├ØÔ£╝`Çph\Õa\┬^åR\Ë\­\Ï\Ô?/1\´Ið×|°ºk\ÏS\¶2¢O\'\ıu&\Õ\§\Ã┌ï\n\ýSêÑ \╔\ÐaBb¢èp┤f+└S\Ù┬É\Þn+Ïº\Ê@\Ê\n\÷)ÏÑ\┼íÿÑºbôBRf╝E&(\§5*;!\╩¿¿E:Ç,\█Bn%╠│\ý\±7×)&üm▒╣æh\±PÇ|║¹Èæ\Ã,\┘ú1qÊÑ\Ê)[	Y\┘B\Ð\­\÷\Zè\ÏhÂLà\÷\╚û[<\ðk[Â\▄\Ð└B¨ÉhãØ\¯\▄K3\ãs\ã=+	\´\╔ËÄ5■Ñ\Ú4\█h-ÑÉçiF¼\õË╗ätb\þ\Ò\ðwk·hïrZó\Ë\¶sx\╦4»\ß\Ìrƒzÿ\È{.QoÑ@┐ìÖúÏñä\¾½\Íeê\╔▒\ÃT¡║eò\ãû\Ó\ÒÄ8\┼>+kTeé9®yùí\Ã\÷\rû\¯;uø\±\´J║\Þ+ÉÇ¹\¯\┼]ùKÂò	« \0#Ü\÷\¸û╣Ahô \õP©▓\▀$|\┘,\Ê\┌\█4É\╚\µfQ\ß#z1\¾=(c\┌I)\´ºH1àî·ü¹½Gem1,Å S┼▒í\╬6âÈ▒\‗¿\§íWP¤ü4fv▀Â^²³KÅLW#TìøDÂ\÷v▒úe\Ï\Ý_É\±\Ã\█╬êÞÀæ\┘IAJ \'\╦\‗\ÛG\­>\ãR\±\╚\Õè1\nü╣<¨T\¯Y\▄▒\Ã\╩\ÙéqËÜ\þ\µ\Ë[\Ùfmj\±í╝\È\\■ƒ$\õ®\Ó(^ÿ\÷\§\µ¬j²¼║╝©X,l\ZÅ8@\Ï\ÃXt¼{\▄+\█\╔─èîî\├`\ã\Ã\È¹è▒d\Îp═à|l`╗ıÀæ┴\÷\Ô┤¹ÑÕèÉ7QâQæ\┌\µY■!Ç@\­g\\yP¹hVfíÜB21\┬VèU×dòoxÉ├ûf\÷42\▀O6Æã¢³\┬Y╔Ç\╬G<¨\±Ký░á\¸g┤K\Ùà\¦lÆ1ìÉªwF=\0\¸ó\¸Â\Ð\ÏM\╔oë\╬\Õ\r\╚`?Y\Ã\­í:dùVû│³%\╦┴nèDj└89\¸½V \0\õ\ÊGYèffQ·ª¡eåòl\╩PòÂn\ý.·\ãdë\"wó\¶8\¾aF9\╔#<{{U;æHÕÀÜ\¯\╔ú)ƒ\´U\Ïn!ÿ\rÄ	#í\Ù^\╬)@\¾▓\Ãc×8┘ÀÎâòöáÉ\┘\╬x┴Á!R\"øè\Ðkó\Z¥ãÅ\n9└¼k\þI;Mc`\ÓdB\¦Á \0Ño¨ÆO\¾«5\█kµôÁ\¸2\ã\ÒtN░\­z‗¼▓¢³ulÿ\Ã\═\Zë:\Þ\╩ru.╚üH\Ú\ãUå\±S@\ÊNâ*\Õf·~\Ûj¿-¦ƒ\"Q¢ü\±/\‗«=\nD+!\ß\µá\¶sé\╦\È}\Û└û\ÓHB^╠ú#iså\Ó~·`s┤8$\╚\0ôo\÷ù\µ■4à¿CÇ╣┘ƒ=º\Õ?×h░\Ôï\Ûzñx)89\ÞpG\¾½\Ù·\╚\0&á\╠O\n$³┐ÿ\═Q,\█wm\ã?H\÷çäÅ\╦&ô\├\'\È\Ô5aËƒÉ²åi\‗b\ßAt\ÝN┤ç\"\ß]YG\╩N³·È®\█aG\Ú!ü░	?ƒ¨4m#ÆB¥=\­\├\ýiHf\╬t\'á qƒ¬\ı,│^Ix`¹FÄ>\┌]®\Ë\Ól£(Wn}?:Ö;u╗\¶\µê┤·\­3\§5ö[9vU8\±cS\ð²WºÌØðØ╦ÇsÉ\Õ\¸G#\ÙOÝƒ▒<8²\Ý¢ª\ð\Ê\┌\\*¨ò\0\Ó\þûje\Ý«És\Ìê\±\Îr\¶\ÃZ\┼r\╩t#wÀO¹╝²k\├9\­F6\´\¸s\§}\Ûûi/$ \0<HÌ»k4b2\Î[=C)╚®ô┤z9p¡¿B╣\ÞNq\\\µY ÀM\‗Kàíö`£t\¤\¾íGS°®\Í\ËLÀ2\╚¨\├:\õ\µ¤òR\╬\Îd \0<=Øòu;\ß.\ßf\‗Py5:\\\█6O|ÿ\Þ<C¡s½n\╩\Û6Z3jê\Z]CPF\▀e\¶¬Eí\Èck╗P\Ð▄á\┼\╠▓ho?Z_\Ê╩╝3½å\õ¿?\Ì\Ó¼z\0~å╣ÜD¨..Â\¶\01Oí■?Jòo\´Pùù\╬2×¥UK\õ■3:\ã\ÍTÅÁ&A\Ò╬╣rkz╝.Ñ:\§<\¾■q\þ[>\╠\ÙS│6\¾K\Ì]\─<l~f\¸¡aòH\╩x\\C¥Ã¡{m8üÉ|└┴\÷ÑÁ1)\Óf¢èQ\┼\09F\Z¢\´uÄ\Ýƒ2Zó~R|¢i\0\ãFzuñ\Ê\ÚìNé\Ý¼HpvÀ\Ù\ðıê\§╣\nìÞñÄÇ\Z╝é$\þáJ\"·U @FA~+)\┬+Âk\═\¶_âVùæ£.sé3F-n\"*Y0O\¯¼\┘\Ên\Í2╩ë J6j¼FX\Õ\╔\ÓÅ,\Í<\Þ\ıdö\╚\█Gq¢ÿ(·ƒZøÅ\┘\ÙY\█}fN\ã<=q³\Þ\╠7Ø\ÛTî\Í2âGD2&+\Ì,1╗?ä/¿\ÙU,\§e╗ÉäBúÍ¼_D▓\┌1e,GJ½cyo\ZàæcY:dt4F½áôi£\r4ì6\Ô\ã7\È&ÖoV?KÐ▒\õk7¿Z>û╬«C\Ã6\Z?\ýA\ÙW/\µi.\ß╗I\Z\┌WB\╚3æ/\ÎÊ¿\\jâQ│Ä\▄\´Y\÷Â\§\Ú\¶\÷\═d\▄Z\niÉAw\Ì\▄ÿæ_p\ãƒ\´½S]äw\Ùü\¾å\þhö*\0\­\\\▄+©g\¦\├núj\ÍÖE─¥ \Ã0©¤º5\¤(«\═I¼\ÕÜ\Û\Õ$ÖZl£aåÊ©\Úô\þE\ıµÂ©+$│pYÿ\‗s\ðÑRÀà\┌`æ!½©<.<\ÙQó\Þ\Ê\ÌYÁ░fÆ9Nd.¥/;ü=Êöa╚ù*\╚LíT\´\rƒ\Ê0\¾¬^I█ê\┌yû`Gw╗ª<¥ıºùC╝ÆG│ÆUH\╚▒+\Zg\§┴\÷\¸í:$V▒\Ùeg·ìØÿ\▀s*¿y%AÈÇW9\Ú\═ZÃ║òìå\µ#\"LðáH\÷\±Ø┼Ä<@\´D┤8./ä│\┌\╚\¦┴É®W,yü\ÚÅ\"k]yc┘ìn(\Z8┼á#┐âé¼áq\ÕÎôÊ®C\┘}kF©ëá─ÇÂ¨çóÄçÄ>Áj\nd╔Â┤Oº\Ú┌Áô|<ªøIäW¿\╬z\¾\ÚF\ÒDëp▒¬c\╦(×ƒº\├xéê6\ß\ı|¢®\¸·zZát\'[║ÎÑåP½G\±╦Â	*ÑÀ`\¯ñ\"ª\┌_\ÕP=╔ñ┤ç└ä\Ò¿╗i#¼»\'\Þ\¸7\ýì▀É\═p]NQy½\▄═î³E\╦Kƒ¡wM^V│Ð».\õR½Mû?¬╬©\'1ú;\"CÜ\þ\¤+\Þ\Û°Ð«├Üd\ã\þIÇ\╚8V10\§S└?ØX!\╠|ØÖC<\╦)\Ò\¸\n│×mt\█Qåx6\´?¥ÿ┼è│»ë\­%N>R<-³ÙæØïc`w©\nìÂ`¥x?0■ðØ#_Ss¨®³\═<ì\┘v\¯\n°\╔\‗ì \0\¶ñûQ\±HAç$\Ò× xñweC¬æƒg\ÈxXN([)É\"0\├\÷O\╩▀║╝\ãYO\ÚTï·¼?ëº, ╣Ç>~îƒ\ý×T■\ÛvKÅ║u²&\▀ \0æ,Rl\\\ß\Ï\ýkƒ \0rƒ\╦£ë¦î╣\±ò■\ð¨ù\‗\┼{á\¯>\­ô²ô\Ô■\¸k*\╠A\ÏW8■\╔¨\Ã\Ï\Ôö╝å\├88\█\ÛG$\═Êá×\§-å\¸?ñrº░\ßù\´\Î\ÝA.ÁÖD6╔┤pä\‗@<▒¬QlMñ©╣ÀÁÅ2╦Â,aHõæ£\þ\ý|4\Z\Ù_Ö┐Ek\n¬\õü+£ƒø\¸\¶á\‗3O31æÿ\§$\¶gB\ý\§Ì│**+,D\Ó░À\Ë³i\Ê]è\█\Þ½ea{¡\¦wq1ûM\Ì\'~Ç¹{¹WV\ý\¸f,t;5\┌\"\╚@\´2z¹ƒ\­GB\ý\õ\Zdq\█\┌\┼ômÖG\n}~Á»M┘¡L3\ã%\'\µv¼Ñ6·+»:║\Ýd▒\▀\╠cÁcm{c.2e>M\Ã\╦Cnl\Ò\Î-╬Áíb\rF\█·¹oA\µq\µ+Q«vx\Ú‗éá¢╣\Þ\├╩▓oouºjw`{╣ô\─=zZ-è\╩\0┼®\┬\Î\±ö©Qïïo\┘■\ð\÷>~\ıL\Ãq\Õy\Ã\±·y{Vå\¯\┼5\Ï╗óüo®C■▒n?[Èü³¿c\È\Òi\ß\╠W®\┼═░\÷ç┐Á5 óù\▀y·y²\¸\È\÷wS\Ï\ÌGylHû#æÄì×ú³}¬\\┐ä)┴\╬\Ê:²çù¡8\‗└ò┴`?¤Ö³¬Ëºí5jÖ\ıt\█°5;8\¯a`█ç ~í\¾\ß\ÝV\±\\█│\┌\╦h\¸ÿñä	@\Ûôo\ß]%YOïpdaö+Ðç¿\÷«\▄y9#â,8▒qJ\09\Ë5\Ó@R\\Ç\‗}h4¢áì.k#\'9#┌×LïdF]\¯&[;G©ò³ gÁE\§╗e#\¶!19¨\¾B╗I®G6×®U┬ù\Ã%°\‗\ÃJ\╚.í*êñp\Ûê\Ì5¤ï\█©s³ƒ\┌q7ågJ¢\ÎOté\ré\µ^Ø\þ*ö\Ë\█]j■éÄ\Ì(╔îÃÁ!>Ö\Ú\§¼#▄ø¹╠┤ÆaO*@\¤L\§½²▄êQ£¿\╬┴V>ç«r\Õ\╠\õ¹:a&█│Ü┼«å\═c\¦\¤:╦â\╔9\´3\Ô\Ù\õk.\Í+øS\▄xƒ«\Ý┬╣\"^M\±ï<s8\╬├ƒ\0\¾ÍÁÜm\¶\‗[©╝ud\┬\Í \0{ú<ÀA0eîä/ûyóV·\─\­ª└¬q\µhVX\õÄÿÑ\0g>uÞ©ªÄH\õÆa°\§\¦\╚\¦\Ûe²GJ¿┌Ç╣ü\ÊH\ÐH>CW\┬r)¨$×Ø*>©«ï¹$¹87h	}@\ãÏÿpº\╠?h·U;YY┴É>³p	ê\Ã8>ıÀùS;dÂ│\ÊY.\Ê ½>\═\┘Q\ÐG¨µ▓Às<\"im\ÓHF0\╩╦îô\Ý\ÙÜ\¾▓*gdÖ\ý\▄]\´╝\'%ÿ½GÀ£ô³+J¿\±X\¸BCJ@_º¢░┤`cE{\┬>f\'<¹×\§zI\"©*K¢ê\▄v\­Á¤æÐ▓\r\Ú╔ÑùI.eq\╚S4m\¾{b║Ìüoú\¸┤\ÚH■!\¾gn<\±\õ+ìZF6)ìU8y1©ô\Þã┤zvøo[ØV\Í▀╣`QU\­\Ï\ÃN╝Íÿe?³3Æv\╩N\ÈkªÁh4¨\'00U¨ç\Ý7\÷=╝\Û}#▓\ÎZ\ÈvVQÿ\ÝF\Í\±.d\´\±g¹x[EÎÁ©x¥*\Ír\¸\¾7<½c{½▒\Ë\µ{åK└á.\ÓX}└\‗\¾«ò[3\ÕZ3\Z\Õ\╠║╣▓Â│\n\±úJ^I8îû\¤\§s\ÚZ¥\¤Z\┬m\ZU╗©%\ãdëñ\╚F?2ò\¶\¶¼.│emg{bù7ÄoÜ6Æiö\Ës┤7@É5g│·øK®1┤v[â\ÌlkêX┤¬1é[ªO¡MTÂ]¬:\\\­┘®°x@\¦\È/ØPıÑ`D%r=zÊ«¿┴\ÕC\Ì\Ý\├mÚÜñ\‗=\├g\þ9«£pi┘å\\ë¬╝F`│\ÏFxó░\¤dj}q\═	╩Ö\0w8╩×%³ä¡i+fQq@_─╗ä_\├²J\¦0\Z\ý¼@²\├*\Ó\ÕD\¾¨\¾¼@{\Z\Û_èùÆ«ÖºZâ\├\\w\═\¶\0è\þÜ4bm^\╩5MÊëDÇ{Á\═4\ý\Ù\ã\ıØR\rÜxt¨¡\÷æÅAÍÇ6\Ï\Ï\§G&¨òq×>µÁ»\ZMæ\¯2\Ò\Î\"▓ÆG\Ôìd▓ÿv¹ÄG\­®æQ\"1Æ	p\rn\┼y?\Ï4\ÃfÉéW\Ú¢@\¾t\Ù¹▒N\╦\╚wÄ·h\¸Æ|ØiI└ÆHcR\'WoC├Å\╚Tö{;IbAH\­\Ûâ\═X`■\¾HÉàQÆ\0²	q\´\‗\¦NP¿Jü▒\Ýb\¦J7#\¸Ü\÷\┬\╩#bT\Ý06h|ç\‗\═\0E$\╠IÀÄ³Å&0¹îP\╦\þI\┼y\╚■\╔\þ°ÐïÙì░\´*_\Ý#\Õa\¾\╚VRY\╩\╬v\ÓÂGÑ\\ë▒»,ô\╬Aa\╬q\ð·\Èl╗ö(\┬\Ý\¶\Ò╦ëL`\ý¬\Ð\¦+\▀<2\0·U\╩ihò│}óv\‗\‗X%╣hZT:\¸2n}OÌ║UñZnò=¥×cyôîÄ1\Ý\¶«O┘ï\Ù─║:òî\Ýj\¯\┬)V#\ã\▀@<¢k\Þ\rC▒┤At¢\Õ\─j\‗\\JA|\Ò\╚\¶«y6\╦M\"\§Á¼VQê\ÔE\╦u\'\╠·ıòSæûàjØª\Ê4yÆ\rF\ÛH$ÉoLBX\ıh╗u┘ù?\ÝP\¯Bº\¸Ê¬░\ý\ð\Ãq	èDBì\È*┴\Ù²ƒ6dÉªKv\ÕX~¡iùÂ=Üo \0\Z│\▀p) \0\Ú\'f\¯ótmsI*\Ìª\ÕzPéÄEsÌô{íc!Iú\õq\ßq\Þj\╠\÷\ð\÷Æ\Ï\Ùz	°}V·{q·\þ\¤ÑÈ┤\¶òe×\È+\Ú\‗?\Þ%î\´R>ÁÆ£Û¢ÖèG\Ë;í\▀M·Gq\ã)è\╩;ú\Èa2█åå\÷0\╠X\ÙÅ²\ÔÅ<zy\ıAØñò\0g\´Å\þ¹¿ª½¿Z^|6ík	▒\ı\\îÄ>cÃºÿ¬Àw_\ÕöI2K<jC\╚z°╝©\þ\ÝM1\Ð[\0╣I\¤£u\þ~┐║ÂÄÍèm\Ê\¯_y\ã\Ùg=z)\¶¤É¼k! àE`z*\±┐\¤ø?jôv]/Ø└\¶\±\ßƒ?J\┌ú<░RTt²[UM,[\¸Éù\´▄îyÅZ\├\ÛJ┌Ñ╔│òÑE\±ô£l\þáu·ÿ\È-\Ýô\ß\È\▄+w│\ãr=7â\õH\Ò$F┘╗╗êQäl\Ï1ç\0\Ò<é~Á\¤\‗rrÆï2ÃÅé=\ÌHàª&^¨Ä\Úú└#\╦óÆ\µÜY\ZUTòTm \Òë\\Ø:\ÓOë(»│h9ì╝ç¢×\╔bl┤▓M\0pJ£Ä}+ƒ,+úxÂC`░\├*Ä\Ú\Ð\õ9l\¯o»\¾¹Qåswg$k░╝jZ#Oƒ■Bà\█\█\\\\ÿÂÉ\¯#\'\Ó\ÓsÜ}Á\┬Ãâou┤\ßè&▄Æ<¨\§¼©Á▒äaO·³Â\¸rãì©w£d\Ò┘╝¬\ı\õü-ÿE+=ÿ\¾<2\¾└j\ÊO8IUd@\╦19\¾½░\╔lÆD;\÷]\┼\Ô\Þ8>gí¬î\┌aIø\█X\õé\┬\Ï\¤w$æå\Ï[$ÅZ▓▒£	\¶¼×Å½\\\'Ø\Í\'\´rpï\µ¡\ý%eDæqÀoæ┴\÷\Ô¢oìøÆ\┘┼ù1\ÓR\Ôö\n\\WEÿ£úW\ı.{å│Ø/,º#\¶q*\rÆ{åµ▓ÀRm^&,\õ\¯\§8\¸\þ¡n5Il/\\øØt,p£®█Ç\ýâ\þ\¶¼ì³▓\Ì^┴\Z\¸oÆ┴\╚\±0\ÚÜ\‗\‗╗=jè6ù/wwé|\ÛgÆ!l\ãP\╚\ÃNâx\¸¿Ö\"mN1$m\"¬Â\ÊF5z\┌(`î$dq©\‗>\§äÖ¿¹[█Ø>\ß\¯%X@$!I\‗	\'á\┬\±Åz&À_$Xª\Ï\¤X╣Ux=MPv1d]FgØH(┼ê\\¹·Å*£j{Õ│ÁH{┐a@*í:D┤m\Z\µ\┬C4À7\"\ÛN\Ð\õWáó■ƒ\ı ¢gäE%▄âTcÄ<êâƒztzä\Ë\Ï\▄Æ\┼\¯-%\█\ð·Åj\§¹\┌─û\‗Áñ0\\)\ã\Þ\Õx#<\­y½ïÂK\'\Ë\¯\¯o\╠v\¾D\Ê\Û.┼ÆFr|\╚=:VçAK{]Ri&é\õáX¡\█┼Éz\õ\¶«v~Öb┤╣{ï{ºbY«D`Â?c\Ï¹\Ð´Çâ\ÔEðàD╣\õ·¹²k│-Yä\ÕZ,c╩╝;▒\¾Üv)1â]	°0\Ý×=\Ì<#Üa\ã:S\¤Jk\ß\0eøîS▒\Ð╩┐n·Akr!Á î¨▒\═\ýe║Â▒p°\'\ß\ÔU\Ë_\ßI\█{ƒèÝÄó\█\¸¿tU\÷└┴O▒ý░╝©\ã\ZIÖ>íz\Z\õ}Øká\ý\Ë\╦²¡║üÂBsÃá¼\▄\┘Y»Ne\Ì\─}0ph²\╚?\Ë6YnWw?j\╬\╚\0\ÝÆ3b%F\´á\'²\¶ƒC]×ûE]\Ý\‗ñ2,¨«º¡4 ë└æ<1I┤ \¾ë°°Ëöxä├ú=ÂÌáâ\‗ôLP═▓=°vFéYôTT#QUClGm═ûÀæÅ\Û¨®■\Ó¡2Éº3║\Ý2éD\Ú³\Ù\█{\Õ®W×?ùªdO³à rQØ\ZH\┼\─J:o_ÿ╬èAE¹K½\┼s¿ë┐NÑ║æ·\Ò\‗/·\n\Ô=\╚\ýå.	py\┌\¦\µkK\ß3«\┘DH¼xQ|\├Ì¢àînp¡┤ÿ█ÅÖò8\÷$SRñ*32\÷^vR#£wè\¦\Ì\Ë¹~_ƒ4\Û\┬kwA*-\Î║║#0\¯\╚di?B\─rwÅò¥\Ò4\╔\"Ä\Õ]%) \´\nÿ0¨ç\▄R\ÕaF_▓Àkg¬$SE+ärƒæ»®\ý\È-¼\n9A\n\§·W╠À}Ø°y\Í<ñed1\þ,Svx\¸\‗»Ñtk¿5\r\Ê{V/æ*®\╠G\ÈTÂg{Têu©\¸óÉ ^XgÄy«®vû(Ásmmº\█]Y╩┤®\ZûV\§N╝\Í├À·\õ:F»\0x\Zgû\‗├×ò\¤Zeô]\╔┤Â\╔2ûJ\þpoq\ÕZ\‗TE\Zm1┤\Õq\§┤$U\"\ß\Ýü\Ò\ÏqD5;│\±\ÚÀ!m,îì	`V\¾¹V\'Q¥╝ÀÄ\Ð-.\┌.\‗4còe#ÉG\¾½SNaÁ\█n	Á\ã\0\ãI¿õèúÑ■D»°mÑú\"┤d7æ\¾\ZÑ°ìioçi\¦\─ø▒·<|³²hç\ß» \0wzXLç╗ìG°î╗╗?l ]\r\´\µúæRãÄQ 9\╦L\Ó6³u¬\▀\Í¹Tx<Ç¼23┤×ï\Ú\§ƒÑLW\µ@┴aÅ/\Ý/Ð╝¬6\ÃLûSç\Î\'\0■|\ÚTåD8\Ïs\Ë└{~ \00³®┐7?6~█À■╝²1R\ã%yG^╝Å¿==®º×º«rzÀƒÐ╝¢1M:bYë8s┴^p3\Ù\ÚÅ!\═ZÂW0┤üÏ¬Â(O_¿&½dÂIô\Ô%ù¤ª\´┐C\Ý\═x▓ºx\Ê*ùU \0?ùÀ5ûH\'Â&Vël°ìÀ\"äaÕÅù¥\§R\▄x&\´[)║)N\né+\Ð┌╝\├+I#\ãN\Ý\Þ\Ì,ƒR)ù²\¦\§\╠\ãy$╩│q££V	\ã[bóı¢\┬Z\├ÜVa&R\"G\¾½3\█Z\¸\naï╗\├*ÃÅ\Í\'»5Zë8íA#\Ý\Û[*\‗U -£3\ýû╬íN6ò\‗oj¿nBô(\¤`\Ði»<\Ó\0\ß@/\ß9¤ƒ¢RÀûX\ð╦©\¸ï\‗+úªhå╗%Èƒ\┘xá9|\¯>|¨îUt\Êd(ù38T\╬#Ps▒1\ð\È\Õ\ÃoAPè\Ê\█6)Â\'#a\Õ¢H \0\n)ójMevY\┘Xÿ└N\§¤à<\╚\¸á\‗\╬cE¦│F	\nm`¥X\§¤æ½ZZ┤2\▄\─$J$J╗è\Ò\È¨fú(▒\╩6Äækq¦»}ê\±\±Æ¢jb9\╬0Jºñ\┼4zU╣hR¦Â°B»O¡àU░$f¡î\¾^\Èe¨8e£1Ö5c$ol░ñ\þ2î\ß\╚\¶\÷¬\¸z}ãù¬w2[ÿ\þXâ£Ç╣\Ú[9\§¢OÈñ©BÀ\¸£ä\Ï\0¹`\­~ıò\ı/.\§Y«o╗éAIQ\Ã=Mp4¢Ø▒e%e}c╝X,.X¥▀ùòDû¥f\´│\ãr\ãQ\Î\ð\È`©╗\▄#0P¼åZoq\±*\­K┤\´o	\Ã#¨V|m\Z\Zk┼Às\0Xå~vRs\Õ\´E¹\Úcë>\È\\6\­©ëp¿>ó▓\ãHb\─*v\¤FN}▒D\¯\µ+ò╗\õc£©\Ò³jºB	\█\▀Ke9yæN\\\´R╗U}1\ÙWÁ\Ý«-\Í\Ô\¯>\‗&Åj▄æ\╔â\Ëò£▒û\µ\ÔmàÜ\Ó\þ\┬\Ë\Ûk_ó\─·ó\r:[\─°[Á\Ìbx\╬°\╚\ÒpÃòi\´D2\´\ß\÷¬%K¢/¥g\¯ÖLe█Æ©\‗BÀY*╗ë■^Á\═4I#\ðu½ì\÷+$äô\¦[6JF╝\¤5\ð\õû{ì5ªËè<»	ks/\╚[╚£yW|-DµÆÂI5\─\‗$s\¤O \╩+ÂoZ~Aü╩×ät5\─{S>íªÛèÀÆ\╬u\▄oÜ\õ░!\ÏtÏóû}│\Î\¶qùv&{rÇÖ\ÝÄ\þa\Û├á¹T²┤Úû▒j╬Á\╬q\µ:èk\╔¦âqÄ\┼\÷ø\Ð{oñ\Ùj¡®└\±xÂ\Ý>ÿ<Ü┐¡\Ì$=×¥║F¦Â\┘\È\‗	<¥\§k\"d©4p╗\Î3j\Îr3gt\Ê╔ƒ^N+y\┘x{«\═┘▒R\ZT\´S\\\Ó\´{w█ô&\─rFk¡EÂÁå\¦>X\ÐTVKf\§óÁ\Í│i×9³¿<½u¡jV\õéÈîÅ]\┘z·Fmb1¹!Å\¯¿tE\Ê\¸Rª:M\0>\ÓöÄn°\╦│*ü║@e\¾ÑÁÂÅR╝{+{é;└%Vû\"úz\‗\▄²+R\Ð	\ıá$d×H\¸®\¶²\Ìk	iºO$1Ö\'øi\▄7pyV/E®YîXm\ÌYvj┤▒\╚$Åp\ã\▀\")\ÔQÀ\Ãy0u_\"Q\¶¡¹\÷\╔fH\Ól\§$c5U┐ÑrA\Ï\È\┘Vb\Ò▒BV\¯Ä&d═ÇP\Ë~òç²b\­\\\´&r1\´£V\╠\÷p0░G¨\Èg░3\þ?ƒíúô1\ã\ã\ÒkG\Ì@3╠éAÆ\╦\‗Ü\¸\┬\\Öæ³>\µn\§x\ß³\¾\ýka\'`\¯;¡ø■V\Ï¹pâlv\Êrz│(░│.Ütð▓\Ý\Õ[(╗·ú|└ƒ®5╗\ý\r\ýû\¾]i.▒╩ê;\Ï{╣>L~»\▀Í¼i \0çÂöôSs,└\ÕV6<}k_mkihÅ\▄ClÅ\n\0Ofòà£\Ë\±;\Ù²=ÜKêg\Ï)`┤iYq\Ë8J\═A╚û\Û│\Û«╚¿Bâd\╩I<\õ\þ╬Ä\÷■\ß\ÓøEHÐâ\Zdí<\‗h%Èù)kvZG!g\\1\'íYI!\¸║<[H┌è░DQÄ\ÞîqO©▒┘º┤K¿█ÉÐÆ\r┴\╚²VvÄ[tìÄ6®a│9\Ô½\▄\▄^Áñ[@`└┴S\┬=4ûçúº\÷>▒Ü}º}¡eÜD9]╣>uô³C\ı\µ╝\ÃiÄ\Ô\Ì@\ý	¨°┴úz=\█┴\┘\╚R\‗\ÊZæx\ã\Ò\Ú@ot\╔¯òòút\Ó\ßÂ+Dd\Ì\╠fıòô╝\¯@²O#²\Õ¿\╠dàu\õÆ\ß[ü²û=\═i\ÎD┐éw\╩qÀ╗\Ò³Üi\Ë5b`Wîf3ƒ\‗(\õQù8\┌r╠ú$uƒq\‗\ËKy#,\Ã\├\õOÜ²?g\´Zª│\ı#B\ð╚ØþÉÆ C~\Û\¾C¿¿r%bQîy\‗)\‗%¢/\Ó6:â\ã\´┐╦Å,fø\ÌD╣─àp9└\¶\Ò?Q\ËòºK=YÄ%xYmM▒/\‗ºG\¤yn\ÔØb²	\Ï\Î\¤°fôù▒\╦y\╠P\═fÆB\0Hc=á■TGNüÑ\ÏZc	r	V\Ï>¥u4¬\ÙrQò$à▒.7A\¯)\ÎV\¾@\Ùr\ÓüØ¬\¤╩░¹~À¡s\Õè{ê\Z\'å\'\┘\"¼\"MíÎ¿8\Ù\¶4\Ër²\È\rTæ#l╚ç$o\ÝU7╣\´d\¯â&v╣A┴\Ò8\§%¼B[ô\"▓\"\­\\3¹c\¤\Ù\┼	I-\"t\Ã[°\¦ZR\±░\╔f\ÃC\ÚƒOjô·NAoRìÐév└▄Å\´}j\Ý¡ä\Úfð╣îG sCV(.oÖ\0ìì\¯º#\Þ\¾¿joaHÜ¨\ÐC+®!\Î,┐?j¢í\┼q&¡└%╦ƒ\rÅ#Cì\ÒH]mîø▓|æÚè┐ª\\\▀Er\Ï~øq\´h\¸¤ÁLKcùGH3ÖB\õ¿\r\╦cºö\µ\▄Gà░5N\Í\ÓKà}\ýáe└\Óƒ:Á©yg\´^\È\Zq\Ð\┼$\╬\­▒ÆYLf\ß¨(`g\╠z\nëî1³Mèmår\Û\╚Y\¤C\ÚT¼PDæ]í\Ì\0(|D░\ÛG¿\÷º	L	y+ú╦Öà\'ÄÖ\ã~┐òy³N·E[©\Z+ï\╚%x│░æyÃí¤ØQè\ÌIäØ▄ï	Q╩ƒ/zpÖ««ño\Ì>Bs\╬}|\Û\┼\ÈJ\Í\¾l\╠R^\Ý\Õ\╬2Où\ðP¼D\Ê\╦\­\Ý4j\Ï\0▒;s\¶\"î\┘\Ãyf&åG\´#*H\╦nl\þ╩│)o-│Ä\ý\ÕÅ\╩@\╔?z5gu▓h×\Ô7\ã\µ\ã\Ô@\Û®hH\ıi\¸-ª<ï\­\0\▄,y$(\╦/æa\Õ\÷ó║&Ñ«jWùChÂ²\Î*½\Ò\´\µ=²k1j\‗╔½D\Ð\╬¢\­G\Ì\r╗╚ƒBh\±\ý \0hÜamas┴R\Î\ã\┘}}¢½Lmñ)F\═\\z}ö71\ÛÎ¡ØÒ®îà\¸²»\\\ı\╚oó\¯\Í\▀Mà\¯jîz\Íw▒\÷ÀÀrA®<s³v▓³\¤\¯OºÁtÄ4P\nØ0ú¬\╩\╠°#¼\÷D]╝║\¦\ýQ¢\╩\ãq│\ßƒ?Z▓;G_ú█é$b0\Ã\▄\n\Þ·╝M>æq░R\╦\¾\Ý\╚j\Õv¼-³Â·£óöfàÐâ\Ò\'Ê▓ò│hQ_P\ýÌØv\▀\¦³=\Ã\Û\╦\Z\È\Û}ï\ıa\Ë^\┌d¥│ÖP┤a\÷╗c£\ÙY¢r\¯.ÐÂøêE\­\¦\Þr2*\¯û\Óæè\r\█é=6î\ÊV&æ\─m¹1ñ┴|\┬gÆ┘Ç\Ãu>Gï<\ÙZL\­\n\þ\Ë$q\÷½]¬\ÝCE\┌t\ý\§ÌØm;©GsÉ¼úqU@*p\¦k\\lû╗\┌5Vô~0ç{Tz:\Ý9ë¦┤\┼\µ1F\Zï\¸Ñ\´#R\"Éc\─F8ZÐÆY┤\▄F@\ÚZ\╬\╬Fë«ka.6üYkL|L`¨\Ã8\ÔÁ]£² ÍÄFNI\¸¼dTC╠ú9\ÙH:TÅ┴\þî\ËN9\õqSE}k\▄·ƒ╬¢╣I¨ù\¯i26\¯\╚\┼Gç)q×OZ\±`	<{SÇ\þ&Ç┤m*8¤Ø=üa\Ó\0▒S×:Îç╔╗\ã)yp\­£`fèƒ\÷ç▓zûÁ>ÿðòDÀE\▄í ÜK■\╠ÛÀÜl\÷¢ıá\Ì\0\þ \þÊ║;Nq\õpI└5\ý\þ$╝`¨P;9ì\þb\§Ö\¯\ßû7üV(\┬\Ò╝<æT░Ü\┬\┌E╣fùvw£\Ô║\­\n└\Ó/N:\ËTÄ\¾\‗\╚\‗óé└]ù▒©Ë¼ÇéTÀ\Ó95æA8 cè~\ð[pq\Û#y\‗\═æ2®¦ò_\╔\ÔÿbÅ\÷\‗®ÄZoQ£z\ð\rD\▄\Ã \0v¬_\Ú\Ð\¦├àè ┴âp©\õQ\╚8\Ó\¾\═5ö	:æ\÷áºh¡nlt)nÆ\¯*6└\0êÎ¼dMe\Ýt\¶É╣ÁÄF\n\¦w($\þ\Ûkó\÷\├p\ý\┼¤ç<t\┼s\¯\Í\\\¸z\ÍRVàÜ\Íö&GöØ  å9¡meí)jÇ┤lp\╠O\\zÜt\n.äf\ß\ßÖ|11\┌@\§\÷4ıÁÜn\¸t\¤[¿d*¨\Ò╠Àí\÷¬ou\"Dc╣\─&7\Õ\╔!²í\¯}+	¬\┌\0¼:|A\ZiØ{öô;\"\Þ8$¨Ux\¯!Ö\ÔGå(\├f(\├╗\÷ÿz¨`UN\÷Yúû;6xîç.\Ï└<tÁ>ÿù7p\\¬\┼$│¬ä*\├«)côA\─| \0}ñ\┌%o[\±Æ:\Ò\ÝQ\█\┼*#1Éë│ëí└\§>\\ıìKJ0E\ÙÿFzÑ\÷¼hz\Ó¨ÜÑ²╝I\"Z┐y\╬░\╚o\‗<\Þƒ5░èëÖúæ▓y­╗┐¢Í¡\Ãs47p┴\═\¾\Ò\╦ \03C\¶\▄^|<q*\─b1nòb\¯\¯h-\╩B│=È¢1|>x\¶¼\¯\ÏÐ»\ýÕÄáÆ\╦4Æ2\ãI8ô¨yVá9\n3\Ã\Èµ©ä\r¡Z\╩^\¦\´!n╣ØÂ\Ýoh\ý\╚Wê\▄\0:\╦$Î½èó╗0Æ\õbÁ║Á╗åh\þHú9Hù#ƒ\\\Ze¿.ûRô.\¦└ép\¤5\ZY.ûiAÉ\Óx3\ß³¬Kûx\§©░ê▓E\‗¿\┼sØ\¶UÄ;y«p\Òj\Òà3âN\ÈH─ªE\Òoeþ»òE\┘\õy-«\Ô\\ÉfêƒE4║øox\Òmé8A$\þÆ})1°\¸Z═òà%R\Ó\ãk\▀\­F½\­»¢Ag þèå)ì╠ü¹─î&1×3\Î\▄¨Q».ø$/oGwk.O\─(r	=H=E\"Q©\ýW\ß■í¿\¤²1¡Ö-\Òt\Ô<n¥©\‗\¾¡oj\§¨╗)5òÄøk\n\█\╦Ë©c\¤c»┤wG\Íc╣\Ê\Ý$h\þ~\‗%dc\├6z£·U\ð\Þ░véT╝Æya{H\╬q¤Øi\ÓLÏÉ½®\ÛN|²kn£àª╣fï\┌[^\¤^¦╝\­\═*Hà▓âÜ\ðvk\±N\Ý.í­ÂûùQæîÖT`P¹ìu\Ò1│`íKA\Ô\╬>ojþÜÉt\Ýñ\¦\╬\¯\µEQx\±\Í\¸RIdÐºéÖ[\n¼8\Ã5\ã\þ\Ýtw7	=Ñ\├\▄A9Àv\0\§\¤?jWE!┌à«{P\Ës¢\¶\þ\┌N6\ßqÜ\´¼\Ê&pN~_\¯è\ß0\÷×\╩\µuÀK9Åù|¬8_1\¸«\Ýl{\╦x\Ïp\n/\Þ)-\─KS\Ðtµ│û\Ú¼Ð«Â$aû\Ù\\\Ê\Û·;?äîú▒ÜMá\§\┼lÁ\╬\█\█\ÚÀ \0\ðsX▄ë\¯ú-æÇPî\ÓÆO5éı▓À\Zj\þ \0ê^J\Ë2.\═r\ð^┴ûB\¦|©¿uø╦ø-*y\Ýû<ã╣│M╣ \0jZƒ\Ý7\­ªvÅ\'@╝²JÂAbÁï²V¨\┼\Ù ìB0\´]3│\ÝÄ\Ð\Ùh\┘▒1Ä¢kô■î_\\\õæWW\ý\¸=½\Î3\Õ\Zc\¾¼Yh7®<▒iùOo+E*\─v╚â${\Î\Í¹U\█\¤|\n═¡\ÌèB\╩Li\╬>\ı\┘52WI║┴#\¶~U\╩³gñ>P░\¤Íæh	\Ôi\¯.\Ê½OçqåÏ×²¬\┼\þo¹[g0▀®\╠\├$)┌Ö#ºLV*╠ƒ\ÚN3·e\þËÜ.\Ë\┌A¡]5\┼\¤=\ß\┌\ÃÅ╬è\\~&vÖHQ} %3\ÔA]3\­\þ[È╗G┘ª¥\È\¯Æ|Kªv■¿©\‗é]ÅÌ╗w\ßà >2O\Õ@Ö░\È/\þ▓X{ï╣36Ë┤³âÍ»)*\ßçaØ¢qJ		J\÷N\Ý\┘\þÍéL_l\§¡sHi%\ËXw0\█	\ÕB╣\¦\Î\'┌░■,\ÙìäD	]├Äò\È¹H\Ú\┌\Þ Æt\┬C\\#│Üù^éÚú╣ÀÇ[F	¨\Ó{\Ê▓¢³M\Ý£\Ð+┤,»¥\0\§»\'\Ôwif▒{©bÁ\"└Sôèw\┘+ìR\Ê\Ì²/¼íD-▓3sÅá®f\Ê\ý\¶×\╬\═À)p¹ZM\Ð\þ\0·d\¾Tät\¯\╠\÷å\ÒT\ýM«Áw\ÚñB╠░ÄO\'è╣½\Û\Ê\┌v^¹Xé│Co\Ì$·\õuí┐ç º\ß\µô\Õö=>ª¡vÐê\ýnÂ\┘\╔°\\g\¯)P└~1k!Xø><\Û¦┐ÔÂ╗sawvûzxå¦é║Ø\┘l\Î,v▄åï\ÚmÄ\╠\Ù/┤Z£·tá¢°?5\ÙÖ\Ì┤\Ý7zG¢ân\ÓQ~\╔\÷¹W\Î¹Pt½½;ú\¯wçv\´\▀\\\þ│\¸\Î\ÎEÖ\\³1\±\¤Z?°|²#▒\Ã?jÆ\Ð,\Ú=┤ \0│7Xà^Ö\Ù\§¼\µ«tû¢Cz\¸2│àLi	*\ÐðüÍ┤Ø┤┴\ýØ\Ô\Ó|╝¹\ÍwX\Ý+i[┬░4Å\­æc^S\'B½\▀^\÷k\Ó{ë\ýh¬_╣`\▄z\±\╔·ðë\µ\ý\È\Í\Ð \0\Î\§db▄Â1\÷¬w²º\ı\´\Õ1H├║ì7 \\n\'\ÙB \0ñ´ªÀø║òíÇ░1öl6<┴¼\µÈƒE(\Z?S\ý¦ì\┌E²+y,ør½,j¾×ÿÑÂÍ┤ë\´.n\ı\▄I©│:!W\ÚX\÷©×\µI\Ó2\Õó!úu¨øÏÜÀ\'zÂÇú4s>Lï\õ{SX\Þ|M\┌\´ev:|C\╩JØ\ýa|■FàØsMòCF²\ÏQÇ^3ô\ÚËèé\▄╣,\µ>î£ƒ¢SÜTeWÄ@Lçr©╩┤Qõ®ôAE\Ýø->¨@L\ý█ì▀ò\Ê;}gf\Úp│àbxC©/ªOÑ`n.\õûG(,ggçí4°ûRQdÌú9óöpñ*gTƒ\±7L{\╚$Ä\═\ÒÀ\\\¸▒2î┐¢░6zåƒ\Ô┌¿Y\ð:é\Ó\¶«#cm\Ê┴,\Û»xá¹\Îa=¿\ýı¬,+®\█└©\┬\ãr6ü\ý+FÆ%»G1h\╩┴ôZ`G \0Já6Ül·ù~║\▄D×6╚╗x<uó®\ÏX%`¡¡\\|¥ \0à[Å\­\ÃMÉƒS┐ùh²Kc■┐%\ËGº\¦\ÞrjûªT!$ëâ»*\╩A\┌s\¶»^\Þù\¸JÅÅt\Ù\█É\Ó¨}\Ù[¿\÷r)Ü\ãı».¼ÓÀê3M2m\´\Ò\0ô\ÚGn5múÉ═¿╔ÁXã¢\ð9l,T±¥âúæ\▄i┌£/¥-&\÷0\╠Ç¨\0\§¿Ô░ƒSÖ\ÌYX½øcg=\¾«Ñ>¡\┘im\┌9»\§<%OZ\þ²úM6\Ê\‗	4BZ6╔ÆBî\ý¹¨\È\­Æúí\Ú¦┐©▓░Â▒■ü\´Q\Ìn@1\Ãa \0ÿD\╩\┌DJ\ýàN\Ù▒\þ\ãzW/}Z\§\Ò\┘▀ÉG\┼2).5	äf\ßK~\Ð®²!\Z\rBd×\r\┬hUîlñ\Ò$\¶º■╝\ZÁ\▀\ÛW6░!\┌\Î\╬qÍá┤\ý\¾╗\÷\‗\'a\‗é¢Á^âDa^H\├1\▄»}*V╗\Z:äØ┤\ý\õ\╔\¦.½\0^\±vÆv\¯┴\╬kîj,¦ª\ı$ä\┼q\¸&E}°Òææ\ÙDí\ýö\¾╗$W6\Ð\┘C&ÃÁÈ┤\█\═+R©\Ë\­À3ó \0bÉ©<î>=*~\█*ÅiÂ▒\█j=\ý\Ë└Ul×!ç\þw\Ô╗}»m¹7¡¬ \nèaEr3▓W║╝a\Ý«l\ÔæI>eÃÀ°\Ð!°o®É╗.¼ÂÅø \¾K\¯IPQ│\ı.╗¼^E~·ô¤¬EKp$\Ó\╬1YmUëùM,\0oê\ÓUx;¿\┘\Ì\├q±ÂÄ▒1|*\Ó\Ò┌Ø®╣i-8;ä\Ó\Îg\Ã|\Òh\╩e╦ÄuK@X■\Ûn¢■┴╗■\þ\¾®nòƒ\═:T\Z\Ð╬âwƒ■_\¾¡Z&╩┐ç\╦ \0_©>®«½\┘\§\Ãj\§\╬z─ç\¸\Î-\ý\¾Å\ý\┼]O@ \0Áz\▀¹¿ \0ìc#D\ıx\Ên \0\¦\Î4³Sï=×\ð\▀#à\Ë5_\÷M\Ð \0\Ú\Í\±&/c\¶ù(\▄<j\ß\µ\rIH\õV`¹^G2®·sO\È y\§ïÁU>\Û[½0\ÚÊÁ\È/\r\'vC9\╬v\§8\ÚE_D¢ûg£Y\\\╔\Ì.\Ó\╦£\­sM43<-$ìÈòV£l\ÓÅZ\Ýä=ç` \0\'\‗«As\┌àt█ñòp²\ß\¦\Ù\┼v/\┬ Ga▓³\█\õztª#wƒ	>öà\ÃxTr2\0>Á\r\ýØ\╠L├ª\Ë\═6ıïAk\µT	>Á▒Á\ð\ã\ËWPGïOa\╚\÷5┴╗;$Â\‗\\G#zØ\─5w\¦a;\ıÈô\╚\ÏÒÅ¢|\┘scg\█+pI\ãN\ÕLg`\Ðo\Ó\Ë;Ë▒\­¤Æns\¶t┤w\÷║¡ï^Eü\Z7:\Ý9\§ê\Î\Ýy¡\\\╔³2Àüê\ÙE┤ÿ.\╚\▄)}\Õ^E\╦|à8¢ëØ{\­\Ò\¶uñu\¤vI\'\¤\─j\▀lr¦ïÍÂì\─█É\▄P\´\├╦¿\Ô³;\Ê▓²y\§ì\\\Ý]\Èrv/Wh\█p\¯─£\Óåù-\ýh¨Ö\Ð\╠╬½Æ3ÄF0h¼\Ô\ýÌ▓í²*\▄Íáÿ\▀feû2g└\Ò>DTV\­ü\┘²Y╬╣\¸¼\Ò;có \0cƒ╝╣║\╬▄ïc\‗Åz\Ê~î~$2 \0·CY■ãñk=\µi\¯H╔¡/\ß· \0\÷û\ß\rohåt^\█.{!{â\╬\╩┴v\┬\▄\\\Ì\┘mS¢¡!]╣\Ã\Ûs]Â*[▓wú!9£ı¼\r─░┤hAe\\\¶Q.èÅg)╣ÁÄ╝!#║A╝û\ã1\´U-\'àñh\─CØ\ß\±À²»AZï\Ý\ý´ô╣W┘ä\§%¤¿\¶¼\¦\ý\ÔÀè2¥3\Û}ÅÁ8\Îl\ıI┤\÷1\▄GÀybüú\'×Á\Þ\‗\█Ò©©Ö%oxú_jKO·ðÂÁôr!║á\þ\╚\┼╗╝qyq\═ê°W¥Sé|¨uÜøöÜB╗.\¸@wê\╚#$ñénI\Ò®?½@\═\Ë	#ÁG\ß	*z\Ó²(¦┤│MÑ©96\ÕVaç#\÷¢hCñm\╦\¦ Øp\Ï\'\├\¤\´¡\█H\Z\Ì\ı¹Édà60\╚\▄1Å®\§óû0└\Ð\ãËòì┴¬[\õU+\¯ıíDR\ýp\╔\╬\Ì>┤\╔\┬\┼,Qê\¦\ßU\╔$\ßÇ\¶r\Þ\Ë┌ïi\'òíoí\­ƒz®\\®╗Ádaì¼¤×z\Ë\ý\§Äs\\└\╚B®Ãÿ\÷¼Á\╚x\µ\Ï┼çvx¹\Èr│*ú¥E¡]BÉ\╚\┌NªRL¬Â\╚\¾╗\ðsH¡┤█À┤©°\Ï.m\Ï	Qú¨\Z\╚Ãá\Û©/®│▓7üâz\¶\"ìMí\┌\Ì\▄;\Ã/─┤░Ç\Ë\╔.Y£·\Í*5I▓\þj{gº\Û\÷Â▒Ñ\▀}\ZH7\┼që▓É\Û(║àë\Ð\Ý-\Ì┴Zb\╠\¦\Û°H¤ò	È╗?wÑk\­\┼e\┼\╦\ý¨\¤8Ð¢o│ùû\¸i*╚ìv¼\┼ \0T·b║qI4e4\ý[Ä\█Q\§y\r$\÷ûùqÑ│RSÌÀ³Tø]ò╝x\‗wëÀ\'y\ð\ÎE#;`\═KD[He©[êò#_\ÐãñÆ~╣á░L~\rjñæn	\Ì[$`¹\Íz\¸H×dT\╠~ıö\ÓUù\Ý5y¡ÿ;ôÑ\Z┤\Í\Ï6─îºqc\µ=~\§îG	\╬<¬\╠S\╬TÉk×X\ËgAâQà¡\õöIÉòå1ƒJ╗n\ÛfT»Ì»äôû\╬\ß¥Q#3¿fu\0\õq\ÃJ7c½ñ1▒ÿ\r\µ\"\Ðm\Óû\ÎÊ░û/E);+êßªÄ\ıVCßöôé\Ã\ÙWãú/vQm\ÐN²íZCÆ+¼.\õC&<ç ô\´E ©@\Ê<×%\├)9\Ï=O»5ïç▓╣.uÆUë\Ý2A\n\┬F\Ô│\Ê^Eu$1\Ã)-½┐*@\ÙE-n]«\╦\╠Læà\┬\╚6¹ÜÀ4ó\Ô8$rúcnFÐƒ2Gÿ«î?#\ÛTDòò ôy×&brA\╚³\Û\r\\\Ý\ý²\÷N\┬ƒ.k\Ê\ÞÍæ^®å{ÿ┴*\ã\▄1·yU-KM\ÍL¢╚ÆøFLHämd··\±]q¨Æ\'ï%³9ù~Ñ7 \Õa<}\ru\═+\┌\¦k³© \0ìrN\═*h:ï\\Ä\Ú\ÕD&#╝?XzZ\Þ:j\¶├½jÜ×\╦\╬\µfXS\¶;çÀº¢KöYi\Z²L \0ý╗á~UCÜ\─~ ┐\Þ \0Oò\‗bÄpy\═╝\ÝFø=î\­╝\┘0(\─@r+/yiº\ÛZb\Ï]^\ÛW6ææ\Ó\¯:qP\┌\÷ZFc│\÷\ð[iW\ÊCô\±\Ã 7A\§¾«úá\╚\Ù┘¡? Q¼t×\Ð\Ó\╠V\▀\Ê\ßBm*ÉÉ©>u~+Då4èÁ\­¿0¬î@4\▀C)v\╦`\Î\¯┘ü\¯8\‗óä╚ƒ\ÞKêÏ¿7▓`z\ı)\¶+╗×\÷\µ\ÃZ║v]Ñ\ÕcE;:æ\÷OI}?O\ðuYáYîÖùÆ¼}²*ËÑ▓YáÍº\¯┤çbTéc¿\þƒ\¦T\Ý\§\█/àé\÷.\‗HTlÉG╚Å\Ë>ıÜ\Ýhñ[Ä\ÒK║ì\╦>\Ò\¾dc\È²Zæ4\Ãa\┘Ùçà\Ò	 F\ÓÇ8?Z\╬\÷ZT}r\╩\±oèî¼\"2ÇG¿5├»┤\Ì\µ\ÔEyåN\µON+úO«[\\\ã\Ë]\ÞÎêÉ¬Cf\\g®\¶v\╩\ÞÀp\Ã*\┌d\­áÂx\ÙTÄ=®[|DÂ\´Ì¬ün½Æj\─,-{7<Ftsù`7Å¡u\Ò\┘\r!ú\ýv7\¯┤*\þ\­¹C\È\┘(\ÍH \0Trá¨¹\ıãô\┘■«\═\█\¦\Ì\Þ\Zd\▄\╦\╩├üû\Ó■uáË»D¢É\Í\ý░MÊâ\╚:\r\Õ\§»h²ÅÀ\Ð\┘\r¢¹▓\"àX▀«\═<hQ\Þ½{rù¿\═r\Õ\┘|\╔\Ò\§*\Ý0=óÀÖ4\█M\÷®bYöú\þ ºÿá)l═º▀ó0\n\ý\né:`■·\Ì\ÙÎ▒\▄ºOmæB╚á»êØ\Ìx¡E«ìº-ñ1ÁÑ»x\"╦ôJ\ã*ÿ\÷³ï│æ╝s\¦4ôDKG\ÃPMh;\0T■&░¡▒└¤ØtD\Ë4┴░\╔kjX\╚#\Î┌ñé\ã\┬\╬\õ\\\┌┴jùp$\¯\­q]ÆK▓Zw\Ð?kô¢\ý\┼\Þ]└ÿ\╬1\È\ðf╝░ïS\Ë\Ý\¯n\Ô25╝Hm│å#`5k┤Rjzåë-¡Ñ═║H\õ.\Ô╣\Ò<■\Û½\Ïi\ÐAe{q\Z¤¿!X\├┴î\ßq┴·T╣┼ÄÜ\ýÀ{¿vedı®ò	R7É[Ïƒ#\\Å^\ý³\¸úFü>&<\r▓G\"£\‗9\ÓQ\¦\ý\ß║Èû\´I+q#öVâq\0y\§\Ûhn┤\┌}èû\Ô\ËGkk\±å[ônca\Ô=5.I\¶\╩M-o│\┌[Kó\▄i┴#╣ë\─W▓m)\╦\ýk%º\Þ\¸ÜN│4Ü×ùíe;ÿ!Lâ¢ø\Õeƒ/:▒5\╠Pk\Zwy\Ããû$	ì¨~2\─Q-\n\ßn;Cb\Ð\Ú\µ=Â\‗\╬Ëÿ\÷ò+îJ1ká┤]\È`\Ë-l%©©ýñûÉ\Ã\▄\┘A#ØsÖ╗1zëqx\┌|æ[Jæ\ÌE,A#\0²+«v▓;ØW▒ÀOl¬$@hº;\╬}d-cK¹+UÂÁi$æ\╠Q\Ýt+éy·îVÆrU@ƒ▓¬v¡í{╣ì\ÔmPY╗Á#Ä┤2\¾@╝¢\Ë\┌\÷(\Ï[ä2wÄTú»«Áz\¾\Ûz]\¶däeísì\═³\Ùƒ\┌wkíAb\ãı»\‗▒\═L▓\Õê<¹\n%9/\0ÿû}ï×KU╣×\╦f\ÞÀ.@W»¾¼îØ£ÂØ\ã%\ã\¸\'\'\█V╣┤û\╩[àè8ô`a\È\ßp9²ı£ð¼ºÀ\Ë6^,A─»Ç#\Ôú.G¡îÁ\Ë\§\╦[\╔S·>\ÌX\\\╠<$yUÂMV\┌\█t:t\¤\¤ÖF\Ìz\µ¡\ý8\▄\õ°A■ÎÖ¿\µ,fW`l¨ \0ÿú\ÛïesÆ@╗ö╗║È┤\§ÂAIpó\ÞAeA\Þ}j\ÎiñTd² Ü\┘8\┼\´h\Ìr\▄4ï\Ý\¤C\\ìñ\ın\n╣N\Ì@¡úî¨9vbºñ;J½\ÈRº	É\╩\─·Üû\Û▄¼à\╦+\ÈM\"ƒ	AÃÖ5\Îf╚ì│u2¿5D^3©\­ƒ*ƒt#\µE \0╝ib#êø\¾8Ïô3WÜc└\┼\ÐWgÖ\▄*Ü▓î\ßåEi«\"V]ª#â\ÙBot¦á2Fc(Q1$u\═Iñ8\╔└<6<àSíòü®úøîc\´X▒áÜ^\╔¦®|3í\­▒\ÛGí½½®	«RI$`æâ·╝q\§á┴ÎéNzRáSsc\─*Sñ▓┐û\ÔVC\"¼\╠┴\¤o!\¸úë¿ø[û▓×<\Þ#ò\§¤»Ê╣°wåFw bzâ\ËV-Q\Ì\═\ÝnP.\╠l©_\ı_z\╩X\Ë6ë{o{ÂHcö.Aq\õáuu½┴\¦╚¿nÂmVd+\õsÄI\Ù\¶¼WM╬æ3<C\┘8nx\ÙZ;=d*<¿Ö#ö3:|\ÐÃï<Vr\ÃCL.g©▓┐¦òÄ0×&#░\‗?_J©¢ó\È-n(.^;X\ÏoæòIo\┌R1\ÚÊ│»t\¸ƒ\╔.avvær}HÁn\¦LVyh\┌i\þ\nPÉ\Ã8\¶>Á/\┘Iú^¢¡×\¤O┴\╬\Ów\ã\þi┌¥åå×\┘\ÙØ¹Lù*ì \─p▓\0³·\ð4©dè\µ;Éæé└KÀ┼Á¢\ÝRFYë- └<àR\¦\┘¨v¹T\± \0ú│Kºv\╩³\╔s\Ãq│+╗åÉ \0!\´CÁ\▄vÜ}N8┤h`kY$»z3\§<yö \╔\ð;¬4\´oîz\È\µòmïbUiÿ`╝\Ô\Ú \0Ñ	liØDïäSjûOrÉ\Õ\ã	\þ¤Çhu\▀i\¶\Ù65├ÖX\õñaê¤Ö¤É«{xùycm¿Ch|ÆÇ\Ùâ\µG¢UKIúÖñ7j┴\╬\µ\±Å\Ý\Ë>\È\▄\r-5AN\Èv×\¤RXí┤ÆFA!\Ì\╠\Ï\¤1Y\█\Ì\ËÂÀË¡\ZcÉI\Ìy#»┌èE]\┘■Æ■\Ê\'\0$+ì\Ó¨\Ò\ð\né\├L\Ê\¯\\\├urØ\█eú0\þÿ\Þ\riÄ7 =ºioªÖbÆVuR¥PºÑo¡;sn░2fW\ãa\▄p¿á▓vkN1wÂ:äp╔£qÿÀÅ\"z\þ\ÚQ\▄ÞÜàÑá┐\╠O\0^V\¾┴\‗5│\├╦óûâú\Ý=Ì®ó,:|-j¤é\‗\¤j>┤\'MôÁû¤ø+W╣ÏüP\╦8N×X=k3\Ùu¿©Â╗\Ïn█╝àî¹\þDt¨\µfOì\Í\r┤Düèw°|\Ú;çï3qwH\Ê«\Î\ÚÆ5Ì½¬\┌└æ`¹ë\¸(m\╠²²╠ô\▄Ksp▓Ìºy\─gb×║]ª»\╠═«\╔$▓å┴8\r\¸<UHj\ZóÉ]\█\¯ìhòFU©8\╬:·VSï^(\╔ñ\ı5mDIÄ\"\‗\Ìc\╦ \¾\┼i\Ú9íîãÂ J▒àu]Ì©\Ùè\══»ÿ\0â·9ÑgUmì\'\Ú£ySbÅI¥ø-ñ\¦\█K)+$èü\Ã8ñ1r[4î-vk┤×\█\ÛùA\¾\­▓\¯│░Ç~â╩┤pvó\¤\Óiíædqòd9SX9╗;ø=ÂMqæ`((G|©¨~Á^\¦\§ƒà[a┬│8\─bÁq\õ2:\Ð³░{░öx«\═\Ùv\¤JéC8\ÃCÉj¡\Îkt\þÆ┼òe)KÀü┤è═øÄ\ð\▄\¸Htl╔£XÂå\╠Èó\├Z╣╗U■ï£+u`âCT¥>8ô6╗,h¦ú│\Ë\Ê\±¹╣d\▄7®X\¾\ð\Z½┌Øb\ËP\ý¬\ÏC\▀)l«\Zî\§\¾ó\÷}øûLïØ&\ÕYh>Áb^\ã┴2\­7l+`\╩x┴\ÓS·\±·\"QÆ\‗ü\´-Ø\ı■×\'À\´\▀Ge#tÖtR\È\┘[└cè\Ì0\╔d¹╩ô\õ:T3vJ\┌\ÍIm.╝JW·\Ò\§\§»E\┘\ÙxVFÄ)▓╔░®É\þªóà\─\╬iù2Zi\0\╔)ÜX_{1<0\'\ÝZ\¸Ü\ÕGXú/ÀîÂ\▄g\¾¬│░└\Ð:A&\"_\Ûz\È\þJøbÇ\┬}|│Ü45>C┤zB©\"E¥UprFz\¾ZgØLq╗G\"pÑ\╩\¾┴\═@\┌TÆn\─D\r\µE$\¶5!\Êf¹;3Ö¨C\Ïq╬Æ>╗s\",ËçÕÅÜ\Ò°U¡@\Û\´c\Zm\╩\┬\ð7îÀ<ö@\Ï0╗#╗R\Ú\ã)\µ\═\ð7ê\¾»┌ô£láâØ¿┴NCë■4#┤S<\Zdí\╬DK²íÈÜ*|%è.\õëx\þ╬▓¦úÿ5\╠²\µZ(\¸\Ó\­	\'ºÍ║qñ\õe\'Q\n\÷&X\¯f┴\╬\­╣á\÷ªj1\╦6Ñt\Ò`ìñ$yú=Åë¡┤&-\═#oÑgnfG©°Oby&«\õ(\¶g\¯mUØ░A┴¨│î²¬Â\ÈNƒ░\═[╝Öÿ▒XbROº5E\ZV$\rÖ·V\Ð\Þ\╬B4p7Y£\╔QòàA\├!?Zö\╚\╦\¾*ƒÑFf\±\­/¡Pè\Ý©█â\ÚâKìf8\╚\Ô»;\r¬~ ?Qj│\¸lIr\õ}*Z\Z\¦Z[Ø█▓t#╬â¥b\ÒfÁ,b\╔B;\┼\‗ b®OªÖÀ4Kæ\õ1Y\╩6è\─└«\´:ÿ<\È2C%╝\┼]XÑyq\┼b\ı`▄î~\ıKk.\Ï\¯îÅ,ÄòWw\\\þ\Ý^èB«y#®b2~ 8?ñ\¾\ÙÅZ╗gx\±çV¨d\Õ\Ò²S\÷í\­æàñí\ÃÍ£\¯▓┴\┼`┤\È\Ô┤cäÿeR¢\ÞñOP=*Ý×í%ºy\n3ê\▀=┴æ░$·²k:\Ë\rñ6\¯p0<│\þRÁ\├In\±ÖYc\╬»ù¢f\±\Ï\ZÑ┐G┐q%│\"\¸k╝FÿÀ»¢_©kI«Yey°ıÂÇ<┴ï▒\È&KÇ\ËbHB\╔\Þ\╩=}Þîù+ó\Ú░cô,é6\┬m\‗R=}\Ù7ê,\È\▄\█╠ùki\┼\­ª1#\╚ÄG5t¼	4\ðGî\┼$£½â\Îç5£Æpƒó│3n\\ëU▓└îpA½vÜ\┼\─Zì┤\╦hTà\¯╩ô╗x\ÃS³k7\n)0╠ÂMyi$æ®Üxv©|\╦z\ÚW$qoª4i®ou¦¼l¼w98zg½b[\┼]\─\Ý$K	\ÛAÚÀ«*\È·ı¡\Ý\├#\┼<ù1Ãâ$\ÝÄ\Ý┐jù\Ãeö╣æ\┘-n\¯îÿIZW\¤CBƒñG³kA¬wM1Ç═┐Æ\├8¹`tí\ð\Û®pM\▀²f1 ║U¤î\±\╔\¾¿t\╚£║J\¦Ûâ©\╚~\ı5¨&l4[Ä¤óMdû¢ \0\§Æ\─s└\'«hÆj:Vƒ6░\█wî\Ã-m╣ÄG\­¼LwIh▓Mg3$Ñ\Ò *ÀP=My«╗å7V\µTò╩¿rú\▄\ð\Õ?+ô7Í║Fü1{æ\┘\Ùh\Ïåx\ãI\¾\═[[\n¹Á\Ð\ý\ÒHc\╬\ÈO#XX\§ØN(\Òè▒;JNrÑ\Ï¿\¶®\Ý¹E}*â{\╦\─\¦\õ┴	UC\¾.ñìíçJå\"\┬\´b/t1Ä©>\ÌubYmû8û[K`º\0ddÄ>ıÉ \0Hm~\Zn\Ú×Pæmö¿\­î¨\Ò\╦=*¢\¤hcôHÀÁèEÀ╣ \±╝j¬:r<\Û│{F\╬Y#îì░[ªx\­á9&ò\'Tu\┘hB\±\ß\ã>┤\þPê\Ú\Í\´=\õSJø|H8\¦\ÙC \0\Êdû\Ý-£┼▓ltI\þ\È \0\n\═K#\Zæ½çP╣h¹\├,N─£ò\\\03Ok\Ù\┼\╔\´8\Ó?¥▒wz\┬█¼ûÅx\╚Ø\õbjâì¡ÜÄ^\ı\Ã©fö\Ò!ò\õ9I3\È`U(\õF\Ê\ÒP║èk\­7zs\ÕT┐ºLhó\µY\Ýñ û.\ð>ƒ\ÒXï\═~k)S\±╝p\‗®FwÌƒo»\\\Û\÷\ã·0ûèàJ¿\╔\Ù×G¡.TmñÈñ°E╣[ÖÑê»Tlîz\ÈCT\´d1\├q#Nÿc\¯H·\ÍDÙü¥,[ã│JÅÂ<\Î\Ú\ÝUf\ÝM\╩┘ç└\█\ÌF\Òscô¤ûzSX\Õ\ý×h┘øÞÑÁ7.\¯P1\Ò<â£b£▓,\┼3\õî³©\'\ÈCXÈÑÜ[ºà\ãBçfa\Ô\þ\╚*ûn\Ê╚û\Û!/\ZÄX\Õ╝\═K\÷>f\ßf╝òpÅÀY²£\ı;ë\õKøi\ÒæbBJ\▄$Å\È{ìc`\Íuç\ÛX\ã\ã\´\r║®\r9\‗aPK¼j=³n\Ê!│ö¦û\þÌ½\Þbs6û║É©Ø\ÝgÆ`▀ó\Ïs╣h·T\Î7ï%óX\Õæ[r¥╝z\Í;Axb0\┼:äî│4d\0\Þ\Þí»-²▄ùø\ÕÜ-Ëìá\╬3└,(·\╠┘¥ªû\±\\Â%¤îqô└Ø\Ãz\ð\¸\Ï\´R3Ìä9PkÊ╝¾Ñ╣Öè┼Ø\Ûç\Ë \0>i½4Ç=╔ûEè4\"R╣;■é½\Ú░\µtë\¯n{╗exì	ièû>TtMCV┐×\ßm\ÒT\ÛIF²3[o\Í_\´7\­¼eƒ¹CR \0å \0¹\Îg\Ã\╚\█ÉÐÂ▓G┤\Ë`æ\¾ÄØ<½g3@xOÖ¡\╚ \0bº³8■â²F¹\Í\¸lò\ð\‗1¢ëlUeä\¯\Ãy×0M\Èz\ð«½²\Û\ÞÅFmà\ã\¸ai\┬x╩æ▒6²*óR>ö\±Ud\Ð\'#8\┌\¶ór\▄ÒƒÁI\╬╗R/_¨M0(e┴\▄3ƒM┤´î©NXmD\n ┐%P╝²JÆæR\Û7╗î\¯æJ\þ>åé\═üÄ(£\▀#xUkÅ\Û\Ù)\─\n╣\­\þ\"É\0A\§\‗¿\Õ_¢OC²\ÐX\ð\ýÉ1ò\nÀU\ÔwF¨\┼,_╬¢\Ú\¶?ãô\§*\Ð\Õ║\ÓèX\õ└\­\0N\▄s¹\Û4\‗·èdÈƒ\¯ \0:\0▒ø\‗▒\­»Ø8GÿeSöÅàVÅ²bO¯è×O\Ûù\ÙHEü{u╝pñäÿ£ÉW½g\╚\ı\§Á╣ûB\÷▓Q\Ów\0WÎ¡┤¨\ß \0~ƒãëº·─ƒ\¯ƒ \0®íÄË»═╝_óÿûƒ\Ù\§ójwzà\ı\█O4r;/àùc\Ý\´@4╬½²\ßSG■©┐\´MCC\n³TCN×Hª*c\05▓/\Þ\╔■¤×G_JôH\ÎE\█,\▄öíRONheç·¼ \0\▄o\Ôhù\§ô²)qL\r║\Û▓]│\┼5¬\╔#nc\'\÷¢\ÛY9m■\Zª++åû0Oüj│\÷]W¹\ÝDt/\§\Ù \0\¸?╬íãça®iÜ║\═a3\═\Z BH\0\¯ÃÀ\þÜeî\¾j¨°×\Õ«Up¥³SW²ÿ┐Zú·\ÝÀÊñ,K;K▀à╗e╝1~¼█ç,:p?ãºÅV°uÂüx\õ^fæF\ÓG\ÃJ¢¹W \0t?àf$ \0SÂ \0x\▀°®\┼\Ï\Zï─│Ü\ı~\Z\§░a\▄└Ç©oJmk5Å\├\ÌNm\Ý▒Im\Ï\Ò\ðy\ı; \0\Ùª·¡Mó\¶O¨®Á½%a	\ÊH×iÜMèA\§³\ÞcGk9ÁäÏºc@\▄s\´D\¶┐\Ù\" \0è┬äk\Ù\¸\▀\´ì8ê%\Z\¦Æ,░®\╦\¯!x\ÚÅ:»gv&\Ë└ÜR6\Ý¦Ä}³\Û\¶ \0\ý┴ \0\nk2ƒ\ÝK¨h[\0■úmomyh\'gÂòó\▄\ÓÆ¤©¬\´3Z▓]┌ê╩│òÆUuþÜ┐°ï■╣\¸äƒ\§;\´¨)ñ2\├]ó┘┤▒N\¦¹x&\\m\¤\¸¿\'ê\Ì╠æ\ã\Ù·1\Òê\‗?OjÑ \0\┬\╦²\§■4\±²dƒ\´\ZÆ<7\ð\Ãu\n4Æ┴o\n2\¸Ååu>ƒJs¼O]o\ßÀ2t\Ò\Î┐»?\▀4@³Æ╣■uLÂËëéáh\µ21/\"ÇÃÆ\¾\Ë\╦\µUød	ò`└xÂ\þ\─I·P½?Æ_\´è1¹6_°w■P╩░\╚\Z\ÝÑéR╗┴jèApZ\Í\┌DQ+££\ßôúÌé\Ï³\± \0v?³B¼] \0\┌\ÔãöéÅ \┘','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',128),(27,'58E2026B4F711CCE41934C2FC1C68850',100,_binary ' \Ï \Ó\0JFIF\0\0\0\0\0\0 \█\0C\0		\n !%0)!#-$*9*-13666 (;?:4>0563 \█\0C			3\"\"33333333333333333333333333333333333333333333333333 └\0\0▓É\"\0 \─\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0 \─\0N\0\n\0\0!1\"AQaqü2Bæ#Rí▒┴3ré\Ð$4CSbs\ß\­&\'6DcÆ▓\±%7Vdt│\Ê \─\0\Z\0\0\0\0\0\0\0\0\0\0\0\0 \─\0(\0\0\0\0\0\0\0!1A\"Qq2üBaí \┌\0\0\0?\0XÑ¡`JPZ\ýi\Ú▒Bû┤òÁ®4Qó\õSü)ñlS¬\§6R&\÷Vlº\Í\±H8\ð▄╝\nq|_zû\0\¶ºa╝║Räo╣Eh\┬jRGè_v1╚í║åHâ\¦\Í\ÎrÈ▓æÍèJüCB@x#ÜRóÀ=+x\n~\Ý(`í\'îP\ýc]\Ïè\ðOZï¬jûÜ=ï\▄]LæØ¥lé\Ý\õ¤ª}\Û¡o\÷£^i¯½îûÀmÏóáÞ╣öZIUáÂ¦½\Ê/\nä╣01³«\µ8²\¶^\'IFctöz\ã┴┐à6\ã\r\Ôd^*1Zÿ\Ó«x\ÒðÄö\Í\╠\¶µé┤\n▓1S\ÕXë\õTæÑ,*ÄáPrDï│Äòÿ\­T¡èzHx	SK©4A+I\'TÂÀ uª<\‗yª▄Ç\Ô6ç9º{Æ┬ô\¦m<sOF\ÕzÄ(9!â\Ì);*CÃÑhlAÑR\nCM┤x\═Ki1#â£SFn┴$êàqMæN▒$\§ñURdÜCfôèwmhè6\n,²\┌\╠b┤zQáXÐ¡R\±Z\┼2ìÜA\ÚáF8>F¿║è\§Ì╣\┬\▄\┼·Áen┴\█ \0¢WhZ;\'Âm\Êc┬┐¼\­\Ó×8½M\§┤vÜt│][#\¤+0$îÇ<▒Uì?│\‗]\▄╚ôLÉ,	\Ìc8cÄW8\ÃÆoÐÆvV\┌4´»Ø%ÜRv$N1Îâ²\¶WP\´─«\Zxí×hï\­Æ<\Î°{UNK\¦B\┼v\\M▒\┘1Á\0\╬:\­Gƒ\´\÷®Z\0[ëò«Ñ█áfeöûGS\õ[\╠¹QR¹ªH²\¸ê\¤	}\Úâ2\0;▓:açS\‗\‗\┼U\§]=\ÝgwÆ7\╔r;\ãlõÄ╣\¸«Ñe=¡═╝▓ãêcêaA]╣\¤\█Uû\Úf┐¥Y└\npc°ÿ\╚\õºxÔîÆfïh\Þaib?zwaUøRkn)Áê\¯\═(DM(>\ÚÔû╗ç¢┬ô\" Rû3\´N«s\╚\ÔØ\\zT\█(É┌âNáèZ¿\¶º;«2¢jmöHl┼â\ÝJU\'Ño\Ã\▀®q¬:\õºÌàà\"2ûNqN¼Çî\ZÉ-ït\0\Ê\r▒èøh4#╗V\ÞE\'cÄáR\╠z58©\▄╦ÅZî0#\'Øºo¿4┤íÁðçtPM~\├1└Ov>T?┤ØÁ[^\÷\╦Fh\ÌU\Ô[\┬|\÷}Z╣\Ï\´\§	Ø#f}\þtôHr_\µi\Ô\Ù▒hQÈ»5[\Ë-─¢\§\╦pº\­á\¶╩òmiå\╔\¯\Õ8g\█&C\ÛGJv(cÇ\ßP\┘¹\Êy\È╚┤\ÐrÑeòÅ9¼Ñ╔Ü\ÓC\Úq\¦\Ã\▀ZL╝\¶\├dÑB)qº¥\Ý▓\ã\ÍD\Õq\¶Qï=\r,K\ã\¯╝\þ \0zÜ\­▒\\Hí■b«▓}Æjü\÷]º\Í-\Ð{ø■²1\Êu\▄\0·`Ðïn█¥v\Ìiñ \0Àn\┘\¤\÷\ÒAn4ÿñ&D\╠r\§8\nH.m\Ùbis\╬\Õ\Ã\¸ËÑv-\╔r_¡{Yó▄Ç>/©c\ã\┘ð®\═▄¿9Q\È¨½Ü\õ\Ú\"JàQ┴ê\╚¨èq¡ÏÿÑxXu¯ÿº>])^Ø>ÿVVÄº╝ªF?uk┐\┌:\¶¬┐hÁï@«#©OI\Èq\§\Ð;~\Ï/\"\‗\┬A\¤Ìüü¨O?Jø┴C,ÂZîí║\Êw(³9¿║■ïrB|bD\þ\­\═\Ó\¤\þEp╗!FC\µbñ\ÔÎóë\Ï\╚u┴\­\Ë!\'\0\nö\╚p|=?}!\Ô=\¤ZD\ı\Ë\¶D\´Øx┌┤\Ë\¤\'¿ºÑs\╬~U¥UTóJVä3ôMû9Ñ\Z\Í\▀:óHøl└sYüXè\Ìp(ñxñJ<Íêªñki4úIªHFe$Ü\┘4éiÆq¼¹Sr┤ï\ÞôtÖ└\¶\Õ`#û\±èjúX%\Ô[Á║vE\╔m─î¹¨bó5²└░û\ÃPæ\ÊH┴Y\­Gá®\¤\¯úW\Ù;\█\╚\ÐLèrÆ/à▒\ÈU]\¯â\┌\¸D\Ã\▀\"░h%]é<\¶<îsSnî12Z]\█\═g\0\─T\¯Àÿ?Ø;ÿaèníà\Ý\╚%V\ÌM\╩Nx,<¥c\¤;\Ôæ À&5Æ\ÝÈ¼r╝\Ï\Ú\Ëª?ì	é·\µSwr`â░\0\╬>┐JKï²R\Õ¼\Ì;©\┌;}╗epÖ#\╦ÍâÂ│6ívMrYàV#.Tu\‗\§\Z\Ô\÷1n%r»r═╣9┬Äÿ²\─\ÒÌäH¹dfF*┴\┌qƒOÑ ¿×ôDÄR\ß6\ã*\Ï=\ÙM░Eù║+\Ò█©yUMu\ÙØ7I║Ä)U«¡\¯Æ6\╔\▀\¸Å└\Ópq\È\Ð\═Q{ï\Ù\µ╣u\─1ª\¦\┘\¤«}\ý\ÙQ\'\ýRNJPjtôrN2@\Û¢k\Ó┴\╚Ìè╚ü░ê\næ[Ø:ÍÑOØ █Àí³®ÈÉ¡4)1\ÙO+¬y\Èný»Ø;L\¶z2lxJ¡\¸öcÍñF¬GèÄ-ƒ:v$x\╔\ÕIhd\Ã\÷É<â[\ÕN\Î\Û|\Ú─ï¢P=\¾ÍåkZ¦Ågm;\Ù\¸|▒²\\(7;¹KA$]O\r¡╝ù¼P\ã2\¯\þ\0¨\‗«]\┌n\┌K®\¯é\ãI-t\µI\Ú, \0 \0òíØá\Ý\¯Àt┐¬\0$\┼ge#\¶\¦¹G\▀╩í[┘ò~■\±â╗tJnRmáÇ\▄ y▒\r┤g;<▒Eßî║¼PáHçB:Ün²aù\Ã\▄m\¸úZ]▓³-Äú4W╔ü║\\ë│\Ë°\±}h┤1\Îi\╚\Òxº\ÎTqñÄiMÂ  \¶¼\¯\¾\ÈSÇR▒N\Ê6B╣äÿÉæ\ÃAU5\¾k}2KKnNU·0¨f»aCydcó]iÍù1x#\ÚÄWÜôO\ÐH\╔{+\µ\Í\Ê·ûU$n¹·\È&Â╣Á9ÑE¹ú\╠{}j╩ÜdQáTO\ÓbÆ\÷ä)\õc\ððîñé\È_Ej9\ßrT¯ìÅ!e\ãA\¶\¸\þvÃíQ\Þx\÷>ú°Q¢.FdÅn<\Ã_╬çIgwo\╠,\Ë/Æ│rEZ9\ýIA\ÎR\▄(\╬×\¤¨Íí\┘\╚Z\┌i``yhøi \0\¾\┼\"[ó^5*Q\ÈxünH\¶\ã0\├\‗¡┴w1ç¥Ö6éHôj\ÒËî£\Zúª\"░ÁÀiÁø\'\0\▄G:\¶)4|ƒl\§®v²╗Ük┘í╗\ËBDè<PHX\þ\Ù¹\Û¢\±v\Ý\'wûV ×qüÅ#\þ¹½\"èæ\ÕGv7;~]joX\█õï¼Z■øu\Ê\Þ!\¾)L|╔®¬X\¸\ãCº\Ý)\╚³\Ûäb\╚¦Àp22+I\Ì└²\õNðÀ¬1╗Ñ+└ùF\‗?e\¶\─GQI*=┐:®A«\ÛpuÿJ┐°╦ô¨\Ð╗N┼½g═ó;┐q\┼6\Ó\Í3Êôîj,\ZÍÖt┴Vm▓t\"Éhå\Í\‗²8¡T\ZÜ┬ó×\n|\±Z(3┴ía#I(OòI8\¶ñòìéê\█Ñ\'a\¶®;E ¿úl3Â░qN\"Ä\ÓP\█\█\┼:\ÔXıÇ9\0Äö\" \0NÁXÅv\Ý?u©4\‗3\╬}▒ƒ:8(&▒Ñ^\ÌH\‗As1à/©T\Ô\╔³C┌ò\‗èdô╗\\\¸\­JX\¸`+w!»Bá?zâ-Év╣║Áüû\╩0╝ò\È2ƒ¤×| \0:)¿\Ï\¦C¨,æ╝1▒x\¦[	C>d¨sB┤\Þ\¯fÆEÏ▓òT\´ø\┬sƒ\¦S\"n®\¦]Zi®\­ÉZ\¸PmySûf\Ù\ÔYtH\Êsê3£n╝└\Ù¨T\Ù®\Ò╣I\Z\µgôg\Û\╬\ý\¤\¾S$O4\þ¥\▄°┬Çk\0>]\0ù=#P▒:×ô<KH\▄IÅ«v\õ┐╩¼¢Ø╗EÆ9ºek~\ÔgyÉìå\­!\╔\¤<\¶\µ¿Z\r┤\═i\Ë\\\¤mf(╩ºh\Óîâ\õwc9\õî\Ô¼·L\¸vØÜû\Ê+%ü%fI/\µ┴åh²▓F\ý\Ã5╔ùâ│▓├ñ\╦}\┌k{Âè\Ú\"0ûÖI┬Æƒw>\Ì.òÐ╗ûô\nÂ\╬\╠\0\╔¤ä|ÅØT╗>▒5\▄w7w,\¦\ý¹ª#p\┌HF p8ÃÁt,F\÷0\0A\ß©\õd}½ø\Õc\═\Ý.£z£\¯\¶Ñ~ÄJ>®\nnÑ!\µé<°²*\Û«\╚y\Ú\0K\´:&\┌\ÈzL¿\▄(£\┌┼▓ó	O¥*1\Í\Ô\Ô9(¿\╚\█┘é\╔\ıq┤ôZ6r?à\Ò$öëuö(_2 ┴\±gº«kÖ\÷½\ÝF\µC%çg«\'I5\╬=Dc\╠¹■T\╩Â═ûn\ıv▓¤▓\¯\ÍPãùzªgêç\ÝHGùáÜÒÀ║ìµ»®K4Æ┤\¸Æƒ\─°P~\╩·\nïøï╣\‗■#╣\┘Ï│▒\§\'╬ë\┌ãèí-\╔uÜ{ñä\┌\┌%▒\╔\Õ┴\¾\‗V\┌└\╩█Üñ\Ï\ÏWp\ÙÎèDMg^¢╝┤Ð┤ÿª{VFû`ág \0j\n6\ÝÖ\╦\ÊÅNÅ╗\±\"░\‗\═Nè\0êá\0ÁO=ô\Ýmö/uƒ¼@│¬I¢ÜéX\┌jZ\În\Ò\ð\Ý\§/äéH{\╠\¸ ò\┬\µ║7F\n\ÞâNA1í¼\█V°¥╩¡\╩b\´\\ÈÑ\'«\ã	³*Ào¡▄ÉBÕúìèb2@\Ò\¾¬c╔╣\ı	(\Ð\nf\‗\÷\ËMê\═wtû\±\þƒÅÑH*Á\█|Ä\¤0\¶p*Æôèt,▓[vúGU\­\\╝ú\╚AlÈì;ZçRƒ╗å©\ðw\¤AƒLÜ\Û²ô▒│N\╩i-ñFÂB\\F2N=iÄ\┌■I\‗\´\Î¨Î£ÁÆyT8*│Éz■\Ò┤R\ÞÜfÄ\¸wQ\þ`Y\\ôü\ÝÜ/g╗}tú║\Ð,¡· \0£>\╠\È>┬íoÂ┐_\ÈM\Î·é╗ÞìòI=\0íƒ<È®!ÆH\ÔñjZc|.░Â \0F\÷ÂWÑW;At·jF\Ð°çü|\Ã9«ô\█a \0I$ \0s\¾¬\ÚEqåPGíıì7\÷#ò2úm{e½\─ßöú(┴V»\╚\ðk½╗øÆ▒û(└Eb0p*\´qª[{çÄ$î\Ý$\ýP3B~\r\\ \§\╚\ÙY╣\"ïk+K¬Aéy`\±┐╩å?Jqµ│╣(fï\─█ë·\ÔÄKÑ┴$]\█┴\'╦ÑC}\┘\Ôh\È0\¤9\rC\╦$o[\Zæ]T\╔lU\█o▓■³~\Û\Ï{äÀ\¦2│c®\ã\¾\‗\┼3·{x \0Qp\Ð·d×+H║\─11\▄\█z█ô\‗=i┐\"╗\ßÀ¢ÄtWdd└▓\Ã\ÚÄ)┴,r× qÁå³¬*jw&{ï	\ÒQ┴d\╦\¶5ÉOªä\█å%cÆíJÆ~b×:¿¹\ßkós\ãN@Ãæ5.\ËU¢▒QrûA°e9¤░4\"Rrù¹\ã\╔\ÏAoùè\▄m02wÄäƒ╣©\╔Õ¬¬pùB8I^\ð\┌\╠\▄)Â2▄»\þETëó\´\"`\╚\¶«{─ÄÄ\Ë@b\­8\Ò\È\¶Ã¢;g¬>LÂù¡\╚|@|\ÃJ\╬	\¶\÷┼®│║é\┘\÷Øqt¬└¿=\õG\'\Û┤r\Ý\¯ç\Û&F\÷x|┴®©┤\ã¨\¾ñÈâ\‗ñ\¸,<®w%\Ïv▓9¡S \0¨\╬3J\╠y#»$~\ãPô\¶G£TY\─└o╝í\÷>\ıRÆ[wêê\­\¯\´x\█#\ÎÌúZ\Û\Ûw@\Û\Õï \þ\­\þ\¤\ÕYM08┤ıºÆ\ãìàñlê\╠\"å\ßweüÕÂÅJ·>\Õ1qzc{Ö«B*┌ÿ£ü\ð`\þ\ÚWø¹1iÑ5\─:æq╗tÆ│g\0\§\┌»\­«{w¿\Ì³lçt!\Ï\´î \├þââ\Èy}h6ìC7	Å8©ÁÉ║`N\¾ \§\╦d}:\È×\õ\╠\Ê	%V©¦Ä	\¶¤ØFéUÜKºQ4ô└üÇÒ£ü\¶®\ZÑ\ýf\´Muô║Ö1ïv└\0×\'Çk#\Ù}Ø║ýÑîK¿ù7aBd!U\╔V\¶<d|¿\▄]«│ïGÅMÀ\Ë¹¢5\Ô.æ\╦7\Ùñ?t0=2r~Ç\ıA╗S½\╦m<-<s	\╩;ùî┴Wh¤ºçY║ëDÆF▓á\Òcú¨{}j;[\\ù\‗Ñ\Ðu\Ë{YsºJº╗M▒\█\╔o\r\╚^\Ã<Ä╣\÷«òÏ®uk½dÆkÑhÇ\±Fz╗\¤\ËÁ─┤Ö\¯]Ø¡óèI98såU<ä\±╗¿*\Û}ûà¼&Ács:\Ï┌ò\´ \01ÜGp	┴<\┼NP«åR\¦\┘\È\Ì2ØÆdâ┤£s╗\╠V¹╗ú²$¼)	¿6═▒¿└S\╔ÌÀ\±e¨g\═ea\ßê├Æ█Åª\╠Rÿ]w×z\ËxènI*}ë®1\═,kÁd5¡â░u\µë═╝É]└Z	$0¬~╣\÷gíKhÅÑ\╚m.C\0{\Í}î¥ÿ»ÑtÄY\Î-&=âS/d¹SÎ®\╬h\¯¹fú×7┘¢▓\Ú\ð\Ãe¿9╝R\┼╦»åC\õáy¹Û»º\Þ\Ë\Ú\¸ÆïàA\Óeäâ\¤\´\Ô╗ Áh£áüæT\┼\═²\╚\═Î»Z|qRb\╩M█ª$@G\Ôî}ïì\┌\Ãj\Ú#·¢A·\õ\ÝO}É\Ù:nùu\┌F\È/aÂ\▀2m\´[np_8¬\õ\\í\"\Ý3½vú\Ó\¦´ƒé╣7c9¹cSô┼í\Ò\╦\¯U\Ù]\Ýƒgu\r*\µ\┬\╦VÂ×\ÛT!#BIlu\‗«eá\ÙÂz┌ø\Ì\▀	D)o┤ÿ\Ëq\Õp8®╬®5\÷4Wz\┼.╣║©9<\╚\▄h\ı\Ã³i\Ú2░iZ\õ═£Â-À\¾¬T▓wÆ╝øXbFGLÆp}\r_M\┌%4\Ê\õ\ð\ÚU«\¦e{:N0;┴ôVP*╣\█T/ñEy┬î×3\ÕT\╔³X!³Ä\Ò┘É½\┘=$cÁLd¹P\Ì\┘\¾ú├é9©Q\¤Í½·}Å\┌d:}Á¬]\ÞPEaUÄ\Ô°\Ã\ÒªÁ;\È\█\█+\Ù\┌┼¢\╠,\Ó,P«0Ì¢+\├─á\¾º©\Þ|Eò×└²▓9e!ä2\¶?\ý\n\´oô`\¶«	\Ï\¯\╬\▄Ù¢╝\Í^\▀UøLû\ıäæ(f`■9\ÚÊ»Îƒfmun\┬\¾┤·╝\├\Ùv \0\n\¶¿╗B┤©\÷\Î?\ß°H²Ju\¾<\ı|*fúí\├\┘╦┐\Ð\Í\¾\▄Lêü─ô╣v\╦u\õ³¬ «╝KÓ¿î╗r\┬M\Ã\Óá└\n3s■i7\§\rè¬èb┘ÿ\Ã=E\"]/┤í~&\ÎEK╗\±$ë2å\ÃNF}Aºy\"¼\┌Wkt[[x\ý$\ımß╣ìv╝R®=qÅ:\‗┐\╩\þ╦ºã×(ngV×*o\õ\╬n;Ml\Ë\╔o5à\─rD\─:Æ=*m¥íap¹;\Òƒ│7ç*¡N\╔&│¬HÑJIr\ý»\È\Ú]o\ýçOÂ¢▓\ÍÒ║▒è\Õ;ÏÂ\¸è\▀	\Ò?¥®¥ß╣«\ã|>\nº┬¼▓$¼zwd6}©¿\‗i\Ð1\¯fê	*\Òo\ð\ı\¯\¾▒Z%\Ã\┌ri\╦d-l\▀M\´Ö!rÿôv3R;]í\Ã┘ìe░\ı.D┴É&uÉò\'×J\µá\þ\┼\ÎEº┴\╦\¯┤;w@*└\õ<Ü6òvÅÂ\Õ\nzÄö[W\ı/ \Î\´\ÍâD│íù#º¡Ñ\ı&▓å\Ý\¶┼û«>$ì\÷\µ_\┘\Ã³kªTTøóm╗h«\█\┼y\Ï&<rT`■t\õ\‗\▄\"<c╗ \­SÄ×┤oVÄKGÜ\┌\µ7ÀÿÉF6¹fíG$Fí▓╩íøq└?\▀Z9\­\╔:\÷Ç \0ñ-dÁK}\═``%F=9®\▀æ$èsËì¢3\Ã\\Å|Q\ÐÒ║Àë\ıF\Ã▒ÄÖí\Î:\0iøN\ýc\┬q]Q£ÿÁ\├e\┌\═FYf\¦B\0\█\Ô\0Å¿\õ\Ð;uº\┌K╗y\ÝæN▓°O\╦╬╣\Ùi\¦\╚└\õc#Ñ▒\ý\¸io\ý\r═ö\Ûò┬££Åc³ifú■\├F\Ë\Ó\Û\÷ZàÂíhÀvr	 ©\┼J\þ\ÞirL@╣¬eøÁ\█\¦-Ñ┼ñ I-«\¤\§\Óî\Ó\ı\‗¹\╦h\┌\Û8í©+\ÒÄ\'.á¹WƒÆó°:\±▄É┤@MjøÖ└\rà█Æ	\‗\═Q\þ\Í\§M:xZ\µ4]ü┐R\╩9\'─©½■®½\┘iF\Ò¥Y$\ã\±¹¿F╣eiwÑ5\ÈÍÄVìú\¤R\¤╩î2\Ê\ÞYbÀ\┘Wk╗╦ÿ¬DvD\0\r║nñ2F}<ìVfÄHäÅ·\¶\┌\õoÉc\‗\'╬¼\¾G-¢\▄\Í0B®Ãâø\´¡é2:ri6wæ;\"└cÂ#║2øôwBy\¤$y\ı\ÍN:9ÕÄêZe«Ñ%Á\▄Vq\├,ôó«f\ÓòÉÖ¨\¾\þDu©ãùwo£\¾M#¼┴wä$rñƒ/ùùZj\þ\ß\¶\Ù╗X\õiàï\¸yu>K\¤#=H>ò1\§Kcj\÷é\┘R\µg\█$N³\ã1┴g$î«+oaP^\╩<}\Ô½▓w«\ÊG\­®qirIª¡\╠SF\¦\µ\Ý\Ð¢\▄ô\´×*┼ívmå½}kxª\┌UE\╩\\9P<c$y░\┼]K░1k\Ú\§e6\ýçT\¾å\¸┴\╚\Ò╩ïÜAÆ¹#\┘\¶\ı\¶i$cl\Ôq\¦4.╗À\ãYqé¥|Å¡]t.═ñZcá▒3	ns!iî%B\­6Ò×â<A¹=d4Èà\¶\Ð_mdÉ+gÉ\╚³=j\§g%┴ViCr|#n¥í,£òîI░YGoè(ÖQF\0$À\Î\'ôX\Ð`\§\Ã╬┤ù½p \0CO,\ý\▀z5>\µôx¹lkkÄö\‗w\├Uë■¡9ò \0^\§E=\þ¡\Ú\§¼\‗#m24╝#&#\Îm>XçÄ#¤Á\'\Ò.\Ï#\┘+Ñƒáò¤«\õ7Y¿Z\═\╬{░OÑs;▄ØF\ý░3>1¾«û║ªr=vü\\\Ê\ýo¥©#+\▀]\ZWmÆ\╔\ð\┌q* E¹%\ð\¶ìG@\Èn»t¹{ëùQò;\╔Wwu¨\ðe#z³\┼¹$\È-¡;¿Ö\¯aîÂ½1\┌\¯GçÜ}T£íq╗LÁv¬\¤IçDskmg\╚ÙÀ╣ëUç<\Ò╣Àe\Ð$\Ý\µ▓_éÂ\Ð\r┴AÃêU\¸ÁWÜãÿV\Ê}>KÎò0\"ug#<\¶\µ╣fò-øv\╦T[¡N\Í\╚)ë│=\├D$c*s\´\┼sM\╦&E1Áruë5\╚\▄\Ãg<bÃèvÄ¥x¬ôûfbHÂN:Ü7▒\┘\Íı¡/%\Ýø¼M#Á¼(]Yÿ``¹:└4î\Ûúk1*Oª|®┐\Ãc£s6ªqùBA¬\¸lI\Zeúq╣nS£gƒòX▒U\¯\ÏA<·m║[┴$╬Àè\ã2p=\Ù\ð╠ÀE\Î\ÐV\¯N\ß\r\Ï°{}\╚X p<>~öÁ\ýìOf9\═V┐\ã| è-\±øn▄¢\─kÀîy\ÈV\Ý$║¡îVS\ÚÅh▒Dì:Kô\‗+\þt\┌MDuöùNI\├kóG\┘Sn\Ý\Îiy\§\¾²ú]r\ÞXW\ýÌ®½vcÁ\Z╬íƒ\┬^*\"\¸ôì╝þÅØY\µ¹E\Ýé#\Ë\¶\╚\╔\Ú©\╩\─~U\ÝJcD\▀2▒\¯\Ïx╗G&■à?ØÑ═®^\Û\‗¡E I\¤`Vü²ni5Ðå.0Iæƒ\‗\Z╣ \05ø·åä└ú?\µ│PðüWêå\±ò\┼FùMÂ©fi\"R[ô\ÃSRçJ\ÌqZù░■ü-┘ï-■\n\█y\┼-d╝\ý▓\´\Ë\§+ïq#B7\¤\▀Êª\¦\▄³<\¯F|ç¡mTàSs4ôd#ù\n\Òº \È2¿\ıñ¿\ın/!\Èn»\ÕÜ\ÕPD$(>\Þ9Û©®Ü\´j\§-i¡■0F╩¬@m©-\╬zy\§¬\ÙC4Q■»\§\õIü°\Î<¢~L\§ññ\╠-«╠«ç!6Ä}N}3Ü\Ò\±╝j\Ý×iY▄åc\È(\Óƒ╬¼\┌.Ñeàcº╩º╝âRI\Ï$(X\¾\§\┼èE╗ËúåB\Ë8▒\╩\¯\0cº░¼/\nKQ\0FåW~}~G╬Ö\Ò°Ñ\Þ\Ð\╔V\┬²½û+╗ùy.-«{┘âÖ!ù\─\Û[ì\├\­\±\È|¿]\÷Æû\÷Æ\▄\ã\╔$\┬\¯o\±æÉ<\ã1\═\rëñE27pé\Ï_?~¥ÁÆ\┬#	┤ÀRwòo	\0Øá{:H\Òñ┴¢0\╠\r4w\╦e)sî8\¤]\╠2W\ÚS×\0┌à\Þ\¾╝v\Î\"\┘\╚C\"│qû\Ú\ãjfØ,ôñóaÖ\ÒÄ@»CNÊå\ÍFr·=©8# è%\┘]?\¶¢Ø\õW7SEco1Ak\ýp	,├ƒ>ÇèÅ\"sÎè)\Ï \0\È┼¬xé»\┼dô\Ë\¯è\Z©TxØ\‗é\ã\╬\╩$QJà\0g\Î=I¨ÈÄ\¯%;B\‗R\¦h-\´i4\Ù(À<²\ß\Ì#Ïá\╔\ã³~\╔\‗5┤w\Ê\┌\┬dÁÈúì\Í,³&\ðLüêåy\╚\╬qÊ╝»}Ø½%\þå\├Qü\▀u╝¡\"¹═╣\Þz{\¾B\´\ý\┌\ÙOë5	&ìä\╠8\┌A\Ó\§L▓ÿÑ«!×KQ║$yw\Ì\þ\─\Ó×\╚³╝½ú¿i-\ÓF!├¿ å¦ôÄ┤\╦\õ(║Íùyy┴vÄ:H$\0+\╚q\¤\nI¥®èbÇÖ\Í9-cq▒ò┴b■î0|\ãA¨\ı\ÙÁ7îÎ»io\═(çàHXü8\¦\┼:²ÿ┤].\▀\Ô\Ì0J\ß@8ly╗sâUèñJR▓Ø╗ƒOôP\Ë,\ÕCwJÀ¼C╠«\¯Ö\╬8í\±i▓³j!åh\ÕÇ\ÔT\'ò#\╚\¦V\ÝJ{k=\▀L\ËfÀ£å[äS┤£ÆF\0îÄ╝ÜzÎ│\Ê\╔.×\Û\±¼çyæóâ\╚l~NzË¬\┘]║\Ý\\\¸░<Kq\ZFÆ\Ô®\n8¹\õy£q\§\═\ý\÷À-\Ý¢┼ï_\¦\├f¿½\ZåÉü■\Ð¹¼O\0³¬ÁaºE\┌I\ÍYf▓æÿ\'(pq²ly\¶\═\Ë/Öú-®B&y¹\¦°2\þ?\¾\þR×¦╝\"\Û\█\ý┐hZ×ƒt\Îz|·m\╠W@Ø£\µN\ýÄ©\¾\Þ~y½¢Ø═▓\┘\┼v¹\ı\ð\¤),q\µOÖ«Im}.ƒksul\Ý¢\'+╝re\▄9\¦\µqè┤\Ú:\Ã\ÚHö╠á4{\ZWÆL.¦úË¿¦Æ1\‗«<ï\┘\Ð6]\╠\­æ\─;}\├I?\¦\rQ\Ý£Lå<» Hz¹ezQbB╝\▄/■°W7\õ\±äj═¼╝\Ë¹?\¶▒}d\'¨Vwƒ\¯\╚¨V\¾â\ãG\▀\Ù[NÖÂ\±\ß·-ieì│╝á\¶\¦\Ã\­ªY\═\Ô49¨U.c·\Ú\Ì\Z╗B2»	>øÅ\¸U-\Î\ã\¦:×ò▀í╔╣\╚\þ\¤\nDv4:=NîûKHUÅêìØ[<\Ð\'\­\Í\¾æ\þ¨W¿Ækôïö\ã ▒é\┌@\Ð!B¢ztÑ|\r#?v\¤CÜ|Gƒ\ÕJUo\Ïc\¶íH*\─,hÅ¬âNä+\õ1\¾Ñ*>@*px\ÚNG\þoá\õ\þI╣Djlkó¬\ı └■öÄW#îƒ.£\ÐSL\█\Z\ZácÅ╩│j»8\ÚJ\ßF7Ä=H¡n\0r\±\Ò\¦└ªN,Y¼/]¢k\\y.+\Ð¹\╠█¥_\´ª\┌\µ\ÈnbÎ¢_\´úi{\Í9è\¦G7\÷M \0}À\Ã¹\┼\Ã\±ñWNG┴¢Á¤ºx	·V\¦░mC\Î?\µÆ \0Tðü\È³\Ûm╬¡º|4ú\Òa\ÕO\Ôíb\‗\─\õ³\\\0w\0|▓jèiv╠ô·%yRI└9¨R\"×)åÞªÄA\ÛÄò)¤è×->Ç\─KlùPd\¤#Ñ\Í-\Ì\0æ\¸×9Gè<║{è<\¾\╬)ø╗$¢uòÿ½¿┘ép}*yckÄ┴%ö\µ·\§d¨ìH\ÛXq£\╚\═2\Ð \0ö]K\¦\ý¨âá\Þ8·S▒\Ú3i\Ëx]QO\r\╬■Ø\Zð┤╦ìeäVQ.ÈÉ	ø~G\█¨Îøô#é¨Ä6\╩\­\Ëu+ÀEos,Ñç)\þ╦Üô5ù\├é\¯\Ê[[Ér¬Cz\Òƒ*\´]æ\ð\█B\Ê^\┘\õùö┐Nâ\╚\þU▀┤.\╠\Ãuas¿\¸\╬\‗ #×=Ey»]\‗ó½\­qW!.Ñe\0á\\ì╚ñB▒\█┴nÃ╝ô\┬¹öÉ╣9¹┐Z\'·?ÃÄ\¸kchlp\Ì\È─░äÂü7\r£zg\¾\Ù]\±╠Ø4NX\÷\÷;o\Õ\Ò2\█G\±E└\‗<}#³\"û\ÊSi<q\'t\█]yQ;-\"\‗\µ┴╗ÖñÁ\Ò\├\"\ãy¨Ü¡\\\÷z\¯\Ì°Á╠óP\õæ!\ÕO®¨\Ëa\ÍmøV;\Ën\Ã{I\Î}ªi[e║¬.pJƒÑÊ╗Mmgez»k1k\┘2ëÄ\ncâ\ÙAá\Ët\¸\"▒\\¿Ãƒ\þM\╦lkoìƒ*T·Ä|²sU╔Ür²X\ÞL\¸û▒\█\ýòR1pC¿:ôùà=8\õSi^Os4Îæ¬┴Çr7╗t\ná×Iº^\┬\ßâ\╦$é/\Ì\\x}x\¾\§_ô¢{░\ÏLd\ý$\Óy\ÈS\▄1b¼\ð7A4Ç/rÅ\'#\¾\¾\¾=*\ßñk:}¢Ø¥ƒdn.dÄ$ÆT\ãZ \▄A\õò\ÚÅ@+û\Ô³D\Ùn\Ð°\¸Dñ▒np1ƒÖ\═M\ËSWÉK\­m╣ \0Y;\ã<eA\Û\Ã\▀\╦\Ýpu®íé[©.%dRÖ\╬\¦\Õ©┴\¤_ù¡j■└j6\═▓╝h╦Áîk\╚J║\ËuØj\ßX┤Ð¡Â\┘U\ã\\²\Ë\Î­»ùÂ=\Û·Ñ\ı¢}\Êmê\ÞMEÂ║e/▓┐o┘ïm1KG\Ô\­Æ╗\▄wzæ\þ\┼ùP6io\Z\┬\Ë4èYJGüÅÉ\þ5&\¯·\r:×\¯eD\╚\0▒8\¤ÊÇ_k0]ó¼l\'g`-\Í3┤;y¹#\ÙEJL\╬1E%!7ù\┘\┌\Z0\\\─\0tÑ½\ÃCé\┼┴█å\╚S \0>T\¾(6\┬D\┬+\ÊG\þ\Úî\Íñï\r\¦+¥\Ìæ\±\─¹è7d#*\Z×\´\¶p\­\┬be;\Õ6û·×~×\¶[IÈ┤¹h\´gwø\ßv¿\n½┤.\´\¶\Ó\±è	~ìrAÖ╗ÂogbÕç┐º®k:$;b\Õ3©à\þ#\ÙÍä\ßï\├;ë\Êt\Ì\Ï\ÚgJyF¬ì░\þ|\Ï+×\0Á	>\È-Z5e│ö\¾kn q\Û=köÂÆ\¯\ß{└>º\┬z\þÌë$KH\Ôee^øÅ8¿~4_\¾\¶uê;wº\═■\­úl/░«\þ\ÙÊê\╦\┌;KUç\Ônm\Ò3░\¯nN|ÅÑqKïy\¯-Â\Ã©\þ4\ÌUòòÃî1,U?\▀J┤0}1┐.╗G|çÀ62jïa\­\╬	ö\Ã\Ì\Òzq\þÃ¥(\ÔkÜz=\╠OwlôB2\Ðn\õîpEy\¾J\Î\§]-×;\µÄi▒ÂP9A££gªjl│j\▀¡F\Ú\¯3Ây=i%ó\þåS\‗\Ó\Ô_.╗o½E¡ëí²]øÂ	ö\'=rh6½\┌}Om╠ûÅ+╝ÿ\╩\0H\Úô┴¬\¶À▄óÄ\¶ö@\▄cÑIØqñ`~\ãrFk│/\Z\Ó\Ò\‗oæsùDÍù▓·F░¦ªÖÄírå\┘<\¶®ê\ý╬ú┌¢2\µ\¯~\Ë^█┤7\rÈë y\¶½lZk\Ûé\ý\╠1/¦┤x\Þ6ƒ*c\ýp\þ│\ZÖ\╔!u	\ã1èµûº/╦ƒóÄÏÿÂ=ì║\ýãéuH¹G¿]HgH¹╣N\ÔGÉ\‗\┼\ý.òq\█\r~\¯\ã\´\\\È-íå\▄J>ö19Ãÿ5p¹O╣övvk&║\0]\Ã&gê£^1A~\ãG²/\ı[┼Æ \0j»Å4×oÆ{)ûû¹$▒ü\r\Î°E\┌	CxFÖÆ9\þ\├ËèƒÑiìpF\ý\‗@?╬«ùY\Z|\¦#~┐#C;6ÞÂïô\╔Q£\È\ÌYJòå<&ùB\┌h\╬=·W-¹JéHt¹%Wtoê┴*\┼|¢½¥I5╣YB¿Æ¢kê}¬hlè×\r\Ð■°ñÈ®╗\nwë²é\ýë»h\Î7Zî:\▄4iïç.=²Ú┐┤>\┼\Þ]×\Ð\Ý«t╗ibÖ\µ\╠\‗╗\õ}Ió¢ëÎá\Ð{$\Ý9\╩w\¯¹W\´38Å┤;█╗\Ì\╚i\¸7J¿▓\▄T@\‗Ìº\¸Æ¼ì\þ▒Z&ƒ¼v\╩MB\Ð\'éF\├!\╚\╚¨è\ý \0ÔÀ▒_²┐_\§Å²\§\╦>╬┐\Ý\§º╗t·Wáï@\┼6V▄ƒ&ñp\═C▓·$mZ<Zdº\╔\ZâQ\╔\¸\Û[u>\┼\Þ▒Y\\┤Z-¬lìÖJ«J\ÓP¢W³A\Ú\├╦║Å \0\þ%_5¹¹;=\ÛKë12®\'ÆH\‗u╦Ö╗èRc\Ò}\┌<\µ\±#└w(9A\Õ]\ý\ÎNÁ╝\Ð%Yl-gs9û~1\¯+ƒ©+\þh\═t \0▓\þd\ð\ÕÄ-Øß£▒\¤P©\Ã\±«\¦t£pªƒðÿ╣ô)6\­\█\÷ù┤I1┬ït\0H\È(\0\õ(æ<\È+¹M\┌\"I\¤\┼\þ\õjwØzZF\Ì(£¨ù╔ÿ8<RË×O4èq¡&í\Þ\¯bå\¯d█║P\Ã,x8\Ã\¸\È■\Ã\Û\Z~òÑ¿	\▀\Ì]<d#cq\ãG\╦3║F\ı\¶\ÚT░fL▓â\Î[D8âKn/ø>■6\Ôkú\ð┴e\¸[\È¯┤¥\╩^\▀Ï¼r\\\█E\Ì*I\¸[æ¨Ux\ÙZ\¸┘ñ┌ò\ýqG<Ðû1\┼\¸S×\0ó=á©\¤cup	 ËâU}\Z_·ó┴o\¶/³k\Ã┌╝w^\╩\ÃLÑ\▀\‗X\Z)╗╣;\┼\├\Ë#Ü!É\█,Æ\╠9ö¬ésÄ~\§O©;m\Óá0taÜb\¾\├\r╣\0\Ò\╦q»GL\┌T├®ä\\ôA┘╗9«\ÞÂ\÷Àùùlûù\0\Ð\'.î\‗1\┼\Ý¢\µ×\┘\Ã\Ì+ñv\§£v│\╬\ÕFH°u\╩\§ç2\Ï6\▄d0\­¹Pì¨W\¶Ix\÷?Ï¢╗Y\┌!ª=\╩┼ïY%Vt\¦\¸Jîqâ°¢j\Ýy\÷1siù½█│DÑ\─k&q\¾&ü}Åq\÷Ç=G\¤³R╗\¯¬\┼t╗│\È°Wnªn-\Ð\þGûæ\µ[»ï]\Ô`\ÛL\­\ý \rú8‗í¢î\Ê-n╗Iûí×Bûa³\r\ÈÄ\╩\\s\Ê²çO\÷¬¢-\¶\┌\\À7vncØ!\­©\Û2@\ÒË¡m:ö\±▓╣\÷\┬J╬ï?a┤äØµÀâ╣&0¿ÿ,½\╔\╦S\¾ÚèÖeñi·[ƒàå!+FH	>\Ë#=3\\Å³3\ÎLs\'\Úè\╦\Î\¦\╔,\¶?.Á\n\Ã_È┤\Ù©\¯m¯ø╝D\├╣\╚\Z>¹d³░·;ë┴\Òw×y¿Z×íò`\¸r!æPî(8g╔«^²¥\Ý¡\╠\nH\ã;ò\Ù\Ù\ÝBu\rsRÈÄ¹╦ªæWá*|\±Ai\┌\ý/4kÇ┐h¹Ou¬\\\╔\nJÏ×|Ãíá1╝▓Hæ¼ä└Â0|╣ªr6®\'ÅØN\Ë-\‗ÖYéê\Ï0\▄2»\Ý]Æ9£øeåWiƒf\¦└«\Ó╗rág\╚3Qg┐Åpæ[úf\╔bGí\¾®│\╔o4û¨.╗ÿ\╚\▄=ê■íf)ªô\╩#ø8M■G\Ë\þQéLQ┴y└│â#\Ò╦î³i\÷\´¡-\ÊC│y|-²\ÏZÍ×½l▓4®è,9.úª1\¾ñ]]\╔)x\┘$U^WfF>y\'?*u┘æ.ÜHÏ¥\─\0\¯`ç4\╩\╠%ø└F\§<dyP\õÜx$X\ð;Â2X\0s²ÈÁû\´\'0│g╚îPÏå&M¿ÑDâ\¤9\┼I}@î`F\§ h46\¾°û{rå\╬H\¶\ÙI*\¯hùo\ßWƒØmê*L&5─½l>D\Ó\ÝL|Ir\0S\Îèd)\Ð-\µ@\ÙHYº┴%.r@║▓é3v7<`r¡\╔└ó7\ZFq■ÄÇ@ndb▒E$ø╣█Å╗V▄ìr0\┬1\┼4iX°╗;xà$\ý_g\õ{╣m╗½=├║8,v3\ÚU´▓½┤Àýåí+û;\§)é»»êká\┼\Ï>\═\§\0@ƒ\ý\Z\÷c2\÷\Z\Ý▄▓$zä\‗1xU»#j3o\Ý¬7²Ç╗}vù7!À\¦wcÇº\ðò¹î×\Ð\ÙpXZá?¨¿k\§\'\È\ý\ÝÎ╗î|9òFå\§úd\Zé\█k:\╦2\­-\ÔPG▒\┼uBIiÝÆÜ{®ó°ò\Ê\¯8\Ú\¾²ôT]7RhwE\¤m\È/\µm6\Ùm▒*al■\\\Ì9\0@Å╩Ä\rÖb┴ÅeûmbI\Ã\õ\╬~\Ð\╔g`I\ÙràYò\╦y\ıSÀ\þ³ïO \0\÷■Ü\Úå%ìp+D}7XÄ\Í\ı- C-\ÙJÄ\0T(\╔\ÃþèÁ\÷\§\¯Åbtu╗@Ä\‗û*<╝¨³\Þ7┘¥æo¬\÷Ö\═\Ê\õCu¤ƒ:Â}»Ü.ÿñé{\÷\þ\õq\Ôé\¾Y▓JÆE\ý\Ù \0¿JH\╬\Ò\Ë\Õ^å8+\Ã?.k\═¦ä{ÿ¹u┤ì%©\├wq\╚¹UÄ<¤ò]ç\┌\¦▄ù\╩^\¤³K\r╗╗ëv\Õ│└\þ\¤5L\╦\µ\Þ	XØH¹z\Ë_Éq\Ò?ÈÆ½}░\´f¹@ÈáR[3*¿$\Ò\ã<\Ú\Ù7\Z7o`\ı\§M>\Û\ÌkU\¦%¼\╠ÿ┴\0\±²n+ZÌú²«©\ıU\╠b\´í<@Çc>g\Îñ\┘\╠X\±}Ç\Õa\ÃMÁ\Ê■\╠^8t+úà\ÌËîôÎªÊ╣╝\Ï\n▄îbÄvo\\ÅE░ûUl\╬\Ê*óìôÜ¥¥Xx¹Nn\ã-\¯\Ê\÷à¢nüÃºZƒ\þC4Â2k║\¾ª\õ1ÖPÄk\Ð\Ê\±å?úø/3fà<ö\ðËï\ÃZªF\Þ06[m\¯£Qp3\§¹¿ÄûJY┘é0R\¾?øÁWM\╦³t{öÆ/\þËâÅ¤Ü▒└B┴î\‗ùq■\‗k\╚\È+╗;▒>C║\▄¹╗%¬&N~O\Ój╗ó\═ \0Ulº\ð\╚0\ÈGT\´.\¶;╗X├▒ò6\ßOZºE5ùe%Ë«!óeSy\Õ┴\õÎ£\ß\­»·u{▓ƒqs\┬@b¦ÁQ│\§¤ÀÁkQ \03ïQ\█wÀ&úÃÀ{&0CNjVñîÂköìw3\Ò×¥u█ÅåÖ┐GF\ÝÈç³_vf@®┬®*FG\¶U\═5½■ \0I\¯~\═\ÓDë\0y\═t^\┌6 \0│n\═¹ \0\‗«]~í\¯¨;ê\¦\ÃJ\\U)Ïæuì»\Ïo\ý▓\±l╗w\‗Ø▒³<¬\þ\╚)\█\╔?A^é\È\¯^\¤\¦\╔çFÀ$\þ\┼yÅ│\¸\Ýi¡)éqHì!C\Ó\­yÄ8²\ı\┘t~\ð┴\'e_NY\Òv6\Ê;╝Æ*╝@ô\╦\‗g\‗└\‗\═SU7&Äh\÷ÄK¿░ \0/AÞÀñ\þ¹TPï\§wg0pƒêQ\ÝH½\÷[På>3¿9³}h\\\õ.óc\"¦åƒ\"»ñMAö\ı;h\rë¥W$\╬\Ï#~u2\r\┬$++\¸Ä,┐å×èug*ÄK\0\0\¾\Ù[\¯\┘c\r),▀å1îƒØ4º+9\Ãgb\╔Qx\õpƒ╬ë-¢¼\╦hí0×d\õÜr\n*2y5®\µ\¯ú\╩,eç$\Ý®╣╔å\ð\█[+F32Äá\ÒèdY&HÈÇ\─ôÇóù\ËL\þyþ┤è\ı\─\Ê]\¯%\0/\Ò-îQMôc6Àî6C\▀ëèª>âº\‗\═.ëÑ*\Ù3\"îô	©â\ð/ÑI«0î\þi\Þí@\Ù\ÚO\"%®UVB\┘\±0\¶\¶\¶ªo\Þkò.ÖÇ\Î!©\­ß¢ë\õ²iñ▓yKwà\╦3n ×*lb,æ┤Ç!z(\ÓR\¯\nDÀe\\Çú╦üJÀ¿®█çƒJ\ÍSÊò╔äå \¸½}\ãz¨Tá+LUz\ð\▄\╠F\├ôƒò)mù\È\§\¶¼Æ\ßW\­â\´Ü\Ï\È-\Ð1<ö\±Ç°ª¨°°\Ýª╬£\¶®O,\ZØëU\­Â\▄U}┤\¦KPÜCeldï\­ù`©\÷\þ¡\Êt-V7VÂè1\¸ô\ÔS?ãî\ß*+Å│Ñ\÷\┌b¦å\ýßÇÉ▒©V\0ƒ(\█ç\÷\ÔH■\═\´Yå║©\¤õéóíY#°S¬\Ì\┼Êá-\Ê\¸aH\õrqK▒\Ð-¡,\Z\╩\Ë^×+rI1,¿T\þ\¾\÷²\ı┴û▒©Ø\Ï\ËTWuÿ\\ik6\ýÄ°!\±}W\ý┐VâI\ı\§Ö\¯lo(\Ú\╔\¤Jvn\╩Z\\\─@\Ý\ÕNN\ÕC¹\Ú¢?░Â»+\╦╗óC\þnÅîS\\<O \0\Óeì╣\¯ó³²┤┤kY¡\ıÏ┤áó«G\Ôüä\Ò8\¾\ÙQa\ýt\ð╚ÄùVß│å▓U<z0\Z!·?QrJ█í\\\þ=\Ó■4\┌\'ÅZrh╣>\"j6\Ô½=¥Ë¼ \0\╚?·j\Îù~[æ³\õávƒ▒┌ª╗khÉ]X┴\¦K╝\¸\Êx\Ã«®jq_\‗\"▒\╔*hù\÷A$iw®,\┼\╠q\ý\¤┴\╔\Ò\‗®l«ºKÊè▓░2\╔\╚¨-\ð¹)\┌\r²o\"M.\±ævàÆv\0âIÝòÅiu¹{hÄçe	üÿ¯À©\╬\ý\ÒîòsG,VN\ı,m\¾E[\ý\ß└¹HÁ-Çsô\Õ\ß\§ºô▒Üçi\Í\¾┤,Ie=\ßè$ÆMä\r¹8C&ú\Ú²ù\ÝVò®\▄¦Ø3éÄè\±╬äÇ\├3E¡\ß\Ý\r¥ù£,Áå6#X\ß*<[║×y#5L╣w\Z\ÏT_\Ð[\Î\ýkWû·ò\╩\▄\\Zân\‗Ää \Ò\ðfó\┌O<\õ\´	ô+\├\Õp8\‗\¤\Õè7q\┘}zk\┘\Õm2\ý	Yÿ│*âÆ=:~U&MUÀ│t]e²Iî░ç$\¶\þ#\Õ¹\Ú²\─\▄\"¢?░\÷ª<\┌©å°ö\¤\§q²\§.\µ\┌\Û5`\÷ôâÄs\═=ñ¼h\Ð^iWÀ│	c1ÉíF\ÊA³\Ù½:n$\±4▓4J\ð\╬\Ýc]\¤^²	³¿\├-PÑ\Î\¯\¶¹\█\╔,Ö#kë72║å#\n?┘«ÊØ^i,«Q\þå_\ãS\ÝW┴4áóFkûÜ}#\¦X▒\Ý\┌1Æx¬\'k5}OJ\Î&ÁYJ[2dðâÎ»\‗¬d|.â\¸@ìftG!┤rE[ØN█░\¦[\¾\¾«Wª\Û\ËNÆ\\\╔\'x\ã,Æ[ î·ÅAL~×\Î.~\"╗ûî╗ÏàõÀÉ\═y\¾\ã\þh\ÚÄMºdådÖè!\Ô\þ╬Ç\÷ô]│▒\ÊW\█p\─:îu╠¼\þ\ımo{¹kıågm─│\Ò>ãª\÷Ä\µ \0┤7]<$░┼▒ärgy\¤ZÆ\Ë=\Í╩¢J\█\┬%\█\═\┼\┘`YU╣Fs³®\¦V·\┌{h\Í─ü\▄\þ>EìSµ▓╣üwL¼z\¯\═3èÀà]Éyø;7h;Qú\Û]é\Ðló¥î\¦[xeë\ã\n\ß1Ã¬\þ╬╣■íA\"\┼qíê▄Ñ·}j░<x\¸\ÚSúÊ«î2I,2ÇSr=Ny·R\├\nç░<«¬å\╬ÐûfÀ|îm╦ƒQ³ÞàÁÏê`Ñ║»,UôwùùÂ)\µ\ý¨ÆXÜ\'\n\Z/¢Ä3\┼;Ö1Ö\╔\±;I\ã\0U«/▓j¹#Â╗,░2\"Hdq╣ 9\╬k5{Åå╝\╔\Ûb└\­·æM6æp/\Ô1ã½\Zò\╬\´/2i\ÝkN©yá1!æèØ\Ã╩ÖR\\Â¹ E®G\ß!\╦H\Ú\§®Kp\ÎÅ¦┤pkV:?éEÿ\¯Ç\r╦Ç	\¤³(\§«ƒkä\ýh\\\0ñ\Ò®4+░Jn\´Z@└û\Þ\ËwWª┴Ux\▄9\┼¢\Ï\ßDh©ì╝\"ó7u\Z\¸@lqÃØ,{1╗e`¼\Õr¤î\þI\­└@É\Ó¥O7nì*H\ÕùwñoE▀╣F³\ß@jmó4╔ÑF\ã#éiÁ\█wøIc\¸B×@¡3║©U\n:\þ\Ë╬ñ[Lb¢\ßI\█\ãv\­x&ªk5\¸ùí\þÆ\r`\ÚK\´#æÖ\╦\õ▒¤äd\n┴┤²\Ê~úíL\ð\r\ÕYç\¶ÑgÁ╣╝ì(E\0=ZÆÐç\Ô│\─?¡\õÍ│	K8xÆs\ËwV\Í}>\┘1·:#\Ë-×hf\Ò \0\Z\┬\¦22ôF\r\\j\Ûm\ÏYÂ\╔	\Ì*┤Æ	╠Åx▒J\ÒÃ┤\¦P▓╣\╔£\Ë,ìì©!i4yHW`bD\─\Ó~]iÖ.«,ê\ý\õ21]í\±┤zÜê\Óû\rô\┼f\Í\╔<d\§\ÓsG{│n&[+¡┤\ÐÀ1\╩1\▄}\±Oi\‗Ïïp#ì 	²æƒLfåò\╚\Úé1îVÈé3\¯+<â,îøi\±¡s1ÿ\¸v¨!d\╔?@jS\¸\ÚÄ\ã\Ý\Ôùp┴,qAúG^}©¡Â\µ$û\'8\¾\ÚY\╬?F\‗0\▄·¡¡ôÀ\Ã\¤q*\þn\Ã#w\¯¿\Ð\÷½SbùQ╗Ä\\\Ó$Ä	í\ß\õQì\ýGí4ÆÑÿ▒┴o\┌#ÜÅ\▄l\╦$¢░½I¼O¿|tÜò\┌m>Cm{Q®o`/w9\▀\rUË½dJ \0Ø)«\¯\±─ñÅqÍèX}\─>Yzeûn\Í\Û\Ê \┘sP2H@\´4\ð\Î\§øçÄ3®Lü©0QÅƒJ¡┐y(└\ÚîbÂüvÆv·Üp«óo,¢░×Ñ®\Ûl▓┬Üñ\´:\­Hl\¾\¯G\‗ª,»º6¹u®ï■\Ð5▒\╬w8>íq[\ÚÅ\Ò┌ÖN+²PÄl\"/-_─▓3\ÙÜr=T#+}│Ü┼ø\Ë\Þ+\\î°Ç¤ÁS╠£iü:\õ	>ïs$\¯Ðÿ\╩$s\Î\'4ø+Kì6¹¥°Ä\ÛMñ)ì░@4{9\╬6·\nësf\ËaÀ#ì└S,æ\rÜ}V\Õe\Ï┌à\ß\┬\§RÑÜ)D\ý\ÔFe$9\"í>ò+r/8\ÃÜ²#ZsæG╚¥\├hò;█ï┤h\▄4[z¹\Í\Îd░┤æâ\ßqÉ8\═E:,©\0J\0\0T²?O[hXJ¨}─«¿\Ã\‗áµéÖc¼└ÿ└\¦\Ã²i\§fXT\§#-\‗®\µLdâæÄ<àAL»╣IR\▄\Ý\‗╗\ÃM\r\Ûè\Êi¡!ò\¾Aí│\´#æ\Ï\ÝÃòXnl\õû\Ê]ì·├Çƒ!CEò\╩d\¾×á\ZëN_D[{▓t,9¼	v\═\n\┬\Ó	#\\¿\‗ ð¿¼»ìF\ß└\╦yREåá¬├«z\õ\§\┼g╚Â\┬W:│l┌¬¬Ka/\ßKïPÅ \ß	\n8>¥t]2°©▄Ä\╩9à\'e\Û?åSâ\╬\▀z\n(m\ý▓5\Ì\Õiîñ8\Û)┐ïìa\'¼Eé+\Òò>┤ \▄^Ckk	`▄Âz\ÈGkçÖ│o&\┬¨+×=Å╬£ÿJMNHCFíOï¬¨ü\ÙQÑ┐iÿÆ¬ÅØè\Ï8ÙÜçpî,2,¢	U\ÙM\¯îr~®┐Xát\ÚèQ¡ä!║ea\ÌHïù\╚┴ª\¯oqp└ù+ît\¶6;y\├\ß\Ô`X\Ó3t\±ïe┴è4ûH°q\¾\¤?╬Ö$â© \Î\Ô8ó(Q▄«ry\Ò\Õ\¶ªE \0Å~\õ9¹¢sP\Ì\Ê\µI£ñ[G³Å\ÕZ²ry(r(\­+e▓\±\ß┴┌╣└\‗¿=\ýïG\0y}k++ÖtLò!!ùS\ÐsôYY@d,²\ËJNå▓▓öc+++(\ıh\ÍVV0ôÊØNòòöQì5&▓▓ï1▒\Î\ÚJî\ÕYY@ã£\0zV\Ã\▄5òòî Íàeec)_å▓▓è0ú\¸\'5òöLfO®¡d·\ÍVRÿ\╩PVVV0ë8\┼d?\Ê}\reecm² ¨\Í┌▓▓ââ[Å·OÑeehÿ\├\¸O╬│²!¼¼ºô\¯ \0f▒9^k++\n\╠@6Ä<ì \¶·\Z\╩\╩\"Ü=G³¨VùÇ©²Ü\╩\╩┴ü¦âü£uª?³½++ \r╣=\±\þ╬░\§:\╩\╩da®\08\╚\¾\Ò*ëå\0·VVQA¢_\Ú³\Ú\´VVQ1 \┘','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',128);
/*!40000 ALTER TABLE `pictures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `redemptions`
--

DROP TABLE IF EXISTS `redemptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `redemptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CouponId` int NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RedeemedDate` datetime(6) NOT NULL,
  `UserEmail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_redemptions_CouponId` (`CouponId`),
  CONSTRAINT `FK_redemptions_Coupons_CouponId` FOREIGN KEY (`CouponId`) REFERENCES `coupons` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `redemptions`
--

LOCK TABLES `redemptions` WRITE;
/*!40000 ALTER TABLE `redemptions` DISABLE KEYS */;
INSERT INTO `redemptions` VALUES (11,14,'08ddaf2a-6964-47e5-8289-e714c41cd281','2025-06-19 12:13:12.165375',NULL),(12,15,'08ddb25d-1ddf-467b-803a-18787c1aabdf','2025-07-19 10:39:39.098421','talad79903@kimdyn.com'),(14,16,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','2025-07-24 11:01:51.000000','test@example.com');
/*!40000 ALTER TABLE `redemptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OfferId` int NOT NULL,
  `RatingValue` int NOT NULL,
  `ReviewComment` longtext,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `ReviewerId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ReviewedId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_reviews_OfferId` (`OfferId`),
  KEY `IX_reviews_ReviewedId` (`ReviewedId`),
  KEY `IX_reviews_ReviewerId` (`ReviewerId`),
  CONSTRAINT `FK_reviews_offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_reviews_users_ReviewedId` FOREIGN KEY (`ReviewedId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_reviews_users_ReviewerId` FOREIGN KEY (`ReviewerId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `Name` varchar(100) NOT NULL,
  `Duration` int NOT NULL,
  `Price_Amount` decimal(18,2) DEFAULT NULL,
  `Price_Currency` varchar(3) DEFAULT NULL,
  `Description` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shopitems`
--

LOCK TABLES `shopitems` WRITE;
/*!40000 ALTER TABLE `shopitems` DISABLE KEYS */;
INSERT INTO `shopitems` VALUES (1,'test',30,0.50,'eur','testbeschreibung'),(2,'Testangebot',1,0.50,'eur','Dies ist ein Test des Shops. Der Coupon hat minimale Laufzeit und kostet den im Zahlungssystem kleinstmoeglichen Betrag.');
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
  `SkillDescrition` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ParentSkill_ID` int DEFAULT NULL,
  PRIMARY KEY (`Skill_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skills`
--

LOCK TABLES `skills` WRITE;
/*!40000 ALTER TABLE `skills` DISABLE KEYS */;
INSERT INTO `skills` VALUES (1,'Handwerk',NULL),(2,'Haushalt',NULL),(3,'Holz',1),(4,'Metall',1),(5,'Tapezieren',1),(6,'W├â┬ñnde streichen',1),(7,'Putzen',2),(8,'Aufr├â┬ñumen',2),(9,'Einkaufen',2),(10,'Kochen',2),(11,'zur Hand gehen - keine speziellen Kenntnisse notwendig',NULL),(12,'Tiersitting',NULL),(13,'Haussitting',11),(14,'Pferde',12),(15,'Kleintiere',12),(16,'V├â┬Âgel oder Amphibien',12),(17,'Gartenarbeit',2),(18,'Trockenbau',1),(19,'Estrichleger',1),(20,'Fliesen verlegen',1),(21,'Installationsarbeiten Bad/Heizung/Sanit├â┬ñr',1),(22,'Installationsarbeiten Elektro',1),(23,'Zimmermann',1),(24,'Tischler',1),(25,'Dachdecker',1),(26,'Maurer',1),(27,'KFZ Reparatur',1),(28,'Informatiker',1),(29,'Backen',2),(30,'Kreativ',NULL),(31,'Marketing',30),(32,'Social Media',30),(33,'Texter',30),(34,'Grafiker',30),(35,'Fotografen',30),(36,'Journalisten',30),(37,'Webseite - Blog schreiben',30),(38,'Finanzen/Buchhaltung',NULL),(39,'K├â┬Ârpertherapie (wie Massage, Jin Shin Jyutsu, Fu├â┼©reflex)',NULL),(40,'F├â┬╝hrerschein',NULL),(41,'F├â┬╝hrerschein PKW',40),(42,'F├â┬╝hrerschein LKW',40),(43,'Kettens├â┬ñgenschein vorhanden',1),(44,'Brennholz hacken',1);
/*!40000 ALTER TABLE `skills` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TransactionDate` datetime(6) NOT NULL,
  `Amount_Value` decimal(18,2) DEFAULT NULL,
  `Amount_Currency` varchar(3) DEFAULT NULL,
  `ShopItemId` int NOT NULL,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TransactionId` varchar(50) NOT NULL,
  `Status` int NOT NULL,
  `CouponId` int DEFAULT NULL,
  `PaymentMethod` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_transaction_CouponId` (`CouponId`),
  KEY `IX_transaction_ShopItemId` (`ShopItemId`),
  KEY `IX_transaction_UserId` (`UserId`),
  CONSTRAINT `FK_transaction_coupons_CouponId` FOREIGN KEY (`CouponId`) REFERENCES `coupons` (`Id`),
  CONSTRAINT `FK_transaction_shopitems_ShopItemId` FOREIGN KEY (`ShopItemId`) REFERENCES `shopitems` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_transaction_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (1,'2025-06-23 10:24:33.596415',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd7YDKTj2OKWdWz1xy6J0ze',0,NULL,NULL),(2,'2025-06-23 10:25:10.087784',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd7YnKTj2OKWdWz0iAeY8jj',0,NULL,NULL),(3,'2025-06-23 10:58:06.338798',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd84g4J7Rgstl9N0UyyyYHB',0,NULL,NULL),(4,'2025-06-23 11:07:48.257118',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8E44J7Rgstl9N00ea6aFz',0,NULL,NULL),(5,'2025-06-23 11:08:53.266512',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8F74J7Rgstl9N0JfoX9t3',0,NULL,NULL),(6,'2025-06-23 11:11:05.270930',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8HF4J7Rgstl9N0fcHZ2sM',0,NULL,NULL),(7,'2025-06-23 11:14:49.805882',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8Kr4J7Rgstl9N1Qytx4G9',0,NULL,NULL),(8,'2025-06-23 11:18:01.486814',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8Nx4J7Rgstl9N18fcJBcy',0,NULL,NULL),(9,'2025-06-23 11:18:22.018850',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8OH4J7Rgstl9N0OSpkWD5',0,NULL,NULL),(10,'2025-06-23 11:18:44.047775',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8Od4J7Rgstl9N1g5pfSWr',0,NULL,NULL),(11,'2025-06-23 11:22:51.629009',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8Sd4J7Rgstl9N0F9vHLKf',0,NULL,NULL),(12,'2025-06-23 11:25:08.408322',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8Uq4J7Rgstl9N0kYQVW5T',0,NULL,NULL),(13,'2025-06-23 11:45:04.293327',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rd8o84J7Rgstl9N0kdbaZO5',0,NULL,NULL),(14,'2025-07-07 11:09:27.728403',0.50,'EUR',1,'08ddb25d-1ddf-467b-803a-18787c1aabdf','pi_3RiCvL4J7Rgstl9N09bejsqK',0,NULL,NULL),(15,'2025-07-07 11:33:05.909504',0.50,'EUR',1,'08ddb25d-1ddf-467b-803a-18787c1aabdf','pi_3RiDID4J7Rgstl9N1WDqt102',0,NULL,NULL),(16,'2025-07-08 12:34:30.563812',0.50,'EUR',1,'08ddb25d-1ddf-467b-803a-18787c1aabdf','pi_3RiajC4J7Rgstl9N11S8CSob',0,NULL,NULL),(17,'2025-07-15 10:26:20.635498',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl6404J7Rgstl9N1AIobhH2',0,NULL,NULL),(18,'2025-07-15 10:27:18.413636',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl64w4J7Rgstl9N00amOUTq',0,NULL,NULL),(19,'2025-07-15 10:27:35.836494',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl65D4J7Rgstl9N1FO76cpa',0,NULL,NULL),(20,'2025-07-15 10:27:56.345731',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl65Y4J7Rgstl9N0GhHVuBf',0,NULL,NULL),(21,'2025-07-15 10:30:09.748106',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl67h4J7Rgstl9N1foohghV',0,NULL,NULL),(22,'2025-07-15 10:32:30.084049',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl69x4J7Rgstl9N0eVj4732',0,NULL,NULL),(23,'2025-07-15 10:34:49.221354',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3Rl6CD4J7Rgstl9N1Nxor7O0',0,NULL,NULL),(24,'2025-07-16 09:54:04.173674',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlS2J4J7Rgstl9N1BbI9o3k',0,NULL,NULL),(25,'2025-07-16 09:54:18.718838',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlS2Y4J7Rgstl9N1Yqs9RVW',0,NULL,NULL),(26,'2025-07-16 09:55:34.670807',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlS3m4J7Rgstl9N11Rgh8rg',0,NULL,NULL),(27,'2025-07-16 09:59:24.886245',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlS7U4J7Rgstl9N1i6HHS1J',0,NULL,NULL),(28,'2025-07-16 10:19:38.496896',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlSR44J7Rgstl9N1LeMOfr3',0,NULL,NULL),(29,'2025-07-16 10:25:37.439255',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlSWr4J7Rgstl9N08tRtyEM',0,NULL,NULL),(30,'2025-07-16 10:36:53.525563',0.50,'EUR',1,'08ddaf2a-6964-47e5-8289-e714c41cd281','pi_3RlShl4J7Rgstl9N1QV5TiX5',0,NULL,NULL);
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usermembership`
--

DROP TABLE IF EXISTS `usermembership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usermembership` (
  `UserMembershipID` int NOT NULL AUTO_INCREMENT,
  `User_Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `MembershipID` int NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`UserMembershipID`),
  KEY `IX_usermembership_MembershipID` (`MembershipID`),
  KEY `IX_usermembership_User_Id` (`User_Id`),
  CONSTRAINT `FK_usermembership_memberships_MembershipID` FOREIGN KEY (`MembershipID`) REFERENCES `memberships` (`MembershipID`) ON DELETE CASCADE,
  CONSTRAINT `FK_usermembership_users_User_Id` FOREIGN KEY (`User_Id`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usermembership`
--

LOCK TABLES `usermembership` WRITE;
/*!40000 ALTER TABLE `usermembership` DISABLE KEYS */;
INSERT INTO `usermembership` VALUES (18,'08ddaf2a-6964-47e5-8289-e714c41cd281',1,'2025-06-19 12:13:12.209086','2025-07-19 12:13:12.209203','2025-06-19 12:13:12.209526','2025-06-19 12:13:12.209641'),(19,'08ddb25d-1ddf-467b-803a-18787c1aabdf',1,'2025-07-19 10:39:39.124016','2025-08-18 10:39:39.124108','2025-07-19 10:39:39.124282','2025-07-19 10:39:39.124371'),(20,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa',1,'2025-07-24 11:12:01.000000','2025-08-23 11:12:01.000000','2025-07-24 11:12:01.000000','2025-07-24 11:12:01.000000'),(27,'22222222-2222-2222-2222-222222222222',1,'2025-07-26 18:22:17.000000','2025-08-26 18:22:17.000000','2025-07-26 18:22:17.000000','2025-07-26 18:22:17.000000'),(28,'11111111-1111-1111-1111-111111111111',1,'2025-07-26 18:22:17.000000','2025-08-26 18:22:17.000000','2025-07-26 18:22:17.000000','2025-07-26 18:22:17.000000'),(29,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa',1,'2025-07-26 18:22:17.000000','2025-08-26 18:22:17.000000','2025-07-26 18:22:17.000000','2025-07-26 18:22:17.000000'),(30,'08ddbf22-79b7-4538-8827-fe47428fadbf',1,'2025-07-26 18:22:17.000000','2025-08-26 18:22:17.000000','2025-07-26 18:22:17.000000','2025-07-26 18:22:17.000000'),(31,'test-1234',1,'2025-07-26 18:30:26.000000','2025-08-26 18:30:26.000000','2025-07-26 18:30:26.000000','2025-07-26 18:30:26.000000'),(32,'test-5678',1,'2025-07-26 18:31:54.000000','2025-08-26 18:31:54.000000','2025-07-26 18:31:54.000000','2025-07-26 18:31:54.000000'),(33,'33333333-3333-3333-3333-333333333333',1,'2025-07-26 18:33:30.000000','2025-08-26 18:33:30.000000','2025-07-26 18:33:30.000000','2025-07-26 18:33:30.000000');
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userprofiles`
--

LOCK TABLES `userprofiles` WRITE;
/*!40000 ALTER TABLE `userprofiles` DISABLE KEYS */;

INSERT INTO `userprofiles` VALUES (3,'22222222-2222-2222-2222-222222222222',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(5,'22222222-2222-2222-2222-222222222222',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(6,'11111111-1111-1111-1111-111111111111',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(7,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(8,'08ddbf22-79b7-4538-8827-fe47428fadbf',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(9,'test-1234',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(10,'test-5678',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen'),(11,'33333333-3333-3333-3333-333333333333',NULL,0,'Lesen, Schwimmen',NULL,'Programmieren, Kochen');
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
('00000000-0000-0000-0000-000000000001','Admin','Admin','1990-01-01','Male',1,'adminuser@example.com','password',NULL,0,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,1),
('11111111-1111-1111-1111-111111111111','Test','User','2000-01-01','Male',1,'testuser@example.com','testpassword','testsalt',1,1,'https://facebook.com/test','https://rs.test','https://vs.test',3,NULL,'Lesen, Schwimmen',NULL,'Programmieren, Kochen',30,0,NULL,NULL,1),
('22222222-2222-2222-2222-222222222222','Test','User','2000-01-01','Male',1,'testuser3@example.com','testpassword','testsalt',1,1,'https://facebook.com/test','https://rs.test','https://vs.test',3,NULL,'Lesen, Schwimmen',NULL,'Programmieren, Kochen',0,0,NULL,NULL,1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-28 15:56:53

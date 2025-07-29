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
  `UserId` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `FirstName` longtext COLLATE utf8mb4_unicode_ci,
  `LastName` longtext COLLATE utf8mb4_unicode_ci,
  `Gender` longtext COLLATE utf8mb4_unicode_ci,
  `DateOfBirth` datetime(6) DEFAULT NULL,
  `Email` longtext COLLATE utf8mb4_unicode_ci,
  `Skills` longtext COLLATE utf8mb4_unicode_ci,
  `Hobbies` longtext COLLATE utf8mb4_unicode_ci,
  `Address` longtext COLLATE utf8mb4_unicode_ci,
  `Latitude` double DEFAULT NULL,
  `Longitude` double DEFAULT NULL,
  `ProfilePicture` longtext COLLATE utf8mb4_unicode_ci,
  `DeletedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `MigrationId` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ProductVersion` varchar(32) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `Name` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `NameAccommodationType` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accomodations`
--

LOCK TABLES `accomodations` WRITE;
/*!40000 ALTER TABLE `accomodations` DISABLE KEYS */;
INSERT INTO `accomodations` VALUES (1,'HÃ¼tte'),(4,'Zelt'),(5,'Wohnmobil'),(6,'Zimmer'),(7,'Bauwagen'),(8,'Tiny House'),(9,'Boot'),(10,'Baumhaus'),(11,'Scheune'),(12,'Wohnwagen'),(13,'Scheune'),(14,'Wohnwagen');
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
  `DisplayName` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `HouseNumber` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Road` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Suburb` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `City` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `County` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `State` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Postcode` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Country` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `CountryCode` varchar(10) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `OsmId` bigint DEFAULT NULL,
  `OsmType` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `PlaceId` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Type` int DEFAULT '0',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addresses`
--

LOCK TABLES `addresses` WRITE;
/*!40000 ALTER TABLE `addresses` DISABLE KEYS */;
INSERT INTO `addresses` VALUES (1,52.520008,13.404954,'Brandenburger Tor, Pariser Platz, 10117 Berlin, Deutschland',NULL,NULL,NULL,'Berlin',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:30:26',NULL),(2,48.137154,11.576124,'Marienplatz, 80331 MÃ¼nchen, Deutschland',NULL,NULL,NULL,'MÃ¼nchen',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:31:54',NULL),(3,50.110924,8.682127,'RÃ¶merberg, 60311 Frankfurt am Main, Deutschland',NULL,NULL,NULL,'Frankfurt',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:33:30',NULL),(32,50.9924496,9.7711081,'Hinter den ZÃ¤unen, Lispenhausen, 36199, Deutschland',NULL,NULL,NULL,'Lispenhausen',NULL,NULL,'36199','Deutschland',NULL,NULL,NULL,NULL,0,'2025-07-28 08:33:00',NULL),(33,52.3490825,9.7259069,'Nettemannstraße, Hannover, 30459, Deutschland',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,'2025-07-29 14:29:59',NULL);
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
  `Code` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Name` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Description` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `CreatedBy` char(36) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `Duration` int NOT NULL DEFAULT '0',
  `MembershipId` int NOT NULL DEFAULT '0',
  `IsEmailSent` tinyint(1) NOT NULL DEFAULT '0',
  `EmailSentDate` datetime DEFAULT NULL,
  `EmailSentTo` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_coupons_CreatedBy` (`CreatedBy`),
  KEY `IX_coupons_MembershipId` (`MembershipId`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `coupons`
--

LOCK TABLES `coupons` WRITE;
/*!40000 ALTER TABLE `coupons` DISABLE KEYS */;
INSERT INTO `coupons` VALUES (1,'WELCOME2024','Welcome Coupon','Welcome discount for new users','2025-07-29 13:59:25.000000','18ddca8e-a8b9-48ef-8e0b-09db16414bfb',365,1,0,NULL,NULL),(2,'PREMIUM50','Premium Discount','50% off premium membership','2025-07-29 13:59:25.000000','18ddca8e-a8b9-48ef-8e0b-09db16414bfb',365,4,0,NULL,NULL),(20,'01K1B1132RNT9RSFNZM2PYF1W8','Admin Issued Coupon','','2025-07-29 12:03:01.848663','08dcd23c-d4eb-456b-87e4-73837709fada',365,4,1,'2025-07-29 14:38:42','sturmiechen@googlemail.com'),(21,'01K1B3EN3SMBF6A4RW5J2D379E','Admin Issued Coupon','','2025-07-29 12:45:23.452276','08dcd23c-d4eb-456b-87e4-73837709fada',365,1,1,'2025-07-29 14:45:31','sturmiechen@googlemail.com'),(22,'01K1B3SN4TN2RHNREDJCZG0D1R','Admin Issued Coupon','','2025-07-29 12:51:23.931018','08dcd23c-d4eb-456b-87e4-73837709fada',365,4,1,'2025-07-29 14:51:30','sturmiechen@googlemail.com'),(23,'01K1B479FETPHZ3JCTRB72NA6M','Admin Issued Coupon','','2025-07-29 12:58:50.738808','08dcd23c-d4eb-456b-87e4-73837709fada',365,1,1,'2025-07-29 14:58:57','sturmiechen@googlemail.com');
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
  `user_Id` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `verificationToken` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `requestDate` datetime(6) NOT NULL,
  PRIMARY KEY (`verificationId`)
) ENGINE=InnoDB AUTO_INCREMENT=83 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailverificators`
--

LOCK TABLES `emailverificators` WRITE;
/*!40000 ALTER TABLE `emailverificators` DISABLE KEYS */;
INSERT INTO `emailverificators` VALUES (82,'08ddceac-65c4-4a92-88d5-b25948ce7352','83fad137-4d73-4ee2-b5ab-886d820b9e3c','2025-07-29 14:29:59.541788');
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
  `Description` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `DurationDays` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '0',
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT '',
  `Price` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `memberships`
--

LOCK TABLES `memberships` WRITE;
/*!40000 ALTER TABLE `memberships` DISABLE KEYS */;
INSERT INTO `memberships` VALUES (1,'Flexible Mitgliedschaft fÃ¼r 1 Monat - perfekt zum Ausprobieren',30,1,'1 Monat',9.990000000000000000000000000000),(2,'N/A',0,0,'Default',0.000000000000000000000000000000),(3,'3 Monate Mitgliedschaft mit attraktivem Rabatt',90,1,'3 Monate',24.990000000000000000000000000000),(4,'Jahresmitgliedschaft - unser beliebtestes Angebot',365,1,'1 Jahr',89.990000000000000000000000000000),(5,'3 Jahre Mitgliedschaft mit maximalem Rabatt',1095,1,'3 Jahre',239.990000000000000000000000000000),(6,'Lebenslange Mitgliedschaft - einmal zahlen, fÃ¼r immer dabei',9999,1,'Lebenslang',499.990000000000000000000000000000);
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
  `UserId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `HostId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Status` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_offerapplication_OfferId` (`OfferId`),
  KEY `IX_offerapplication_UserId` (`UserId`),
  KEY `IX_offerapplication_HostId` (`HostId`)
) ENGINE=InnoDB AUTO_INCREMENT=132 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offerapplication`
--

LOCK TABLES `offerapplication` WRITE;
/*!40000 ALTER TABLE `offerapplication` DISABLE KEYS */;
INSERT INTO `offerapplication` VALUES (1,1,'28ddca8e-a8b9-48ef-8e0b-09db16414bfc','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',0,'2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000'),(131,128,'08dcd23c-d4eb-45db-87e4-73837709fada','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',1,'2025-07-29 13:17:41.346379','2025-07-29 13:19:18.913326');
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
  `UserId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `CreatedAt` date NOT NULL,
  `Discriminator` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offers`
--

LOCK TABLES `offers` WRITE;
/*!40000 ALTER TABLE `offers` DISABLE KEYS */;
INSERT INTO `offers` VALUES (125,'Ferienhaus am See','SchÃ¶nes Ferienhaus direkt am See mit Bootsverleih',NULL,'Familie mit Kindern',NULL,NULL,'WÃ¤nde streichen, Gartenarbeit','Mindestens 2 Wochen Aufenthalt',NULL,NULL,'08dcd23c-d4eb-456b-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',4,0,'2025-07-27',1,0,'2025-08-26',NULL),(126,'Alpine HÃ¼tte','Traditionelle BerghÃ¼tte mit Panoramablick',NULL,'Wanderer, Bergsteiger',NULL,NULL,'Holz hacken, Kamin reinigen','Erfahrung im Umgang mit HolzÃ¶fen',NULL,NULL,'08dcd23c-d4eb-45db-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',6,1,'2025-07-27',1,0,'2025-09-10',NULL),(127,'Stadtwohnung Zentrum','Moderne Wohnung im Herzen der Stadt',NULL,'Paare, Singles',NULL,NULL,'Putzen, Einkaufen, Haustiere versorgen','Keine Haustiere erlaubt',NULL,NULL,'08dcd23c-d4eb-45db-88e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',2,2,'2025-07-27',1,0,'2025-08-16',NULL),(128,'Mein tolles Haus','Kommt mich besuchen',NULL,'','Boot',NULL,'Metall','Senioren, Gruppen',NULL,NULL,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','2025-07-27','OfferTypeLodging','2025-01-01',0,0,'0001-01-01',0,0,'2025-08-08',31);
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
  `user_Id` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Token` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `requestDate` datetime(6) NOT NULL,
  PRIMARY KEY (`TokenId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `Hash` longtext COLLATE utf8mb4_unicode_ci,
  `Width` int NOT NULL,
  `ImageData` longblob NOT NULL,
  `UserId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `OfferId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_pictures_UserId` (`UserId`),
  KEY `IX_pictures_OfferId` (`OfferId`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pictures`
--

LOCK TABLES `pictures` WRITE;
/*!40000 ALTER TABLE `pictures` DISABLE KEYS */;
INSERT INTO `pictures` VALUES (1,'hash1',800,_binary '�PNG\r\n\Z\n\0\0\0\rIHDR\0\0\02\0\0\02\0\0\0?��\0\0\0	pHYs\0\0\0\0\0��\0\0\0tIME\�;;;;\0\0\0tEXtComment\0Created with Tkinter\0\0\0\0IEND�B`�','08ddca8e-a8b9-48ef-8e0b-09db16414bfa',1),(2,'hash2',800,_binary '�PNG\r\n\Z\n\0\0\0\rIHDR\0\0\02\0\0\02\0\0\0?��\0\0\0	pHYs\0\0\0\0\0��\0\0\0tIME\�;;;;\0\0\0tEXtComment\0Created with Tkinter\0\0\0\0IEND�B`�','18ddca8e-a8b9-48ef-8e0b-09db16414bfb',2);
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
  `UserId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `RedeemedDate` datetime(6) NOT NULL,
  `UserEmail` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_redemptions_CouponId` (`CouponId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `redemptions`
--

LOCK TABLES `redemptions` WRITE;
/*!40000 ALTER TABLE `redemptions` DISABLE KEYS */;
INSERT INTO `redemptions` VALUES (1,1,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','2025-07-29 13:59:25.000000','test@example.com');
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
  `ReviewComment` longtext COLLATE utf8mb4_unicode_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `ReviewerId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ReviewedId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_reviews_OfferId` (`OfferId`),
  KEY `IX_reviews_ReviewerId` (`ReviewerId`),
  KEY `IX_reviews_ReviewedId` (`ReviewedId`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (1,1,5,'Sehr netter Gastgeber und schÃ¶ne Unterkunft!','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000','28ddca8e-a8b9-48ef-8e0b-09db16414bfc','08ddca8e-a8b9-48ef-8e0b-09db16414bfa'),(2,3,4,'Gute Erfahrung, gerne wieder!','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000','08ddca8e-a8b9-48ef-8e0b-09db16414bfa','28ddca8e-a8b9-48ef-8e0b-09db16414bfc'),(70,128,5,'Das war so toll!','2025-07-29 13:19:27.681396','2025-07-29 13:19:27.681434','08ddca8e-a8b9-48ef-8e0b-09db16414bfa','08dcd23c-d4eb-45db-87e4-73837709fada'),(71,128,3,'War ganz ok','2025-07-29 13:32:38.380237','2025-07-29 13:32:38.380278','08dcd23c-d4eb-45db-87e4-73837709fada','08ddca8e-a8b9-48ef-8e0b-09db16414bfa');
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
  `Name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Duration` int NOT NULL,
  `Price_Amount` decimal(18,2) DEFAULT NULL,
  `Price_Currency` varchar(3) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Description` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `SkillDescrition` longtext COLLATE utf8mb4_unicode_ci,
  `ParentSkill_ID` int DEFAULT NULL,
  PRIMARY KEY (`Skill_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skills`
--

LOCK TABLES `skills` WRITE;
/*!40000 ALTER TABLE `skills` DISABLE KEYS */;
INSERT INTO `skills` VALUES (1,'Handwerk',NULL),(2,'Haushalt',NULL),(3,'Holz',1),(4,'Metall',1),(5,'Tapezieren',1),(6,'WÃ¤nde streichen',1),(7,'Putzen',2),(8,'AufrÃ¤umen',2),(9,'Einkaufen',2),(10,'Kochen',2),(11,'zur Hand gehen - keine speziellen Kenntnisse notwendig',NULL),(12,'Tiersitting',NULL),(13,'Haussitting',11),(14,'Pferde',12),(15,'Kleintiere',12),(16,'VÃ¶gel oder Amphibien',12),(17,'Gartenarbeit',2),(18,'Trockenbau',1),(19,'Estrichleger',1),(20,'Fliesen verlegen',1),(21,'Installationsarbeiten Bad/Heizung/SanitÃ¤r',1),(22,'Installationsarbeiten Elektro',1),(23,'Zimmermann',1),(24,'Tischler',1),(25,'Dachdecker',1),(26,'Maurer',1),(27,'KFZ Reparatur',1),(28,'Informatiker',1),(29,'Backen',2),(30,'Kreativ',NULL),(31,'Marketing',30),(32,'Social Media',30),(33,'Texter',30),(34,'Grafiker',30),(35,'Fotografen',30),(36,'Journalisten',30),(37,'Webseite - Blog schreiben',30),(38,'Finanzen/Buchhaltung',NULL),(39,'KÃ¶rpertherapie (wie Massage, Jin Shin Jyutsu, FuÃŸreflex)',NULL),(40,'FÃ¼hrerschein',NULL),(41,'FÃ¼hrerschein PKW',40),(42,'FÃ¼hrerschein LKW',40),(43,'KettensÃ¤genschein vorhanden',1),(44,'Brennholz hacken',1);
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
  `Amount_Currency` varchar(3) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `ShopItemId` int NOT NULL,
  `UserId` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `TransactionId` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Status` int NOT NULL,
  `CouponId` int DEFAULT NULL,
  `PaymentMethod` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_transaction_ShopItemId` (`ShopItemId`),
  KEY `IX_transaction_UserId` (`UserId`),
  KEY `IX_transaction_CouponId` (`CouponId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (1,'2025-07-29 13:59:25.000000',0.50,'EUR',1,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','txn_test_001',1,NULL,NULL),(2,'2025-07-29 13:59:25.000000',0.50,'EUR',2,'18ddca8e-a8b9-48ef-8e0b-09db16414bfb','txn_test_002',1,NULL,NULL);
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
  `User_Id` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `MembershipID` int NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `Expiration` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `UpdatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`UserMembershipID`),
  KEY `IX_usermembership_User_Id` (`User_Id`),
  KEY `IX_usermembership_MembershipID` (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usermembership`
--

LOCK TABLES `usermembership` WRITE;
/*!40000 ALTER TABLE `usermembership` DISABLE KEYS */;
INSERT INTO `usermembership` VALUES (1,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa',1,'2025-07-29 13:59:25.000000','2025-08-28 13:59:25.000000','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000'),(2,'08dcd23c-d4eb-456b-87e4-73837709fada',4,'2025-07-29 13:59:25.000000','2026-07-29 13:59:25.000000','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000'),(3,'08dcd23c-d4eb-45db-87e4-73837709fada',1,'2025-07-29 13:59:25.000000','2025-08-28 13:59:25.000000','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000'),(4,'08dcd23c-d4eb-45db-88e4-73837709fada',1,'2025-07-29 13:59:25.000000','2025-08-28 13:59:25.000000','2025-07-29 13:59:25.000000','2025-07-29 13:59:25.000000');
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
  `User_Id` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `UserPic` longblob,
  `Options` int NOT NULL,
  `Hobbies` longtext COLLATE utf8mb4_unicode_ci,
  `Token` longtext COLLATE utf8mb4_unicode_ci,
  `Skills` longtext COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_userprofiles_User_Id` (`User_Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
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
  `User_Id` char(36) COLLATE utf8mb4_unicode_ci NOT NULL,
  `FirstName` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `LastName` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `DateOfBirth` date NOT NULL,
  `Gender` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `AddressId` int DEFAULT NULL,
  `Email_Address` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `Password` longtext COLLATE utf8mb4_unicode_ci NOT NULL,
  `SaltKey` longtext COLLATE utf8mb4_unicode_ci,
  `IsTwoFactorEnabled` tinyint(1) NOT NULL DEFAULT '0',
  `TwoFactorSecret` longtext COLLATE utf8mb4_unicode_ci,
  `BackupCodes` longtext COLLATE utf8mb4_unicode_ci,
  `FailedBackupCodeAttempts` int NOT NULL DEFAULT '0',
  `LastFailedBackupCodeAttempt` datetime DEFAULT NULL,
  `IsBackupCodeLocked` tinyint(1) NOT NULL DEFAULT '0',
  `IsEmailVerified` tinyint(1) NOT NULL DEFAULT '0',
  `MembershipId` int DEFAULT NULL,
  `Facebook_link` longtext COLLATE utf8mb4_unicode_ci,
  `Link_RS` longtext COLLATE utf8mb4_unicode_ci,
  `Link_VS` longtext COLLATE utf8mb4_unicode_ci,
  `Hobbies` longtext COLLATE utf8mb4_unicode_ci,
  `Skills` longtext COLLATE utf8mb4_unicode_ci,
  `ProfilePicture` longblob,
  `About` longtext COLLATE utf8mb4_unicode_ci,
  `VerificationState` int NOT NULL DEFAULT '0',
  `UserRole` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`User_Id`),
  KEY `IX_users_AddressId` (`AddressId`),
  KEY `IX_users_MembershipId` (`MembershipId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES ('08dcd23c-d4eb-456b-87e4-73837709fada','Admin','User','1985-05-15','Male',2,'admin@example.com','$2a$11$rQZ9N0NpqN0NpqN0NpqN0O','testsalt',1,'JBSWY3DPEHPK3PXP','[\"12345678\",\"87654321\",\"22222222\",\"33333333\",\"44444444\",\"55555555\",\"66666666\",\"77777777\"]',0,NULL,0,1,4,NULL,NULL,NULL,NULL,NULL,NULL,NULL,3,1),('08dcd23c-d4eb-45db-87e4-73837709fada','John','Doe','1992-03-20','Male',3,'john@example.com','$2a$11$rQZ9N0NpqN0NpqN0NpqN0O','testsalt',0,NULL,NULL,0,NULL,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,3,0),('08dcd23c-d4eb-45db-88e4-73837709fada','Jane','Smith','1988-07-12','Female',NULL,'jane@example.com','$2a$11$rQZ9N0NpqN0NpqN0NpqN0O','testsalt',0,NULL,NULL,0,NULL,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,3,0),('08ddca8e-a8b9-48ef-8e0b-09db16414bfa','Test','User','1990-01-01','Other',1,'test@example.com','$2a$11$rQZ9N0NpqN0NpqN0NpqN0O','testsalt',0,NULL,NULL,0,NULL,0,1,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,3,0),('08ddceac-65c4-4a92-88d5-b25948ce7352','Anna','Lena','1994-08-11','Female',33,'sturmiechen@googlemail.com','eN3SjpYME3iynqzegzQ+L10fpa/yNWjlt1BkEbKWrKA=','TDbcZa933XLjU/HV4i9iD1gAG7wGK+RQKaZwHAX18pQ=',0,NULL,NULL,0,NULL,0,0,NULL,'https://www.facebook.com/lena.fehlhaber/','','',NULL,NULL,NULL,NULL,0,0);
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

-- Dump completed on 2025-07-29 17:05:05

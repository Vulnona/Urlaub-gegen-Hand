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
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--


--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accommodationsuitables`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accomodations`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emailverificators`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offerapplication`
--


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
  `GroupProperties` longtext NOT NULL,
  `AdditionalLodgingProperties` longtext,
  `LodgingType` longtext,
  `Skills` longtext NOT NULL,
  `Requirements` longtext,
  `SpecialConditions` longtext,
  `PossibleLocations` longtext,
  `UserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreatedAt` date NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FromDate` date NOT NULL DEFAULT '0001-01-01',
  `GroupSize` int NOT NULL DEFAULT '0',
  `Mobility` int NOT NULL DEFAULT '0',
  `ModifiedAt` date NOT NULL DEFAULT '0001-01-01',
  `OfferType` int NOT NULL DEFAULT '0',
  `PictureId` int DEFAULT NULL,
  `Status` int NOT NULL DEFAULT '0',
  `ToDate` date NOT NULL DEFAULT '0001-01-01',
  PRIMARY KEY (`Id`),
  KEY `IX_offers_UserId` (`UserId`),
  KEY `IX_offers_PictureId` (`PictureId`),
  CONSTRAINT `FK_offers_pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `pictures` (`Id`),
  CONSTRAINT `FK_offers_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=111 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offers`
--


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
  PRIMARY KEY (`Id`),
  KEY `IX_pictures_UserId` (`UserId`),
  CONSTRAINT `FK_pictures_users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`User_Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pictures`
--


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


--
-- Table structure for table `skills`
--

DROP TABLE IF EXISTS `skills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `skills` (
  `Skill_ID` int NOT NULL AUTO_INCREMENT,
  `SkillDescrition` longtext NOT NULL,
  `ParentSkill_ID` int DEFAULT NULL,
  PRIMARY KEY (`Skill_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skills`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usermembership`
--


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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userprofiles`
--


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
  PRIMARY KEY (`User_Id`),
  KEY `IX_users_MembershipId` (`MembershipId`),
  CONSTRAINT `FK_users_memberships_MembershipId` FOREIGN KEY (`MembershipId`) REFERENCES `memberships` (`MembershipID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-24 17:56:30

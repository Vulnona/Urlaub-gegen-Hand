-- UGH Database Initialization Script
-- Based on complete MySQL Dump

-- Drop existing tables if they exist (for clean initialization)
DROP TABLE IF EXISTS `reviews`;
DROP TABLE IF EXISTS `offerapplication`;
DROP TABLE IF EXISTS `pictures`;
DROP TABLE IF EXISTS `offers`;
DROP TABLE IF EXISTS `usermembership`;
DROP TABLE IF EXISTS `memberships`;
DROP TABLE IF EXISTS `users`;
DROP TABLE IF EXISTS `addresses`;
DROP TABLE IF EXISTS `shopitems`;
DROP TABLE IF EXISTS `coupons`;
DROP TABLE IF EXISTS `redemptions`;
DROP TABLE IF EXISTS `transaction`;
DROP TABLE IF EXISTS `skills`;
DROP TABLE IF EXISTS `passwordresettokens`;
DROP TABLE IF EXISTS `emailverificators`;
DROP TABLE IF EXISTS `userprofiles`;
DROP TABLE IF EXISTS `DeletedUserBackups`;
DROP TABLE IF EXISTS `accommodationsuitables`;
DROP TABLE IF EXISTS `accomodations`;
DROP TABLE IF EXISTS `__EFMigrationsHistory`;

-- Create DeletedUserBackups table
CREATE TABLE `DeletedUserBackups` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` longtext NOT NULL,
    `FirstName` longtext,
    `LastName` longtext,
    `Gender` longtext,
    `DateOfBirth` datetime(6) DEFAULT NULL,
    `Email` longtext,
    `Skills` longtext,
    `Hobbies` longtext,
    `Address` longtext,
    `Latitude` double DEFAULT NULL,
    `Longitude` double DEFAULT NULL,
    `ProfilePicture` longtext,
    `DeletedAt` datetime(6) NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create __EFMigrationsHistory table
CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create accommodationsuitables table
CREATE TABLE `accommodationsuitables` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create accomodations table
CREATE TABLE `accomodations` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `NameAccommodationType` longtext NOT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create addresses table
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
    `Type` int DEFAULT 0,
    `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` datetime DEFAULT NULL,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create memberships table
CREATE TABLE `memberships` (
    `MembershipID` int NOT NULL AUTO_INCREMENT,
    `Description` longtext NOT NULL,
    `DurationDays` int NOT NULL,
    `IsActive` tinyint(1) NOT NULL DEFAULT 0,
    `Name` varchar(100) NOT NULL DEFAULT '',
    `Price` decimal(65,30) NOT NULL DEFAULT 0.000000000000000000000000000000,
    PRIMARY KEY (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create users table
CREATE TABLE `users` (
    `User_Id` char(36) NOT NULL,
    `FirstName` longtext NOT NULL,
    `LastName` longtext NOT NULL,
    `DateOfBirth` date NOT NULL,
    `Gender` longtext NOT NULL,
    `AddressId` int NULL,
    `Email_Address` longtext NOT NULL,
    `Password` longtext NOT NULL,
    `SaltKey` longtext NULL,
    `IsTwoFactorEnabled` tinyint(1) NOT NULL DEFAULT 0,
    `TwoFactorSecret` longtext NULL,
    `BackupCodes` longtext NULL,
    `FailedBackupCodeAttempts` int NOT NULL DEFAULT 0,
    `LastFailedBackupCodeAttempt` datetime NULL,
    `IsBackupCodeLocked` tinyint(1) NOT NULL DEFAULT 0,
    `IsEmailVerified` tinyint(1) NOT NULL DEFAULT 0,
    `MembershipId` int NOT NULL DEFAULT 1,
    `Facebook_link` longtext NULL,
    `Link_RS` longtext NULL,
    `Link_VS` longtext NULL,
    `Hobbies` longtext NULL,
    `Skills` longtext NULL,
    `ProfilePicture` longblob NULL,
    `About` longtext NULL,
    `VerificationState` int NOT NULL DEFAULT 0,
    `UserRole` int NOT NULL DEFAULT 0,
    PRIMARY KEY (`User_Id`),
    INDEX `IX_users_AddressId` (`AddressId`),
    INDEX `IX_users_MembershipId` (`MembershipId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create usermembership table
CREATE TABLE `usermembership` (
    `UserMembershipID` int NOT NULL AUTO_INCREMENT,
    `User_Id` char(36) NOT NULL,
    `MembershipID` int NOT NULL,
    `StartDate` datetime(6) NOT NULL,
    `Expiration` datetime(6) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `UpdatedAt` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`UserMembershipID`),
    INDEX `IX_usermembership_User_Id` (`User_Id`),
    INDEX `IX_usermembership_MembershipID` (`MembershipID`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create offers table
CREATE TABLE `offers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `Location` longtext NULL,
    `GroupProperties` longtext NOT NULL,
    `AdditionalLodgingProperties` longtext NULL,
    `LodgingType` longtext NULL,
    `Skills` longtext NOT NULL,
    `Requirements` longtext NULL,
    `SpecialConditions` longtext NULL,
    `PossibleLocations` longtext NULL,
    `UserId` char(36) NOT NULL,
    `CreatedAt` date NOT NULL,
    `Discriminator` longtext NOT NULL,
    `FromDate` date NOT NULL DEFAULT '0001-01-01',
    `GroupSize` int NOT NULL DEFAULT 0,
    `Mobility` int NOT NULL DEFAULT 0,
    `ModifiedAt` date NOT NULL DEFAULT '0001-01-01',
    `OfferType` int NOT NULL DEFAULT 0,
    `Status` int NOT NULL DEFAULT 0,
    `ToDate` date NOT NULL DEFAULT '0001-01-01',
    `AddressId` int DEFAULT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_offers_UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create offerapplication table
CREATE TABLE `offerapplication` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OfferId` int NOT NULL,
    `UserId` char(36) NOT NULL,
    `HostId` char(36) NOT NULL,
    `Status` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_offerapplication_OfferId` (`OfferId`),
    INDEX `IX_offerapplication_UserId` (`UserId`),
    INDEX `IX_offerapplication_HostId` (`HostId`)
) ENGINE=InnoDB AUTO_INCREMENT=131 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create pictures table
CREATE TABLE `pictures` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Hash` longtext NULL,
    `Width` int NOT NULL,
    `ImageData` longblob NOT NULL,
    `UserId` char(36) NOT NULL,
    `OfferId` int DEFAULT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_pictures_UserId` (`UserId`),
    INDEX `IX_pictures_OfferId` (`OfferId`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create reviews table
CREATE TABLE `reviews` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OfferId` int NULL,
    `RatingValue` int NOT NULL,
    `ReviewComment` longtext NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `UpdatedAt` datetime(6) NOT NULL,
    `ReviewerId` char(36) NOT NULL,
    `ReviewedId` char(36) NOT NULL,
    `ReviewerFirstName` longtext NULL,
    `ReviewerLastName` longtext NULL,
    `ReviewerEmail` longtext NULL,
    `IsVisible` tinyint(1) NOT NULL DEFAULT 0,
    `VisibilityDate` datetime(6) NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_reviews_OfferId` (`OfferId`),
    INDEX `IX_reviews_ReviewerId` (`ReviewerId`),
    INDEX `IX_reviews_ReviewedId` (`ReviewedId`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create shopitems table
CREATE TABLE `shopitems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `Duration` int NOT NULL,
    `Price_Amount` decimal(18,2) DEFAULT NULL,
    `Price_Currency` varchar(3) DEFAULT NULL,
    `Description` longtext,
    PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create coupons table
CREATE TABLE `coupons` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Code` longtext NOT NULL,
    `Name` longtext NOT NULL,
    `Description` longtext NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `CreatedBy` char(36) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    `Duration` int NOT NULL DEFAULT 0,
    `MembershipId` int NOT NULL DEFAULT 0,
    `IsEmailSent` tinyint(1) NOT NULL DEFAULT 0,
    `EmailSentDate` datetime DEFAULT NULL,
    `EmailSentTo` varchar(255) DEFAULT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_coupons_CreatedBy` (`CreatedBy`),
    INDEX `IX_coupons_MembershipId` (`MembershipId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create redemptions table
CREATE TABLE `redemptions` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `CouponId` int NOT NULL,
    `UserId` char(36) NOT NULL,
    `RedeemedDate` datetime(6) NOT NULL,
    `UserEmail` longtext,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `IX_redemptions_CouponId` (`CouponId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create transaction table
CREATE TABLE `transaction` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `TransactionDate` datetime(6) NOT NULL,
    `Amount_Value` decimal(18,2) DEFAULT NULL,
    `Amount_Currency` varchar(3) DEFAULT NULL,
    `ShopItemId` int NOT NULL,
    `UserId` char(36) NOT NULL,
    `TransactionId` varchar(50) NOT NULL,
    `Status` int NOT NULL,
    `CouponId` int DEFAULT NULL,
    `PaymentMethod` longtext,
    PRIMARY KEY (`Id`),
    INDEX `IX_transaction_ShopItemId` (`ShopItemId`),
    INDEX `IX_transaction_UserId` (`UserId`),
    INDEX `IX_transaction_CouponId` (`CouponId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create skills table
CREATE TABLE `skills` (
    `Skill_ID` int NOT NULL AUTO_INCREMENT,
    `SkillDescrition` longtext,
    `ParentSkill_ID` int DEFAULT NULL,
    PRIMARY KEY (`Skill_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=141 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create passwordresettokens table
CREATE TABLE `passwordresettokens` (
    `TokenId` int NOT NULL AUTO_INCREMENT,
    `user_Id` char(36) NOT NULL,
    `Token` char(36) NOT NULL,
    `requestDate` datetime(6) NOT NULL,
    PRIMARY KEY (`TokenId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create emailverificators table
CREATE TABLE `emailverificators` (
    `verificationId` int NOT NULL AUTO_INCREMENT,
    `user_Id` char(36) NOT NULL,
    `verificationToken` char(36) NOT NULL,
    `requestDate` datetime(6) NOT NULL,
    PRIMARY KEY (`verificationId`)
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create userprofiles table
CREATE TABLE `userprofiles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `User_Id` char(36) NOT NULL,
    `UserPic` longblob,
    `Options` int NOT NULL,
    `Hobbies` longtext,
    `Token` longtext,
    `Skills` longtext,
    PRIMARY KEY (`Id`),
    INDEX `IX_userprofiles_User_Id` (`User_Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert sample data

-- Insert EF Migrations History
INSERT INTO `__EFMigrationsHistory` VALUES 
('20241016071620_InitialMigration','7.0.0'),
('20250725223915_InitialCreate','7.0.20'),
('20250725231705_RemoveCountryFields','7.0.20'),
('20250726154812_AddDeletedUserBackup','7.0.20'),
('20250727164854_AddOfferIdToPictures','7.0.20'),
('20250730150000_MakeOfferIdOptionalInReviews','7.0.20'),
('20250731182740_AddReviewVisibilityAndReviewerInfo','7.0.20');

-- Insert accommodationsuitables
INSERT INTO `accommodationsuitables` VALUES 
(3,'Kinderfreundlich'),
(4,'Hundehalter'),
(5,'Alleinreisend'),
(6,'Paare'),
(7,'Senioren'),
(8,'Gruppen'),
(9,'Barrierefrei');

-- Insert accomodations
INSERT INTO `accomodations` VALUES 
(1,'Hütte'),
(4,'Zelt'),
(5,'Wohnmobil'),
(6,'Zimmer'),
(7,'Bauwagen'),
(8,'Tiny House'),
(9,'Boot'),
(10,'Baumhaus'),
(11,'Scheune'),
(12,'Wohnwagen'),
(15,'Ferienwohnung');

-- Insert addresses
INSERT INTO `addresses` VALUES 
(1,52.520008,13.404954,'Brandenburger Tor, Pariser Platz, 10117 Berlin, Deutschland',NULL,NULL,NULL,'Berlin',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:30:26',NULL),
(2,48.137154,11.576124,'Marienplatz, 80331 München, Deutschland',NULL,NULL,NULL,'München',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:31:54',NULL),
(3,50.110924,8.682127,'Römerberg, 60311 Frankfurt am Main, Deutschland',NULL,NULL,NULL,'Frankfurt',NULL,NULL,NULL,'Deutschland','DE',NULL,NULL,NULL,0,'2025-07-26 18:33:30',NULL),
(32,50.9924496,9.7711081,'Hinter den Zäunen, Lispenhausen, 36199, Deutschland',NULL,NULL,NULL,'Lispenhausen',NULL,NULL,'36199','Deutschland',NULL,NULL,NULL,NULL,0,'2025-07-28 08:33:00',NULL);

-- Entferne Default und Test-Mitgliedschaften aus memberships
INSERT INTO `memberships` (`MembershipID`, `Description`, `DurationDays`, `IsActive`, `Name`, `Price`) VALUES
(1,'Flexible Mitgliedschaft für 1 Monat - perfekt zum Ausprobieren',30,1,'1 Monat',9.99),
(3,'3 Monate Mitgliedschaft mit attraktivem Rabatt',90,1,'3 Monate',24.99),
(4,'Jahresmitgliedschaft - unser beliebtestes Angebot',365,1,'1 Jahr',89.99),
(5,'3 Jahre Mitgliedschaft mit maximalem Rabatt',1095,1,'3 Jahre',239.99),
(6,'Lebenslange Mitgliedschaft - einmal zahlen, für immer dabei',9999,1,'Lebenslang',499.99);

-- Insert users (VerificationState: 0=IsNew, 1=VerificationPending, 2=VerificationFailed, 3=Verified)
-- UserRole: 0=User, 1=Admin
-- IsTwoFactorEnabled: 0=Disabled, 1=Enabled
INSERT INTO `users` (`User_Id`, `FirstName`, `LastName`, `DateOfBirth`, `Gender`, `AddressId`, `Email_Address`, `Password`, `SaltKey`, `IsEmailVerified`, `MembershipId`, `VerificationState`, `UserRole`, `IsTwoFactorEnabled`, `TwoFactorSecret`, `BackupCodes`) VALUES
('08ddca8e-a8b9-48ef-8e0b-09db16414bfa', 'Test', 'User', '1990-01-01', 'Other', 1, 'test@example.com', 'TtAMIGRlcJiwjfv4TTJAQPug220cJQ8z+0UY1s9+twM=', 'dGVzdHNhbHQ=', 1, 1, 3, 0, 0, NULL, NULL),
('08dcd23c-d4eb-456b-87e4-73837709fada', 'Admin', 'User', '1985-05-15', 'Male', 2, 'admin@example.com', 'TtAMIGRlcJiwjfv4TTJAQPug220cJQ8z+0UY1s9+twM=', 'dGVzdHNhbHQ=', 1, 4, 3, 1, 1, 'JBSWY3DPEHPK3PXP', '["12345678", "87654321", "11111111", "22222222", "33333333", "44444444", "55555555", "66666666", "77777777", "88888888"]'),
('08dcd23c-d4eb-45db-87e4-73837709fada', 'John', 'Doe', '1992-03-20', 'Male', 3, 'john@example.com', 'TtAMIGRlcJiwjfv4TTJAQPug220cJQ8z+0UY1s9+twM=', 'dGVzdHNhbHQ=', 1, 1, 3, 0, 0, NULL, NULL),
('08dcd23c-d4eb-45db-88e4-73837709fada', 'Jane', 'Smith', '1988-07-12', 'Female', NULL, 'jane@example.com', 'TtAMIGRlcJiwjfv4TTJAQPug220cJQ8z+0UY1s9+twM=', 'dGVzdHNhbHQ=', 1, 1, 3, 0, 0, NULL, NULL);

-- Insert usermembership
INSERT INTO `usermembership` (`UserMembershipID`, `User_Id`, `MembershipID`, `StartDate`, `Expiration`, `CreatedAt`, `UpdatedAt`) VALUES
(1, '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', 1, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY), NOW(), NOW()),
(2, '08dcd23c-d4eb-456b-87e4-73837709fada', 4, NOW(), DATE_ADD(NOW(), INTERVAL 365 DAY), NOW(), NOW()),
(3, '08dcd23c-d4eb-45db-87e4-73837709fada', 1, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY), NOW(), NOW()),
(4, '08dcd23c-d4eb-45db-88e4-73837709fada', 1, NOW(), DATE_ADD(NOW(), INTERVAL 30 DAY), NOW(), NOW());

-- Insert offers
INSERT INTO `offers` (`Id`, `Title`, `Description`, `Location`, `GroupProperties`, `AdditionalLodgingProperties`, `LodgingType`, `Skills`, `Requirements`, `SpecialConditions`, `PossibleLocations`, `UserId`, `CreatedAt`, `Discriminator`, `FromDate`, `GroupSize`, `Mobility`, `ModifiedAt`, `OfferType`, `Status`, `ToDate`, `AddressId`) VALUES
(125,'Ferienhaus am See','Schönes Ferienhaus direkt am See mit Bootsverleih',NULL,'Familie mit Kindern',NULL,NULL,'Wände streichen, Gartenarbeit','Mindestens 2 Wochen Aufenthalt',NULL,NULL,'08dcd23c-d4eb-456b-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',4,0,'2025-07-27',1,0,'2025-08-26',NULL),
(126,'Alpine Hütte','Traditionelle Berghütte mit Panoramablick',NULL,'Wanderer, Bergsteiger',NULL,NULL,'Holz hacken, Kamin reinigen','Erfahrung im Umgang mit Holzöfen',NULL,NULL,'08dcd23c-d4eb-45db-87e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',6,1,'2025-07-27',1,0,'2025-09-10',NULL),
(127,'Stadtwohnung Zentrum','Moderne Wohnung im Herzen der Stadt',NULL,'Paare, Singles',NULL,NULL,'Putzen, Einkaufen, Haustiere versorgen','Keine Haustiere erlaubt',NULL,NULL,'08dcd23c-d4eb-45db-88e4-73837709fada','2025-07-27','OfferTypeLodging','2025-07-27',2,2,'2025-07-27',1,0,'2025-08-16',NULL),
(128,'Mein tolles Haus','Kommt mich besuchen',NULL,'','Boot',NULL,'Metall','Senioren, Gruppen',NULL,NULL,'08ddca8e-a8b9-48ef-8e0b-09db16414bfa','2025-07-27','OfferTypeLodging','2025-01-01',0,0,'0001-01-01',0,0,'2025-08-08',31);

-- Insert sample pictures (dummy base64 data)
INSERT INTO `pictures` (`Id`, `Hash`, `Width`, `ImageData`, `UserId`, `OfferId`) VALUES
(1, 'hash1', 800, 0x89504E470D0A1A0A0000000D49484452000000320000003208060000001E3F88B1000000097048597300000B1300000B1301009A9C180000000774494D4507E4010F0F1D1E3B3B3B3B0000001974455874436F6D6D656E740043726561746564207769746820546B696E7465720000000049454E44AE426082, '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', 125),
(2, 'hash2', 800, 0x89504E470D0A1A0A0000000D49484452000000320000003208060000001E3F88B1000000097048597300000B1300000B1301009A9C180000000774494D4507E4010F0F1D1E3B3B3B3B0000001974455874436F6D6D656E740043726561746564207769746820546B696E7465720000000049454E44AE426082, '08dcd23c-d4eb-45db-87e4-73837709fada', 126);

-- Insert sample offer applications (Status: 0=Pending, 1=Approved, 2=Rejected)
-- Note: No offer applications to avoid Admin User appearing in offer-request

-- Insert sample reviews (mit korrekten User-IDs und Sichtbarkeitslogik)
INSERT INTO `reviews` (`Id`, `OfferId`, `RatingValue`, `ReviewComment`, `CreatedAt`, `UpdatedAt`, `ReviewerId`, `ReviewedId`, `ReviewerFirstName`, `ReviewerLastName`, `ReviewerEmail`, `IsVisible`, `VisibilityDate`) VALUES 
(1, 125, 5, 'Sehr netter Gastgeber und schöne Unterkunft!', '2025-07-29 15:14:25.000000', '2025-07-29 15:14:25.000000', '08dcd23c-d4eb-45db-87e4-73837709fada', '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', 'John', 'Doe', 'john@example.com', 1, '2025-07-29 15:14:25.000000'),
(2, 126, 4, 'Gute Erfahrung, gerne wieder!', '2025-07-29 15:14:25.000000', '2025-07-29 15:14:25.000000', '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', '08dcd23c-d4eb-45db-87e4-73837709fada', 'Test', 'User', 'test@example.com', 1, '2025-07-29 15:14:25.000000');

-- Insert shop items (Membership Coupons)
INSERT INTO `shopitems` (`Id`, `Name`, `Duration`, `Price_Amount`, `Price_Currency`, `Description`) VALUES
(1, 'Testmitgliedschaft', 3, 0.50, 'EUR', 'Testmitgliedschaft für 3 Tage - nur während der Testphase verfügbar! Perfekt zum Ausprobieren der Plattform. Gilt nur für die Testphase und wird später entfernt.'),
(2, '1 Monat Mitgliedschaft', 30, 9.99, 'EUR', 'Flexible Mitgliedschaft für 1 Monat - perfekt zum Ausprobieren. Erhalten Sie vollen Zugang zu allen Angeboten und können Ihre Mitgliedschaft jederzeit verlängern.'),
(3, '3 Monate Mitgliedschaft', 90, 24.99, 'EUR', '3 Monate Mitgliedschaft mit attraktivem Rabatt. Sparen Sie 5€ im Vergleich zu 3x 1 Monat. Ideal für längere Aufenthalte und mehrere Reisen.'),
(4, '1 Jahr Mitgliedschaft', 365, 89.99, 'EUR', 'Jahresmitgliedschaft - unser beliebtestes Angebot! Sparen Sie 30€ im Vergleich zu 12x 1 Monat. Perfekt für regelmäßige Reisende.'),
(5, '3 Jahre Mitgliedschaft', 1095, 239.99, 'EUR', '3 Jahre Mitgliedschaft mit maximalem Rabatt. Sparen Sie 120€ im Vergleich zu 36x 1 Monat. Für langfristige Planer und Vielreisende.'),
(6, 'Lebenslange Mitgliedschaft', 9999, 499.99, 'EUR', 'Lebenslange Mitgliedschaft - einmal zahlen, für immer dabei! Unbegrenzter Zugang zu allen Angeboten ohne weitere Kosten. Das ultimative Angebot für lebenslange Reisende.');

-- Insert sample coupons
INSERT INTO `coupons` (`Id`, `Code`, `Name`, `Description`, `CreatedDate`, `CreatedBy`, `Duration`, `MembershipId`) VALUES
(1, 'WELCOME2024', 'Welcome Coupon', 'Welcome discount for new users', NOW(), '18ddca8e-a8b9-48ef-8e0b-09db16414bfb', 365, 1),
(2, 'PREMIUM50', 'Premium Discount', '50% off premium membership', NOW(), '18ddca8e-a8b9-48ef-8e0b-09db16414bfb', 365, 4);

-- Insert sample redemptions
INSERT INTO `redemptions` (`Id`, `CouponId`, `UserId`, `RedeemedDate`, `UserEmail`) VALUES
(1, 1, '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', NOW(), 'test@example.com');

-- Insert sample transactions
INSERT INTO `transaction` (`Id`, `TransactionDate`, `Amount_Value`, `Amount_Currency`, `ShopItemId`, `UserId`, `TransactionId`, `Status`) VALUES
(1, NOW(), 0.50, 'EUR', 1, '08ddca8e-a8b9-48ef-8e0b-09db16414bfa', 'txn_test_001', 1),
(2, NOW(), 0.50, 'EUR', 2, '18ddca8e-a8b9-48ef-8e0b-09db16414bfb', 'txn_test_002', 1);

-- Insert sample skills
INSERT INTO `skills` (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`) VALUES
(1,'Handwerk',NULL),(2,'Haushalt',NULL),(3,'Holz',1),(4,'Metall',1),(5,'Tapezieren',1),(6,'Wände streichen',1),(7,'Putzen',2),(8,'Aufräumen',2),(9,'Einkaufen',2),(10,'Kochen',2),(11,'zur Hand gehen - keine speziellen Kenntnisse notwendig',NULL),(12,'Tiersitting',NULL),(13,'Haussitting',11),(14,'Pferde',12),(15,'Kleintiere',12),(16,'Vögel oder Amphibien',12),(17,'Gartenarbeit',2),(18,'Trockenbau',1),(19,'Estrichleger',1),(20,'Fliesen verlegen',1),(21,'Installationsarbeiten Bad/Heizung/Sanitär',1),(22,'Installationsarbeiten Elektro',1),(23,'Zimmermann',1),(24,'Tischler',1),(25,'Dachdecker',1),(26,'Maurer',1),(27,'KFZ Reparatur',1),(28,'Informatiker',1),(29,'Backen',2),(30,'Kreativ',NULL),(31,'Marketing',30),(32,'Social Media',30),(33,'Texter',30),(34,'Grafiker',30),(35,'Fotografen',30),(36,'Journalisten',30),(37,'Webseite - Blog schreiben',30),(38,'Finanzen/Buchhaltung',NULL),(39,'Körpertherapie',NULL),(40,'Führerschein',NULL),(41,'Führerschein PKW',40),(42,'Führerschein LKW',40),(43,'Kettensägenschein vorhanden',1),(44,'Brennholz hacken',1),(45,'Landwirtschaft',NULL),(46,'Obst- und Gemüseanbau',45),(47,'Weinbau',45),(48,'Imkerei',45),(49,'Tierhaltung',45),(50,'Pferdepflege',49),(51,'Kuh- und Schafhaltung',49),(52,'Geflügelhaltung',49),(53,'Tourismus & Gastronomie',NULL),(54,'Rezeption',53),(55,'Kellner',53),(56,'Koch',53),(57,'Reiseführer',53),(58,'Sprachunterricht',53),(59,'Deutsch',58),(60,'Englisch',58),(61,'Französisch',58),(62,'Spanisch',58),(63,'Italienisch',58),(64,'Sport & Outdoor',NULL),(65,'Wandern',64),(66,'Klettern',64),(67,'Schwimmen',64),(68,'Skifahren',64),(69,'Surfen',64),(70,'Segeln',64),(71,'Fahrradreparatur',64),(72,'Bildung & Unterricht',NULL),(73,'Nachhilfe',72),(74,'Mathematik',73),(75,'Sprachen',73),(76,'Musikunterricht',72),(77,'Gitarre',76),(78,'Klavier',76),(79,'Gesang',76),(80,'Kunst & Design',30),(81,'Malen',80),(82,'Zeichnen',80),(83,'Fotografie',80),(84,'Videografie',80),(85,'Webdesign',80),(86,'Handwerk Spezial',1),(87,'Schmuckherstellung',86),(88,'Töpfern',86),(89,'Holzschnitzerei',86),(90,'Lederarbeiten',86),(91,'Nähen & Schneidern',86),(92,'Stricken & Häkeln',86),(93,'Gesundheit & Wellness',NULL),(94,'Yoga',93),(95,'Pilates',93),(96,'Meditation',93),(97,'Erste Hilfe',93),(98,'Pflege',93),(99,'Seniorenbetreuung',98),(100,'Kinderbetreuung',98),(101,'Bau & Renovierung',1),(102,'Dachbodenausbau',101),(103,'Kellerausbau',101),(104,'Balkonbau',101),(105,'Terrassenbau',101),(106,'Zaunbau',101),(107,'Carportbau',101),(108,'Gartenhausbau',101),(109,'Technik & IT',NULL),(110,'Computerreparatur',109),(111,'Smartphone-Reparatur',109),(112,'Netzwerk-Installation',109),(113,'Smart Home',109),(114,'Solaranlagen',109),(115,'Transport & Logistik',NULL),(116,'Umzugshilfe',115),(117,'Lieferfahrten',115),(118,'Möbeltransport',115),(119,'Veranstaltungen',NULL),(120,'Eventplanung',119),(121,'Dekoration',119),(122,'Musik bei Events',119),(123,'Fotografie bei Events',119),(124,'Catering',119),(125,'Reinigung & Wartung',NULL),(126,'Poolreinigung',125),(127,'Saunawartung',125),(128,'Kaminreinigung',125),(129,'Dachrinnenreinigung',125),(130,'Winterdienst',125),(131,'Buchhaltung',38),(132,'Steuererklärung',38),(133,'Lohnbuchhaltung',38),(134,'Controlling',38),(135,'Massage',39),(136,'Jin Shin Jyutsu',39),(137,'Fußreflexzonenmassage',39),(138,'Shiatsu',39),(139,'Thai-Massage',39),(140,'Exoten',12); 
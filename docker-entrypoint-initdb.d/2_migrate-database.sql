DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
        DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
        DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
        DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
        DECLARE SQL_EXP VARCHAR(1000);
        SELECT COUNT(*)
                INTO HAS_AUTO_INCREMENT_ID
                FROM `information_schema`.`COLUMNS`
                WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                        AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                        AND `Extra` = 'auto_increment'
                        AND `COLUMN_KEY` = 'PRI'
                        LIMIT 1;
        IF HAS_AUTO_INCREMENT_ID THEN
                SELECT `COLUMN_TYPE`
                        INTO PRIMARY_KEY_TYPE
                        FROM `information_schema`.`COLUMNS`
                        WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                                AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                                AND `COLUMN_KEY` = 'PRI'
                        LIMIT 1;
                SELECT `COLUMN_NAME`
                        INTO PRIMARY_KEY_COLUMN_NAME
                        FROM `information_schema`.`COLUMNS`
                        WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                                AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                                AND `COLUMN_KEY` = 'PRI'
                        LIMIT 1;
                SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
                SET @SQL_EXP = SQL_EXP;
                PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
                EXECUTE SQL_EXP_EXECUTE;
                DEALLOCATE PREPARE SQL_EXP_EXECUTE;
        END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))       
BEGIN
        DECLARE HAS_AUTO_INCREMENT_ID INT(11);
        DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
        DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
        DECLARE SQL_EXP VARCHAR(1000);
        SELECT COUNT(*)
                INTO HAS_AUTO_INCREMENT_ID
                FROM `information_schema`.`COLUMNS`
                WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                        AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                        AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
                        AND `COLUMN_TYPE` LIKE '%int%'
                        AND `COLUMN_KEY` = 'PRI';
        IF HAS_AUTO_INCREMENT_ID THEN
                SELECT `COLUMN_TYPE`
                        INTO PRIMARY_KEY_TYPE
                        FROM `information_schema`.`COLUMNS`
                        WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                                AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                                AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
                                AND `COLUMN_TYPE` LIKE '%int%'
                                AND `COLUMN_KEY` = 'PRI';
                SELECT `COLUMN_NAME`
                        INTO PRIMARY_KEY_COLUMN_NAME
                        FROM `information_schema`.`COLUMNS`
                        WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
                                AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
                                AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
                                AND `COLUMN_TYPE` LIKE '%int%'
                                AND `COLUMN_KEY` = 'PRI';
                SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
                SET @SQL_EXP = SQL_EXP;
                PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
                EXECUTE SQL_EXP_EXECUTE;
                DEALLOCATE PREPARE SQL_EXP_EXECUTE;
        END IF;
END //
DELIMITER ;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230818135829_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230818135829_InitialCreate') THEN

    CREATE TABLE `Users` (
        `UserId` int NOT NULL AUTO_INCREMENT,
        `UserName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `IsVerified` tinyint(1) NOT NULL,
        `MembershipActive` tinyint(1) NOT NULL,
        `MembershipExireDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230818135829_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230818135829_InitialCreate', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Users');
    ALTER TABLE `Users` DROP PRIMARY KEY;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` DROP COLUMN `IsVerified`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` DROP COLUMN `MembershipActive`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` DROP COLUMN `MembershipExireDate`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` RENAME COLUMN `UserName` TO `Name`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` RENAME COLUMN `UserId` TO `VerificationState`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` MODIFY COLUMN `VerificationState` int NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD `User_Id` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD `Adress` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD `CurrentMembershipMembershipID` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD `Email_Adress` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD CONSTRAINT `PK_Users` PRIMARY KEY (`User_Id`);
    CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Users', 'User_Id');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    CREATE TABLE `Membership` (
        `MembershipID` int NOT NULL AUTO_INCREMENT,
        `Expiration` datetime(6) NOT NULL,
        CONSTRAINT `PK_Membership` PRIMARY KEY (`MembershipID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    CREATE INDEX `IX_Users_CurrentMembershipMembershipID` ON `Users` (`CurrentMembershipMembershipID`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    ALTER TABLE `Users` ADD CONSTRAINT `FK_Users_Membership_CurrentMembershipMembershipID` FOREIGN KEY (`CurrentMembershipMembershipID`) REFERENCES `Membership` (`MembershipID`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231004072133_ClassesInitially') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231004072133_ClassesInitially', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` DROP FOREIGN KEY `FK_Users_Membership_CurrentMembershipMembershipID`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'Membership');
    ALTER TABLE `Membership` DROP PRIMARY KEY;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Membership` RENAME `Memberships`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` RENAME COLUMN `Name` TO `VisibleName`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` RENAME COLUMN `Adress` TO `Street`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `City` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `Country` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `DateOfBirth` date NOT NULL DEFAULT '0001-01-01';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `Gender` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `HouseNumber` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `IsEmailVerified` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `LastName` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD `PostCode` longtext CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Memberships` ADD CONSTRAINT `PK_Memberships` PRIMARY KEY (`MembershipID`);
    CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'Memberships', 'MembershipID');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    CREATE TABLE `Profiles` (
        `Profile_ID` int NOT NULL AUTO_INCREMENT,
        `NickName` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Profiles` PRIMARY KEY (`Profile_ID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    ALTER TABLE `Users` ADD CONSTRAINT `FK_Users_Memberships_CurrentMembershipMembershipID` FOREIGN KEY (`CurrentMembershipMembershipID`) REFERENCES `Memberships` (`MembershipID`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009162507_Class_User') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231009162507_Class_User', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009164115_Class_Skills') THEN

    CREATE TABLE `Skills` (
        `Skill_ID` int NOT NULL AUTO_INCREMENT,
        `SkillDescrition` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Skills` PRIMARY KEY (`Skill_ID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231009164115_Class_Skills') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231009164115_Class_Skills', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121090805_Class_skills_changed') THEN

    ALTER TABLE `Skills` ADD `ParentSkill_ID` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121090805_Class_skills_changed') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231121090805_Class_skills_changed', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121150741_New_Classes') THEN

    ALTER TABLE `Skills` MODIFY COLUMN `SkillDescrition` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121150741_New_Classes') THEN

    CREATE TABLE `Continents` (
        `Continent_ID` int NOT NULL AUTO_INCREMENT,
        CONSTRAINT `PK_Continents` PRIMARY KEY (`Continent_ID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121150741_New_Classes') THEN

    CREATE TABLE `Countries` (
        `Country_ID` int NOT NULL AUTO_INCREMENT,
        `CountryName` longtext CHARACTER SET utf8mb4 NULL,
        `Region_ID` int NULL,
        CONSTRAINT `PK_Countries` PRIMARY KEY (`Country_ID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121150741_New_Classes') THEN

    CREATE TABLE `Regions` (
        `Region_ID` int NOT NULL AUTO_INCREMENT,
        `RegionName` longtext CHARACTER SET utf8mb4 NULL,
        `ContinentID` int NULL,
        `CountryID` int NULL,
        CONSTRAINT `PK_Regions` PRIMARY KEY (`Region_ID`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20231121150741_New_Classes') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20231121150741_New_Classes', '7.0.10');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;
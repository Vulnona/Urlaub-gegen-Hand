-- Demo-Daten für UGH-System

-- Adressen
INSERT INTO addresses (`Id`, `Latitude`, `Longitude`, `DisplayName`, `HouseNumber`, `Road`, `Suburb`, `City`, `County`, `State`, `Postcode`, `Country`, `CountryCode`, `OsmId`, `OsmType`, `PlaceId`) VALUES
  (1, 52.5200, 13.4050, 'Berlin, Deutschland', '1', 'Unter den Linden', 'Mitte', 'Berlin', 'Berlin', 'Berlin', '10117', 'Deutschland', 'DE', 123456789, 'node', '987654321');

-- Memberships
INSERT INTO memberships (`MembershipID`, `Name`, `Description`, `DurationDays`, `Price`, `IsActive`) VALUES
  (1, 'Standard', 'Standard-Mitgliedschaft', 365, 0.00, 1),
  (2, 'Premium', 'Premium-Mitgliedschaft', 365, 49.99, 1);

-- Skills
INSERT INTO skills (`Skill_ID`, `SkillDescrition`, `ParentSkill_ID`) VALUES
  (100, 'Test-Skill', NULL);

-- Users
INSERT INTO users (`User_Id`, `FirstName`, `LastName`, `DateOfBirth`, `Gender`, `AddressId`, `Email_Address`, `Password`, `SaltKey`, `IsTwoFactorEnabled`, `TwoFactorSecret`, `BackupCodes`, `IsEmailVerified`, `MembershipId`, `Facebook_link`, `Link_RS`, `Link_VS`, `Hobbies`, `Skills`, `ProfilePicture`, `About`, `VerificationState`, `UserRole`) VALUES
  ('11111111-1111-1111-1111-111111111111', 'Max', 'Mustermann', '1990-01-01', 'm', 1, 'max@example.com', 'hashedpw', 'salt', 0, NULL, NULL, 1, 1, NULL, NULL, NULL, 'Lesen', '100', NULL, 'Testuser', 0, 0),
  ('22222222-2222-2222-2222-222222222222', 'Erika', 'Musterfrau', '1992-02-02', 'w', 1, 'erika@example.com', 'hashedpw', 'salt', 1, '2FASECRET', '["code1","code2"]', 1, 2, NULL, NULL, NULL, 'Sport', '100', NULL, 'Admin-Test', 1, 1);

-- UserProfiles
INSERT INTO userprofiles (`Id`, `User_Id`, `Hobbies`, `Skills`, `Options`) VALUES
  (1, '11111111-1111-1111-1111-111111111111', 'Lesen', '100', 0),
  (2, '22222222-2222-2222-2222-222222222222', 'Sport', '100', 1);
-- Demo- und Testdaten (nur für Dev, nicht Production)
-- Beispiel:
-- INSERT INTO users (User_Id, Email_Address, ...) VALUES (...);

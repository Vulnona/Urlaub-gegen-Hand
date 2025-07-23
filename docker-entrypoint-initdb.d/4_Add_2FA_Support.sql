-- UGH Database Migration: Add 2FA Support
-- Migration ID: 20250723174700_Add2FASupport
-- Description: Adds Two-Factor Authentication columns to users table

SET SQL_SAFE_UPDATES = 0;

-- Add 2FA columns to users table
ALTER TABLE users 
ADD COLUMN IsTwoFactorEnabled TINYINT(1) NOT NULL DEFAULT 0;

ALTER TABLE users 
ADD COLUMN TwoFactorSecret LONGTEXT NULL;

ALTER TABLE users 
ADD COLUMN BackupCodes LONGTEXT NULL;

-- Insert migration record
INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
VALUES ('20250723174700_Add2FASupport', '7.0.0');

SET SQL_SAFE_UPDATES = 1;

-- Setup Admin 2FA Requirement
-- This script ensures admin account has 2FA properly configured

-- Update admin account to require 2FA (this will be enforced in the application)
UPDATE users 
SET IsTwoFactorEnabled = 1,
    TwoFactorSecret = NULL,  -- Will be set when admin sets up 2FA
    BackupCodes = NULL       -- Will be generated when admin sets up 2FA
WHERE Email_Address = 'admin@gmail.com';

-- Note: Admin will be forced to set up 2FA on next login
-- This is handled in the AuthController with the 2FA enforcement logic

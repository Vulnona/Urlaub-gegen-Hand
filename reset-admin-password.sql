-- Reset Admin Password Script
-- This script will reset the admin password to 'admin123' with proper hashing

-- Check current admin user data
SELECT User_Id, Email_Address, Password, SaltKey FROM users WHERE Email_Address = 'admin@gmail.com';

-- Update the admin password with a temporary random password and salt.
-- You must generate a secure password and hash using your application's password service.
-- Example (DO NOT USE IN PRODUCTION):
-- UPDATE users SET 
--   Password = '<GENERATED_HASH>',
--   SaltKey = '<GENERATED_SALT>',
--   IsEmailVerified = 1
-- WHERE Email_Address = 'admin@gmail.com';

-- Alternatively, instruct the admin to reset their password via the application's password reset feature.

-- Verify the update
SELECT User_Id, Email_Address, Password, SaltKey, IsEmailVerified FROM users WHERE Email_Address = 'admin@gmail.com';

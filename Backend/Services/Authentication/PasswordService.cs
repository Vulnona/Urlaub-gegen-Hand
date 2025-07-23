using System.Security.Cryptography;

namespace UGH.Infrastructure.Services
{
    public class PasswordService
    {
        #region password-encryption
        public string GenerateSalt(int length = 32)
        {
            byte[] salt = new byte[length];
            RandomNumberGenerator.Fill(salt);
            return Convert.ToBase64String(salt);
        }


        // Hash the password using PBKDF2 with HMAC-SHA256
        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var HashPassword = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = HashPassword.GetBytes(32); 
                return Convert.ToBase64String(hash);
            }
        }

        // Verify the password against stored hash and salt
        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(salt))
                return false;

            try
            {
                string hashedInput = HashPassword(password, salt);
                return hashedInput == storedHash;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}

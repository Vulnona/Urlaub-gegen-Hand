using System.Security.Cryptography;

namespace UGHApi.Services
{
    public class PasswordService
    {
        public  string GenerateSalt(int length = 32)
        {
            byte[] salt = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        // Hash the password using PBKDF2 with HMAC-SHA256
        public string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256 bits
                return Convert.ToBase64String(hash);
            }
        }

        // Verify if the entered password matches the hashed password
        
    }
}

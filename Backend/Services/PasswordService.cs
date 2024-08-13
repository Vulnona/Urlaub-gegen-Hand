using System.Security.Cryptography;

namespace UGHApi.Services
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
        #endregion
    }
}

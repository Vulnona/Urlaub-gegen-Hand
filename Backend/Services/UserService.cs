using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHModels;

namespace UGHApi.Services
{
    public class UserService
    {
        private readonly UghContext _context;
        private readonly PasswordService _passwordService;
        public UserService(UghContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }
        public  User GetUserByToken(string token)
        {
            var userId = _context.EmailVerificators.Where(x => x.verificationToken.ToString().Equals(token)).Select(x=>x.user_Id).FirstOrDefault();
            if (userId>0)
            {
                 var user = _context.Users.Where(x=>x.User_Id == userId).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public bool IsValidUser(string Email, string Password)
        {
            var user = _context.Users.Where(x => x.Email_Adress.Equals(Email)).FirstOrDefault();
            if (user != null && user.IsEmailVerified)
            {
                string newHash = _passwordService.HashPassword(Password, user.SaltKey);
                if (newHash == user.Password)
                {
                    return true;
                }
                return false;
            }
            return false;

        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Simulate async operation (e.g., database query)
            var user= await _context.Users.Where(x => x.Email_Adress == email).FirstOrDefaultAsync();
            if(user != null && user.User_Id > 0)
            {
                return user;
            }
            return null;
        }
    }
}

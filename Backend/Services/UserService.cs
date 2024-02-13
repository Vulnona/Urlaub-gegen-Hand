using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHModels;

namespace UGHApi.Services
{
    public class UserService
    {
        private readonly UghContext _context;
        public UserService(UghContext context)
        {
            _context = context;
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
    }
}

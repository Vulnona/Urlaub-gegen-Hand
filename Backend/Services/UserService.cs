using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UGHApi.Models;
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

      
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            // Simulate async operation (e.g., database query)
            var user= await _context.Users.Where(x => x.Email_Adress == email).FirstOrDefaultAsync();
            if(user != null && user.User_Id > 0)
            {
                return user;
            }
            return null;
        }
        public bool CreateAdmin(RegisterRequest request)
        {
            try
            {
                if (_context.Users.Any(u => u.Email_Adress.ToLower().Equals(request.Email_Adress.ToLower())))
                {
                    // Email already exists
                    return false;
                }

                DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
                DateOnly dateOfBirth = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);

                var salt = _passwordService.GenerateSalt();
                var hashPassword = _passwordService.HashPassword(request.Password, salt);

                var newUser = new User(

                    request.VisibleName,
                request.FirstName,
                request.LastName,
                dateOfBirth,
                request.Gender,
                request.Street,
                request.HouseNumber,
                request.PostCode,
                request.City,
                request.Country,
                request.Email_Adress,
                false,
                hashPassword,
                salt
                    );
               
                newUser.VerificationState = UGH_Enums.VerificationState.verified;
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Add default role for the user (e.g., "User")
                var defaultUserRole = _context.UserRoles.FirstOrDefault(r => r.RoleName == "Admin");
                if (defaultUserRole != null)
                {
                    var userRoleMapping = new UserRoleMapping
                    {
                        UserId = newUser.User_Id,
                        RoleId = defaultUserRole.RoleId
                    };
                    _context.UserRolesMapping.Add(userRoleMapping);
                    _context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }
        public async Task<IEnumerable<string>> GetUserRolesByUserEmail(string userEmail)
        {
            var userRoles = await _context.Users
                .Where(ur => ur.Email_Adress == userEmail)
                .SelectMany(ur => _context.UserRolesMapping
                    .Where(urm => urm.UserId == ur.User_Id)
                    .Join(_context.UserRoles,
                        urm => urm.RoleId,
                        role => role.RoleId,
                        (urm, role) => role.RoleName))
                .ToListAsync();

            return userRoles;
        }

        public (bool IsValid, string ErrorMessage) ValidateUser(string Email, string Password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email_Adress.Equals(Email));

            if (user == null)
            {
                return (false, "User not found");
            }

            if (!user.IsEmailVerified)
            {
                return (false, "Email not verified");
            }

            if (user.VerificationState != UGH_Enums.VerificationState.verified)
            {
                return (false, "User not verified");
            }

            string newHash = _passwordService.HashPassword(Password, user.SaltKey);

            if (newHash != user.Password)
            {
                return (false, "Incorrect password");
            }

            return (true, "User is valid");
        }

    }
}

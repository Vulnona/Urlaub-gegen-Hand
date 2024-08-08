using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHModels;

namespace UGHApi.Services
{
    public class userservice
    {
        private readonly UghContext _context;
        private readonly PasswordService _passwordService;

        public userservice(UghContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public User GetUserByToken(string token)
        {
            try
            {
                var userId = _context.emailverificators
                                     .Where(x => x.verificationToken.ToString().Equals(token))
                                     .Select(x => x.user_Id)
                                     .FirstOrDefault();
                if (userId > 0)
                {
                    var user = _context.users.FirstOrDefault(x => x.User_Id == userId);
                    if (user != null)
                    {
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while getting user by token.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.Email_Address == email);
                if (user != null && user.User_Id > 0)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while getting user by email asynchronously.", ex);
            }
        }

        public bool CreateAdmin(RegisterRequest request)
        {
            try
            {
                if (_context.users.Any(u => u.Email_Address.ToLower().Equals(request.Email_Address.ToLower())))
                {
                    return false; // User already exists
                }

                DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
                DateOnly dateOfBirth = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);

                var salt = _passwordService.GenerateSalt();
                var hashPassword = _passwordService.HashPassword(request.Password, salt);
                var newUser = new User(
                    request.FirstName,
                    request.LastName,
                    dateOfBirth,
                    request.Gender,
                    request.Street,
                    request.HouseNumber,
                    request.PostCode,
                    request.City,
                    request.Country,
                    request.Email_Address,
                    false,
                    hashPassword,
                    salt,
                    request.Facebook_link,
                    request.Link_RS,
                    request.Link_VS,
                    request.State
                );
                newUser.VerificationState = UGH_Enums.VerificationState.verified;
                _context.users.Add(newUser);
                _context.SaveChanges();

                var defaultUserRole = _context.userroles.FirstOrDefault(r => r.RoleName == "user");
                if (defaultUserRole != null)
                {
                    var userRoleMapping = new UserRoleMapping
                    {
                        UserId = newUser.User_Id,
                        RoleId = defaultUserRole.RoleId
                    };
                    _context.userrolesmapping.Add(userRoleMapping);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while creating admin.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetuserrolesByUserEmail(string userEmail)
        {
            try
            {
                var userroles = await _context.users
                    .Where(ur => ur.Email_Address == userEmail)
                    .SelectMany(ur => _context.userrolesmapping
                        .Where(urm => urm.UserId == ur.User_Id)
                        .Join(_context.userroles,
                            urm => urm.RoleId,
                            role => role.RoleId,
                            (urm, role) => role.RoleName))
                    .ToListAsync();
                return userroles;
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while getting user roles by user email asynchronously.", ex);
            }
        }

        public (bool IsValid, string ErrorMessage) ValidateUser(string Email, string Password)
        {
            try
            {
                var user = _context.users.FirstOrDefault(x => x.Email_Address.Equals(Email));

                if (user == null)
                {
                    return (false, "User not found");
                }

                if (!user.IsEmailVerified)
                {
                    return (false, "Email not verified");
                }

                string newHash = _passwordService.HashPassword(Password, user.SaltKey);

                if (newHash != user.Password)
                {
                    return (false, "Incorrect password");
                }

                return (true, "User is valid");
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while validating user.", ex);
            }
        }

        public void DeleteUserInfo(int userId)
        {
            try
            {
                var user = _context.users.FirstOrDefault(u => u.User_Id == userId);
                if (user != null)
                {
                    user.Link_RS = null;
                    user.Link_VS = null;

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                throw new InvalidOperationException("An error occurred while deleting user information.", ex);
            }
        }
    }
}

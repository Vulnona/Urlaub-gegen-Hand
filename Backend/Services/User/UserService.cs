using Microsoft.EntityFrameworkCore;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;
using UGH.Domain.Entities;
// namespace UGH.Infrastructure.Services;
using UGHApi.DATA;
using UGH.Domain.Interfaces;

namespace UGH.Infrastructure.Services
{
    public class UserService
    {
        private readonly Ugh_Context _context;
        private readonly PasswordService _passwordService;
        private readonly ILogger<UserService> _logger;
        private readonly IUserMembershipRepository _userMembershipRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IAddressService _addressService;

        public UserService(
            Ugh_Context context,
            PasswordService passwordService,
            IUserMembershipRepository userMembershipRepository,
            IMembershipRepository membershipRepository,
            IAddressService addressService,
            ILogger<UserService> logger
        )
        {
            _context = context;
            _passwordService = passwordService;
            _userMembershipRepository = userMembershipRepository;
            _membershipRepository = membershipRepository;
            _addressService = addressService;
            _logger = logger;
        }

        public async Task<(bool IsValid, string ErrorMessage)> ValidateUser(string Email, string Password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    return (false, "Email and password are required");
                }

                // Get user from database
                var user = await _context.users
                    .FirstOrDefaultAsync(u => u.Email_Address == Email);

                if (user == null)
                {
                    return (false, "Invalid email or password");
                }

                // Verify password
                if (!_passwordService.VerifyPassword(Password, user.Password, user.SaltKey))
                {
                    return (false, "Invalid email or password");
                }

                return (true, "User is valid");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in ValidateUser: {ex.Message} | StackTrace: {ex.StackTrace}");
                return (false, "An error occurred during validation");
            }
        }

        public async Task<User> GetUserByPasswordResetTokenAsync(string tokenString)
        {
            // ...stub implementation...
            return null;
        }

        public async Task<Result> VerifyEmailAddressAsync(string token)
        {
            // ...stub implementation...
            return null;
        }

        public void DeleteUserInfo(Guid userId)
        {
            // ...stub implementation...
        }

        public async Task<string> GetUserRoleByUserEmail(string userEmail)
        {
            // ...stub implementation...
            return null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("GetUserByEmailAsync: Email is null or empty.");
                return null;
            }
            var user = await _context.users
                .Include(u => u.UserMemberships)
                .Include(u => u.CurrentMembership)
                .FirstOrDefaultAsync(u => u.Email_Address == email);
            if (user == null)
            {
                _logger.LogWarning($"GetUserByEmailAsync: No user found for email {email}.");
            }
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in GetUserByEmailAsync: {ex.Message} | StackTrace: {ex.StackTrace}");
            return null;
        }
    }
    }
    // ...existing code for UserService methods goes here, all inside the class and namespace...
}

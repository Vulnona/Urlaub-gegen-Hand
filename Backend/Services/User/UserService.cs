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
            // DEBUG: Print connection string from context
            string connStr = "(null)";
            try {
                connStr = _context?.Database?.GetDbConnection()?.ConnectionString ?? "(null)";
                Console.WriteLine($"[DEBUG] UserService using connection string: {connStr}");
                System.IO.File.AppendAllText("/app/connectionstring.log", $"[UserService DEBUG] {connStr}\n");
                // Try to open a connection to MySQL and catch any errors
                var conn = _context?.Database?.GetDbConnection();
                if (conn != null)
                {
                    try {
                        conn.Open();
                        Console.WriteLine("[DEBUG] MySQL connection opened successfully.");
                        conn.Close();
                    } catch (Exception ex) {
                        Console.WriteLine($"[DEBUG] MySQL connection error: {ex.Message}");
                        System.IO.File.AppendAllText("/app/connectionstring.log", $"[MySQL ERROR] {ex.Message}\n");
                        return (false, $"MySQL error: {ex.Message}");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"[DEBUG] Exception in ValidateUser: {ex.Message}");
                System.IO.File.AppendAllText("/app/connectionstring.log", $"[ValidateUser ERROR] {ex.Message}\n");
                return (false, $"ValidateUser error: {ex.Message}");
            }
            // ...actual validation logic here...
            return (true, "User is valid"); // stub for debug
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

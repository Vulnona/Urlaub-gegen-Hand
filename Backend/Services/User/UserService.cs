using Microsoft.EntityFrameworkCore;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGH.Domain.Interfaces;
using UGHApi.DATA;

namespace UGH.Infrastructure.Services;

public class UserService
{
    private readonly Ugh_Context _context;
    private readonly PasswordService _passwordService;
    private readonly ILogger<UserService> _logger;
    private readonly IUserMembershipRepository _userMembershipRepository;
    private readonly IMembershipRepository _membershipRepository;

    public UserService(
        Ugh_Context context,
        PasswordService passwordService,
        IUserMembershipRepository userMembershipRepository,
        IMembershipRepository membershipRepository,
        ILogger<UserService> logger
    )
    {
        _context = context;
        _passwordService = passwordService;
        _userMembershipRepository = userMembershipRepository;
        _membershipRepository = membershipRepository;
        _logger = logger;
    }

    #region user-services
    public async Task<User> GetUserByTokenAsync(string token)
    {
        try
        {
            var userId = await _context
                .emailverificators.Where(x => x.verificationToken.ToString().Equals(token))
                .Select(x => x.user_Id)
                .FirstOrDefaultAsync();

            if (userId != Guid.Empty)
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.User_Id == userId);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }
    
    public async Task<User> GetUserByPasswordResetTokenAsync(string tokenString)
    {
        try
        {
            var token = await _context
                .passwordresettokens.Where(x => x.Token.ToString().Equals(tokenString))                
                .FirstOrDefaultAsync();
            if (token == null)
                return null;
            bool valid = (DateTime.Compare(token.requestDate.AddHours(2), (DateTime.Now))) > 0;            
            UGH.Domain.Entities.User user = null;
            if (token.user_Id != Guid.Empty)
                user = await _context.users.FirstOrDefaultAsync(x => x.User_Id == token.user_Id);
            else
                valid = false;
            _context.Remove(_context.passwordresettokens.Single(x => x.Token.ToString().Equals(tokenString)));
            _context.SaveChanges();
            if (valid)
                return user;
            else
                return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return null;
        }
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.users.FirstOrDefaultAsync(r => r.Email_Address == email);
    }

    public async Task<Result> SetMembership(Guid userId, int membershipId)
    {
        try
        {
            var membership = await _membershipRepository.GetMembershipByIdAsync(membershipId);
            if (membership == null)
            {
                return Result.Failure(Errors.General.InvalidOperation("Membership not found."));
            }

            DateTime startDate = DateTime.UtcNow;
            DateTime expirationDate = startDate.AddDays(membership.DurationDays);

            var userMembership = new UserMembership
            {
                User_Id = userId,
                MembershipID = membershipId,
                StartDate = startDate,
                Expiration = expirationDate,
            };

            await _userMembershipRepository.AddUserMembershipAsync(userMembership);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                Errors.General.InvalidOperation(
                    $"An error occurred while setting the default membership: {ex.Message}"
                )
            );
        }
    }

    public async Task<Membership> GetDefaultMembershipAsync()
    {
        return await _context.memberships.FirstOrDefaultAsync(r => r.MembershipID == 2);
    }

    public async Task<Result> VerifyEmailAddressAsync(string token)
    {
        try
        {
            var user = await GetUserByTokenAsync(token);
            if (user == null)
            {
                return Result.Failure(new Error("Error.InvalidToken", "Invalid token."));
            }

            user.SetVerifyStatus(true);

            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while verifying email: {ex.Message} | StackTrace: {ex.StackTrace}"
            );

            return Result.Failure(
                new Error(
                    "Error.UnexpectedError",
                    "An unexpected error occurred while verifying the email."
                )
            );
        }
    }

    public async Task<string> GetUserRoleByUserEmail(string userEmail)
    {
        try
        {
            var user = await _context.users.Where(ur => ur.Email_Address == userEmail).FirstOrDefaultAsync();
            String userRole = "User";
            if (user.UserRole == UserRoles.Admin)
                userRole = "Admin";
            return userRole;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
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

    public void DeleteUserInfo(Guid userId)
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
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }
    #endregion
}

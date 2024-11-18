using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UGH.Contracts.Authentication;
using UGH.Domain.Core;
using UGH.Domain.Entities;
using UGHApi.Repositories.Interfaces;

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
            var userId = await _context.emailverificators
                .Where(x => x.verificationToken.ToString().Equals(token))
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

    public async Task<UserRole> GetDefaultUserRoleAsync()
    {
        return await _context.userroles.FirstOrDefaultAsync(r => r.RoleName == "User");
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

            DateTime startDate = DateTime.Now;
            DateTime expirationDate = startDate.AddDays(membership.DurationDays);

            var userMembership = new UserMembership
            {
                User_Id = userId,
                MembershipID = membershipId,
                StartDate = startDate,
                Expiration = expirationDate
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

    public async Task<bool> AssignUserRoleAsync(Guid userId, int roleId)
    {
        try
        {
            var existingMapping = await _context.userrolesmapping.AnyAsync(
                mapping => mapping.UserId == userId && mapping.RoleId == roleId
            );

            if (existingMapping)
            {
                return false;
            }

            var userRole = new UserRoleMapping { UserId = userId, RoleId = roleId };

            await _context.userrolesmapping.AddAsync(userRole);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while assigning user role. UserId: {userId}, RoleId: {roleId} | Exception: {ex.Message} | StackTrace: {ex.StackTrace}"
            );

            return false;
        }
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

    public bool CreateAdmin(RegisterRequest request)
    {
        try
        {
            if (
                _context.users.Any(
                    u => u.Email_Address.ToLower().Equals(request.Email_Address.ToLower())
                )
            )
            {
                return false;
            }

            DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
            DateOnly dateOfBirth = new DateOnly(
                parsedDateOfBirth.Year,
                parsedDateOfBirth.Month,
                parsedDateOfBirth.Day
            );

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
            newUser.VerificationState = UGH.Domain.Core.UGH_Enums.VerificationState.Verified;
            _context.users.Add(newUser);
            _context.SaveChanges();

            var defaultUserRole = _context.userroles.FirstOrDefault(r => r.RoleName == "Admin");
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
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesByUserEmail(string userEmail)
    {
        try
        {
            var userRoles = await _context.users
                .Where(ur => ur.Email_Address == userEmail)
                .SelectMany(
                    ur =>
                        _context.userrolesmapping
                            .Where(urm => urm.UserId == ur.User_Id)
                            .Join(
                                _context.userroles,
                                urm => urm.RoleId,
                                role => role.RoleId,
                                (urm, role) => role.RoleName
                            )
                )
                .ToListAsync();
            if (userRoles.Count == 0)
                return null;
            return userRoles;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
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
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
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

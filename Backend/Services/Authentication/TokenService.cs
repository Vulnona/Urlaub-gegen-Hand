using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using UGH.Domain.Entities;
using UGH.Domain.Core;
using UGHApi.DATA;

namespace UGH.Infrastructure.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;
    private readonly Ugh_Context _context;
    private readonly UserService _userService;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        IConfiguration configuration,
        IMemoryCache cache,
        Ugh_Context context,
        UserService userService,
        ILogger<TokenService> logger
    )
    {
        _configuration = configuration;
        _cache = cache;
        _context = context;
        _userService = userService;
        _logger = logger;
    }

    #region token-generation-service

    public string GenerateJwtToken(
        User user,
        List<UserMembership> memberships = null
    )
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string role = user.UserRole.ToString();

            var hasActiveMembership = memberships != null && memberships.Any();
            var membershipStatus = hasActiveMembership ? "Active" : "Inactive";

            _logger.LogError($"=== TOKEN SERVICE: User {user.Email_Address} has {memberships?.Count ?? 0} memberships, status: {membershipStatus} ===");

            var verificationStatus = user.VerificationState;
            String verified;
            if(verificationStatus == UGH_Enums.VerificationState.Verified)
                verified = "ok";
            else
                verified = "not";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email_Address),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("MembershipStatus", membershipStatus),
                new Claim("VerificationStatus", verified)
            };

            claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public Task<Guid?> GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                },
                out SecurityToken validatedToken
            );

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.NameIdentifier
            );

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                // Check if user still exists in DB
                var userExists = _context.users.Any(u => u.User_Id == userId);
                if (!userExists)
                {
                    // user doesn't exist
                    return Task.FromResult<Guid?>(null);
                }
                return Task.FromResult<Guid?>(userId);
            }
            else
            {
                throw new FormatException();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new ArgumentException(ex.Message);
        }
    }

    public string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public void StoreRefreshToken(string token, string email)
    {
        try
        {
            _cache.Set(token, email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public bool TryGetUserEmail(string token, out string userEmail)
    {
        try
        {
            return _cache.TryGetValue(token, out userEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public void RemoveRefreshToken(string token)
    {
        try
        {
            _cache.Remove(token);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public Guid GenerateNewEmailVerificator(Guid userId)
    {
        try
        {
            var newVerificator = new UGH.Domain.Entities.EmailVerificator
            {
                requestDate = DateTime.UtcNow,
                user_Id = userId,
                verificationToken = Guid.NewGuid(),
            };

            _context.emailverificators.Add(newVerificator);
            _context.SaveChanges();
            return newVerificator.verificationToken;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    public Guid GenerateNewPasswordResetToken(Guid userId) {
        try {
            var newToken = new UGH.Domain.Entities.PasswordResetToken
            {
                requestDate = DateTime.UtcNow,
                user_Id = userId,
                Token = Guid.NewGuid(),
            };

            _context.passwordresettokens.Add(newToken);
            _context.SaveChanges();
            return newToken.Token;
        }
        catch (Exception ex) {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new InvalidOperationException(ex.Message);
        }
    }

    // 2FA-Token für temporäre Authentifizierung (max. Sicherheit)
    public string GenerateTwoFactorToken(Guid userId, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim("TokenType", "2fa"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    #endregion
}

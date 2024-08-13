using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Backend.Models;

namespace UGHApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly UghContext _context;
        private readonly UserService _userService;

        public TokenService(IConfiguration configuration, IMemoryCache cache, UghContext context, UserService userService)
        {
            _configuration = configuration;
            _cache = cache;
            _context = context;
            _userService = userService;
        }
        #region token-generation-service
        public async Task<string> GenerateJwtToken(string userName, string userId)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var roles = await _userService.GetUserRolesByUserEmail(userName);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException( ex.Message);
            }
        }

        public Task<int?> GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Task.FromResult<int?>(userId);
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException( ex.Message);
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
                throw new InvalidOperationException(ex.Message);
            }
        }

        public Guid GenerateNewEmailVerificator(int userId)
        {
            try
            {
                var newVerificator = new EmailVerificator
                {
                    requestDate = DateTime.Now,
                    user_Id = userId,
                    verificationToken = Guid.NewGuid()
                };

                _context.emailverificators.Add(newVerificator);
                _context.SaveChanges();
                return newVerificator.verificationToken;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
    #endregion
}

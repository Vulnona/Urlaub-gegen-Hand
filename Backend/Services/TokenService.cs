using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UGHModels;
using Microsoft.Extensions.Caching.Memory;
using Backend.Models;
using UGHApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
            _userService =userService;
        }
        public async Task<string> GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userService.GetUserRolesByUserEmail(username);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
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
        public string GenerateRefreshToken()
        {
            // Generate a random refresh token
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public void StoreRefreshToken(string token, string email)
        {
            _cache.Set(token, email);
        }

        public bool TryGetUserEmail(string token, out string userEmail)
        {
            return _cache.TryGetValue(token, out userEmail);
        }

        public void RemoveRefreshToken(string token)
        {
            _cache.Remove(token);
        }
        public Guid GenrateNewEmailVerificator(int userId)
        {
            EmailVerificator newVerificator = new EmailVerificator();
            newVerificator.requestDate = DateTime.Now;
            newVerificator.user_Id = userId;
            newVerificator.verificationToken = Guid.NewGuid();

            _context.EmailVerificators.Add(newVerificator);
            _context.SaveChanges();
            return newVerificator.verificationToken;
        }
        
        
    }
}

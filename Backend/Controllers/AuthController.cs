using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services;
using UGHApi.Models;
using Backend.Models;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        

        public AuthController(UghContext context, EmailService emailService,UserService userService,PasswordService passwordService, IConfiguration configuration, TokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
            _passwordService = passwordService;
            _configuration = configuration;
            _tokenService = tokenService;
        }
       
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email_Adress.ToLower().Equals(request.Email_Adress.ToLower())))
            {
                return Conflict("E-Mail Adresse existiert bereits");
            }

            DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
            DateOnly dateOnly = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);

            var salt = _passwordService.GenerateSalt();
            var HashPassword = _passwordService.HashPassword(request.Password, salt);
            var newUser = new User(
                request.VisibleName,
                request.FirstName,
                request.LastName,
                dateOnly,
                request.Gender,
                request.Street,
                request.HouseNumber,
                request.PostCode,
                request.City,
                request.Country,
                request.Email_Adress,
                false,
                HashPassword,
                salt
            );

            newUser.VerificationState=UGH_Enums.VerificationState.isNew;

            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Add default role for the user (e.g., "User")
            var defaultUserRole = _context.UserRoles.FirstOrDefault(r => r.RoleName == "User");
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

            var verificatioToken = _tokenService.GenrateNewEmailVerificator(newUser.User_Id);

            //string verificationURL=request.VerificationURL.Replace("*USER_ID*",newUser.User_Id.ToString()).Replace("*TOKEN*",newVerificator.verificationToken.ToString());

            _emailService.SendVerificationEmail(newUser.Email_Adress, verificatioToken);

            return Ok(newUser);
        }


        [HttpGet("verifyemail")]
        public IActionResult Verify(string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null)
            {
                return NotFound("Invalid token");
            }

            user.IsEmailVerified = true;
            _context.SaveChanges();
            return Ok("Email verified successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Validate user credentials (e.g., check username and password against database)
            var validationResult = _userService.ValidateUser(model.Email, model.Password);

            if (validationResult.IsValid)
            {
                var accessToken = await _tokenService.GenerateJwtToken(model.Email);
                var refreshToken = _tokenService.GenerateRefreshToken();
                _tokenService.StoreRefreshToken(refreshToken, model.Email);

                return Ok(new { accessToken, refreshToken });
            }
           else
            {
                
                // Return the specific error message
                return BadRequest(validationResult.ErrorMessage);
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            if (!_tokenService.TryGetUserEmail(request.RefreshToken, out var Email))
            {
                return Unauthorized();
            }

            var accessToken =await _tokenService.GenerateJwtToken(Email);

            return Ok(new { accessToken });
        }

        [HttpPost("resendemailverification")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResentEmailVerification resentUrl)
        {
            // Validate email address
            if (string.IsNullOrEmpty(resentUrl.Email) || !_emailService.IsValidEmail(resentUrl.Email))
            {
                return BadRequest("Invalid email address.");
            }

            // Get user by email address
            var user = await _userService.GetUserByEmailAsync(resentUrl.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Generate new verification token
            var verificatioToken = _tokenService.GenrateNewEmailVerificator(user.User_Id);

            // Send verification email
            var emailSent= _emailService.SendVerificationEmail(resentUrl.Email, verificatioToken);
            if (!emailSent)
            {
                return StatusCode(500, "Failed to send verification email.");
            }

            return Ok("Verification email sent successfully.");
        }
       

    }
}


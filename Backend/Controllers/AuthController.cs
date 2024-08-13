using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services;
using UGHApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UGHApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public AuthController(UghContext context, EmailService emailService, UserService userService, PasswordService passwordService, IConfiguration configuration, TokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
            _passwordService = passwordService;
            _configuration = configuration;
            _tokenService = tokenService;
        }
        #region user-authorization
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                else if (await _context.users.AnyAsync(u => u.Email_Address.ToLower().Equals(request.Email_Address.ToLower())))
                {
                    return Conflict("E-Mail Adresse existiert bereits");
                }

                DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
                DateOnly dateOnly = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);

                var salt = _passwordService.GenerateSalt();
                var hashPassword = _passwordService.HashPassword(request.Password, salt);

                var newUser = new User(
                    request.FirstName,
                    request.LastName,
                    dateOnly,
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

                newUser.VerificationState = UGH_Enums.VerificationState.IsNew;

                await _context.users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                var getRegisteredUser = await _context.users.FirstOrDefaultAsync(u => u.Email_Address == request.Email_Address);

                var newUserProfile = new UserProfile
                {
                    User_Id = getRegisteredUser.User_Id,
                };

                await _context.userprofiles.AddAsync(newUserProfile);
                await _context.SaveChangesAsync();

                var defaultUserRole = await _context.userroles.FirstOrDefaultAsync(r => r.RoleName == "User");
                if (defaultUserRole != null)
                {
                    var userRoleMapping = new UserRoleMapping
                    {
                        UserId = newUser.User_Id,
                        RoleId = defaultUserRole.RoleId
                    };
                    await _context.userrolesmapping.AddAsync(userRoleMapping);
                    await _context.SaveChangesAsync();
                }

                var verificationToken = _tokenService.GenerateNewEmailVerificator(newUser.User_Id);
                await _emailService.SendVerificationEmailAsync(newUser.Email_Address, verificationToken);

                return Ok(newUser);
            }
            catch (Exception)
            {
                return StatusCode(400, "Bad Request ");
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> Verify([Required]string token)
        {
            try
            {
                var user = await _userService.GetUserByTokenAsync(token);
                if (user == null)
                {
                    return NotFound("Invalid token");
                }

                user.IsEmailVerified = true;
                await _context.SaveChangesAsync();
                return Ok("Email verified successfully");
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userValid = _userService.ValidateUser(model.Email, model.Password);

                var user = await _context.users.FirstOrDefaultAsync(u => u.Email_Address == model.Email);
                if (userValid.IsValid)
                {
                    var accessToken = await _tokenService.GenerateJwtToken(model.Email, user.User_Id.ToString());
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    _tokenService.StoreRefreshToken(refreshToken, model.Email);

                    return Ok(new { accessToken, refreshToken, model.Email, user.User_Id, user.FirstName });
                }
                else
                {
                    return Unauthorized(new { errorMessage = userValid.ErrorMessage });
                }
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");

            }
        }

        [HttpPost("refresh-request-token")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            try
            {
                if (!_tokenService.TryGetUserEmail(request.RefreshToken, out var Email))
                {
                    return Unauthorized();
                }

                var accessToken = await _tokenService.GenerateJwtToken(Email, string.Empty); 
                return Ok(new { accessToken });
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendEmailVerification resentUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrEmpty(resentUrl.Email) || !_emailService.IsValidEmail(resentUrl.Email))
                {
                    return BadRequest("Invalid email address.");
                }
                var user = await _userService.GetUserByEmailAsync(resentUrl.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                var verificationToken = _tokenService.GenerateNewEmailVerificator(user.User_Id);

                // Send verification email asynchronously
                var emailSent = await _emailService.SendVerificationEmailAsync(resentUrl.Email, verificationToken);
                if (!emailSent)
                {
                    return StatusCode(500, "Failed to send verification email.");
                }

                return Ok("Verification email sent successfully.");
            }
            catch (Exception)
            { 
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion
    }
}

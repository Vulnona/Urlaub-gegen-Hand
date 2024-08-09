using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services;
using UGHApi.Models;
using Microsoft.EntityFrameworkCore;

namespace UGHApi.Controllers
{
    [Route("api/auth")]
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

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                else if (_context.users.Any(u => u.Email_Address.ToLower().Equals(request.Email_Address.ToLower())))
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

                _context.users.Add(newUser);
                _context.SaveChanges();

                var getRegisteredUser = _context.users.FirstOrDefault(u => u.Email_Address == request.Email_Address);

                var newUserProfile = new UserProfile
                {
                    User_Id = getRegisteredUser.User_Id,
                };

                _context.userprofiles.Add(newUserProfile);
                _context.SaveChanges();

                var defaultUserRole = _context.userroles.FirstOrDefault(r => r.RoleName == "User");
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

                var verificationToken = _tokenService.GenerateNewEmailVerificator(newUser.User_Id);
                _emailService.SendVerificationEmail(newUser.Email_Address, verificationToken);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("verify-email")]
        public IActionResult Verify(string token)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                var userValidation = _userService.ValidateUser(model.Email, model.Password);

                var user = await _context.users.FirstOrDefaultAsync(u => u.Email_Address == model.Email);
                if (userValidation.IsValid)
                {
                    var accessToken = await _tokenService.GenerateJwtToken(model.Email, user.User_Id.ToString());
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    _tokenService.StoreRefreshToken(refreshToken, model.Email);

                    return Ok(new { accessToken, refreshToken, model.Email, user.User_Id, user.FirstName });
                }
                else
                {
                    return Unauthorized(new { errorMessage = userValidation.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                // Log the exception 
               //return StatusCode(500, $"Internal server error: {ex.Source+"***"+ ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            try
            {
                if (!_tokenService.TryGetUserEmail(request.RefreshToken, out var Email))
                {
                    return Unauthorized();
                }

                var accessToken = await _tokenService.GenerateJwtToken(Email, string.Empty); // Assuming you have a method that generates the token

                return Ok(new { accessToken });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                var verificationToken = _tokenService.GenerateNewEmailVerificator(user.User_Id);

                // Send verification email
                var emailSent = _emailService.SendVerificationEmail(resentUrl.Email, verificationToken);
                if (!emailSent)
                {
                    return StatusCode(500, "Failed to send verification email.");
                }

                return Ok("Verification email sent successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

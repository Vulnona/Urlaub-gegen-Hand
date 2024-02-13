using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services;
using UGHApi.Models;
using Backend.Models;
using Microsoft.AspNetCore.Mvc.Abstractions;

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

        public AuthController(UghContext context, EmailService emailService,UserService userService,PasswordService passwordService)
        {
            _context = context;
            _emailService = emailService;
            _userService = userService;
            _passwordService = passwordService;
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

            EmailVerificator newVerificator= new EmailVerificator();
            newVerificator.requestDate=DateTime.Now;
            newVerificator.user_Id=newUser.User_Id;
            newVerificator.verificationToken=Guid.NewGuid();

            _context.EmailVerificators.Add(newVerificator);
            _context.SaveChanges();

            //string verificationURL=request.VerificationURL.Replace("*USER_ID*",newUser.User_Id.ToString()).Replace("*TOKEN*",newVerificator.verificationToken.ToString());

            _emailService.SendVerificationEmail(newUser.Email_Adress, newVerificator.verificationToken);

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
       
    }
}

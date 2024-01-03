using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services; 

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly EmailService _emailService;

        public AuthController(UghContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email_Adress == request.Email_Adress))
            {
                return Conflict("E-Mail Adresse existiert bereits");
            }

            DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
            DateOnly dateOnly = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);


            var newUser = new User(
                request.User_Id,
                request.VisibleName,
                request.FirstName,
                request.LastName,
                parsedDateOfBirth,
                request.Gender,
                request.Street,
                request.HouseNumber,
                request.PostCode,
                request.City,
                request.Country,
                request.Email_Adress,
                request.IsEmailVerified
            );

            _context.Users.Add(newUser);
            _context.SaveChanges();

            _emailService.SendEmail(newUser.Email_Adress, "Bestätigen Sie Ihre E-Mail", "Hier ist Ihr Bestätigungslink...");

            return Ok(newUser);
        }
    }
}

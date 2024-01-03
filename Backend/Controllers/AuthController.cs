using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services; // Stellen Sie sicher, dass der richtige Namespace für EmailService verwendet wird

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
                return Conflict("A user with this email already exists");
            }

            // Hier sollten Sie die Validierung des 'request' Modells durchführen,
            // um sicherzustellen, dass die Daten korrekt sind, bevor Sie sie verwenden.

            var newUser = new User
            {
                User_Id = request.User_Id,
                VisibleName = request.VisibleName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                City = request.City,
                Country = request.Country,
                Email_Adress = request.Email_Adress,
                IsEmailVerified = request.IsEmailVerified
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            // Hier senden Sie die Bestätigungsmail
            _emailService.SendEmail(newUser.Email_Adress, "Bestätigen Sie Ihre E-Mail", "Hier ist Ihr Bestätigungslink...");

            return Ok(newUser);
        }
    }
}

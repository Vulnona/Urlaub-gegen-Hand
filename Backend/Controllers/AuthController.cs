using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UGHModels;
using UGHApi.Services;
using UGHApi.Models;
using Backend.Models;

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
            if (_context.Users.Any(u => u.Email_Adress.ToLower().Equals(request.Email_Adress.ToLower())))
            {
                return Conflict("E-Mail Adresse existiert bereits");
            }

            DateTime parsedDateOfBirth = DateTime.Parse(request.DateOfBirth);
            DateOnly dateOnly = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);


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
                false
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

            string verificationURL=request.VerificationURL.Replace("*USER_ID*",newUser.User_Id.ToString()).Replace("*TOKEN*",newVerificator.verificationToken.ToString());

            _emailService.SendEmail(newUser.Email_Adress, "Bestätigen Sie Ihre E-Mail", "<a href="+""+verificationURL+""+">Hier klicken um Ihre E-Mail zu bestätigen</a>");

            return Ok(newUser);
        }
    }
}

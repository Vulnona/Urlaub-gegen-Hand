using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHModels;
=======
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UGHModels;  
using System.ComponentModel.DataAnnotations;
>>>>>>> merge-master-to-main

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UghContext _context;

        public AuthController(UghContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // POST: api/Auth/Register
        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            if (request.Email == null || request.Password == null || request.Username == null)
            {
                return BadRequest();
            }

            if (_context.Users.Any(e => e.Email == request.Email))
            {
                return Conflict();
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Username = request.Username,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // POST: api/Auth/Login
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            if (request.Email == null || request.Password == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == request.Email && e.Password == request.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        // POST: api/Auth/VerifyEmail
        [HttpPost]
        public async Task<ActionResult<string>> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == request.Email);

            if (user == null)
            {
                return NotFound();
            }

            user.EmailVerified = true;
            await _context.SaveChangesAsync();

            return "Die E-Mail-Adresse wurde verifiziert.";
        }

        // POST: api/Auth/ResendVerificationEmail
        [HttpPost]
        public async Task<ActionResult<string>> ResendVerificationEmail([FromBody] ResendVerificationEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == request.Email);

            if (user == null)
            {
                return NotFound();
            }

            var link = "https://example.com/verify?id=" + user.UserId;

            var email = new EmailMessage()
            {
                Subject = "E-Mail-Adresse verifizieren",
                Body = $"Um Ihre E-Mail-Adresse zu verifizieren, klicken Sie bitte auf den folgenden Link: {link}",
                To = user.Email,
            };

            await _context.Emails.AddAsync(email);
            await _context.SaveChangesAsync();

            return "Die E-Mail wurde erneut versendet.";
        }

        public class RegisterRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class VerifyEmailRequest
        {
            public string Email { get; set; }
        }

        public class ResendVerificationEmailRequest
        {
            public string Email { get; set; }
        }
    }
}
=======
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email_Adress == request.Email_Adress))
            {
                return Conflict("A user with this email already exists");
            }

            var newUser = new User(
                request.User_Id,
                request.VisibleName,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
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
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }
    }

    public class RegisterRequest
    {
        [Required]
        public int User_Id { get; set; }

        [Required]
        public string VisibleName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Email_Adress { get; set; }

        [Required]
        public bool IsEmailVerified { get; set; }
    }
}
>>>>>>> merge-master-to-main

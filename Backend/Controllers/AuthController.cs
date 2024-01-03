using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHModels;
using System.ComponentModel.DataAnnotations;

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


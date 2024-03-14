using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using UGHApi.Models;
using UGHApi.Services;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly UserService _userService; private readonly IConfiguration _configuration;

        public AdminController(UghContext context, UserService userService, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }
        
        
    }
}

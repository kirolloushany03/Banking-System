using Banking_System.Data;
using Banking_System.Dtos.security;
using Banking_System.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Controllers.security
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController: ControllerBase
    {
        private readonly DataContext _context;

        public RegisterController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        { 
            var customerexist = await _context.TbCustomer.AnyAsync(c => c.Email == registerDto.Email);
            if (customerexist)
            {
                return Conflict("this email already exists 🤡");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newCustomer = new Customer
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = "customer"
            };

            _context.TbCustomer.Add(newCustomer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Chill, your email's safely registered. 😎" });
        }
    }
}

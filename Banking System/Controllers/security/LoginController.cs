using Banking_System.Data;
using Banking_System.Dtos.security;
using Banking_System.security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Controllers.security
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly JwtProvider _jwtProvider;

        public LoginController(DataContext context, JwtProvider jwtProvider)
        { 
            _context = context;
            _jwtProvider = jwtProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        { 
            var customer = await _context.TbCustomer.FirstOrDefaultAsync(c => c.Email == loginDto.Email);
            if (customer == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            bool ispasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, customer.PasswordHash);
            if (!ispasswordValid)
            {
                return Unauthorized(new { message  = "Invalid email or password." });
            }

            var token = _jwtProvider.CreateToken(customer);
            return Ok(new { token });
        }

    }
}

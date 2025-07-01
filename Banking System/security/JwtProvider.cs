using System.Security.Claims;
using System.Text;
using Banking_System.Entites;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Banking_System.security
{
    public class JwtProvider
    {
        private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Customer customer)
        {
            string secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException(
                "Jwt secretKey is not found");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                ([
                    new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, customer.Email),
                    new Claim(ClaimTypes.Role, customer.Role)


                ]),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:Expiration"]!)),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials
            };

            var tokenhandler = new JsonWebTokenHandler();
            
            return tokenhandler.CreateToken(TokenDescriptor);

        }
    }
}

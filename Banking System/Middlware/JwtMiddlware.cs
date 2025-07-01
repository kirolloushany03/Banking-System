using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Banking_System.Middlware
{
    public class JwtMiddlware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddlware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            { 
                await AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // Using the standard handler for maximum compatibility
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured."));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // STEP 1: Validate the token and get the principal.
                // The ValidateToken method will throw an exception if validation fails.
                // If it succeeds, it returns the ClaimsPrincipal.
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // STEP 2: Explicitly attach the validated principal to the context.
                // This is the most direct way to set the user.
                context.User = principal;

                // --- Optional Debugging ---
                // You can leave this in for one last test run to confirm success.
                Console.WriteLine("--- JWT Middleware Debug ---");
                Console.WriteLine("Token validation succeeded.");
                Console.WriteLine($"User IsAuthenticated: {context.User.Identity?.IsAuthenticated}");
                Console.WriteLine("Claims found:");
                foreach (var claim in context.User.Claims)
                {
                    Console.WriteLine($"- {claim.Type}: {claim.Value}");
                }
                Console.WriteLine("--- End Debug ---");
            }
            catch (Exception ex)
            {
                // If ValidateToken throws an exception (e.g., token expired, bad signature),
                // we will log it and do nothing, leaving the user as anonymous.
                Console.WriteLine("--- JWT Middleware EXCEPTION ---");
                Console.WriteLine($"Exception during token validation: {ex.Message}");
                Console.WriteLine("--- End Exception ---");
            }
        }
    }
}

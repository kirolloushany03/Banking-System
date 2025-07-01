using System.Security.Claims;
using Banking_System.Dtos.AccountDtos;
using Banking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking_System.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
        {
            // 1. Get the customer's ID from the JWT claims.
            //    The "Sub" (Subject) claim holds the user's unique ID.
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (customerIdClaim == null)
            {
                // This should not happen if [Authorize] is working, but it's a good safety check.
                return Unauthorized();
            }

            // 2. Parse the ID into an integer.
            if (!int.TryParse(customerIdClaim.Value, out var customerId))
            {
                return BadRequest("Invalid customer ID in token.");
            }

            // 3. Call the service with the DTO and the customer's ID.
            var account = await _accountService.CreateAccountAsync(createAccountDto, customerId);

            // CreatedAtAction is better for POST responses
            return CreatedAtAction(nameof(GetBalanceDto), new { accountNumber = account.AccountNumber }, account);
        }

        [HttpGet("{accountNumber}/balance")]
        public async Task<IActionResult> GetBalanceDto(string accountNumber)
        {
            try
            {
                var balance = await _accountService.GetBalance(accountNumber);
                return Ok(new { Balance = balance });
            }
            catch (Exception ex)
            {
                return NotFound(new {Message = ex.Message});
            }
        }

    }
}

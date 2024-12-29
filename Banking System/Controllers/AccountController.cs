using Banking_System.Dtos.AccountDtos;
using Banking_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking_System.Controllers
{
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
            var account = await _accountService.CreateAccountAsync(createAccountDto);
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

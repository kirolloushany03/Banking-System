using Banking_System.Data;
using Banking_System.Dtos.TransactionDtos;
using Banking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banking_System.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly DataContext _context;

        public TransactionController(ITransactionService transactionService,DataContext context)
        {
            _transactionService = transactionService;
            _context = context;
        }

        [Authorize]
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] CreateDepositDto createDepositDto)
        {
            var transaction = await _transactionService.DepositAsync(createDepositDto);
            return Ok(transaction);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] CreateWithdrawDto createWithdrawDto)
        {
            var transaction = await _transactionService.WithdrawAsync(createWithdrawDto);
            return Ok(transaction);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] CreateTransferDto createTransferDto)
        { 
            var transaction = await _transactionService.TransferAsync(createTransferDto);
            return Ok(transaction);
        }

        // POST /api/accounts/transaction - Manually create a transaction (if needed)
        [HttpPost("transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto createTransactionDto)
        {
            
            var resultdto = await _transactionService.CreateManualTransactionAsync(createTransactionDto);
            return CreatedAtAction(nameof(GetTransaction), new { id = resultdto.Id }, resultdto);
        }

        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _context.TbTransaction
                .Include(t => t.Account) // Ensure Account is included in the query
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            // Prepare a custom response that includes the account number
            var response = new
            {
                transaction.Id,
                AccountNumber = transaction.Account?.AccountNumber,
                transaction.TransactionType,
                transaction.Amount,
                transaction.Timestamp
            };

            return Ok(response);
        }

    }
}

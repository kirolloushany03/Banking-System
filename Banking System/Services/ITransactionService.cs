using Banking_System.Entites;
using Banking_System.Dtos.TransactionDtos;

namespace Banking_System.Services
{
    public interface ITransactionService
    {
        Task<TransactionDto> DepositAsync(CreateDepositDto createDepositDto);
        Task<TransactionDto> WithdrawAsync(CreateWithdrawDto createWithdrawDto);
        Task<TransactionDto> TransferAsync(CreateTransferDto createTransferDto);
        Task<TransactionDto> LogTransactionAsync(Transaction transaction);
    }
}

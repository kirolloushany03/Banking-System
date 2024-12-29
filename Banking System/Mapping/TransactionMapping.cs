using AutoMapper;
using Banking_System.Dtos.TransactionDtos;
using Banking_System.Entites;

namespace Banking_System.Mapping
{
    public class TransactionMapping: Profile
    {
        public TransactionMapping() {

            // Mapping from CreateWithdrawDto to Transaction
            CreateMap<CreateWithdrawDto, Transaction>()
                .ForPath(dest => dest.Account.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(_ => TransactionType.Withdrawal))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping from CreateDepositDto to Transaction
            CreateMap<CreateDepositDto, Transaction>()
                .ForPath(dest => dest.Account.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(_ => TransactionType.Deposit))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping from CreateTransferDto to Transaction
            CreateMap<CreateTransferDto, Transaction>()
                .ForPath(dest => dest.Account.AccountNumber, opt => opt.MapFrom(src => src.SourceAccountNumber)) // Source account number
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(_ => TransactionType.transfer))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapping from CreateTransactionDto to Transaction
            CreateMap<CreateTransactionDto, Transaction>()
                .ForPath(dest => dest.Account.AccountNumber, opt => opt.MapFrom(src => src.AccountId)) // Assumes AccountId is a field on Transaction, can adjust based on model
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.AccountNumber));
        }
    }
}

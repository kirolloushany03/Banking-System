using AutoMapper;
using Banking_System.Dtos.AccountDtos;
using Banking_System.Dtos.TransactionDtos;
using Banking_System.Entites;

namespace Banking_System.MappingProfiles
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<CreateAccountDto, Account>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance));

            CreateMap<Account, GetBalanceDto>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));

            CreateMap<Account, AccountDto>();

        }
    }
}
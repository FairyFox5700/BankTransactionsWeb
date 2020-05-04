using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class TransactioMapperProfile : Profile
    {
        public TransactioMapperProfile()
        {
            CreateMap<TransactionDTO, UpdateTransactionViewModel>()
      .ForMember(vm => vm.AccountDestinationNumber, dto => dto.MapFrom(s => s.DestinationAccountNumber))
     .ForMember(vm => vm.AccountSourceNumber, dto => dto.MapFrom(s => s.SourceAccountNumber)).ReverseMap();
            CreateMap<AddTransactionViewModel, TransactionDTO>();
        }
    }
}

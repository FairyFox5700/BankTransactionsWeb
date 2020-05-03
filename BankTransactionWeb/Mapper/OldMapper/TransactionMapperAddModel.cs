using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class TransactionMapperAddModel : IMapper<TransactionDTO, AddTransactionViewModel>
    {
        public AddTransactionViewModel Map(TransactionDTO source)
        {
            return new AddTransactionViewModel()
            {
                AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                Amount = source.Amount,
            };
        }

        public TransactionDTO MapBack(AddTransactionViewModel destination)
        {
            return new TransactionDTO()
            {
                AccountDestinationId = destination.AccountDestinationId,
                AccountSourceId = destination.AccountSourceId,
                Amount = destination.Amount

            };
        }
    }

   
}




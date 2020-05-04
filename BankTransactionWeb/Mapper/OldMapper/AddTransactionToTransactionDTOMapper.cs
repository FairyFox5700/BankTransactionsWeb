using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Mapper
{
    public class AddTransactionToTransactionDTOMapper : IMapper<TransactionDTO, AddTransactionViewModel>
    {
        private AddTransactionToTransactionDTOMapper() { }
        public static readonly AddTransactionToTransactionDTOMapper Instance = new AddTransactionToTransactionDTOMapper();
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




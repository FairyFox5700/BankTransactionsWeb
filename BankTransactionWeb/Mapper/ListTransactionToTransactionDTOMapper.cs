using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class ListTransactionToTransactionDTOMapper : IMapper<TransactionDTO, TransactionListViewModel>
    {
        private ListTransactionToTransactionDTOMapper() { }
        public static readonly ListTransactionToTransactionDTOMapper Instance = new ListTransactionToTransactionDTOMapper();
        public TransactionListViewModel Map(TransactionDTO source)
        {
            return new TransactionListViewModel()
            {
                Id = source.Id,
                DateOftransfering = source.DateOftransfering,
                AccountSourceId = source.AccountSourceId,
                AccountDestinationeNumber = source.DestinationAccountNumber,
                AccountSourceNumber = source.SourceAccountNumber,
                Amount = source.Amount

            };
        }

        public TransactionDTO MapBack(TransactionListViewModel destination)
        {
            return new TransactionDTO()
            {
                AccountSourceId = destination.AccountSourceId,
                DateOftransfering = destination.DateOftransfering,
                Amount = destination.Amount,
                Id = destination.Id
            };
        }
    }

   
}




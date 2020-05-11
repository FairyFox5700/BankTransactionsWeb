using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class UpdateTransactionToTransactionDTOMapper : IMapper<TransactionDTO, UpdateTransactionViewModel>
    {
        private UpdateTransactionToTransactionDTOMapper() { }
        public static readonly UpdateTransactionToTransactionDTOMapper Instance = new UpdateTransactionToTransactionDTOMapper();
        public UpdateTransactionViewModel Map(TransactionDTO source)
        {
            return new UpdateTransactionViewModel()
            {
                Id = source.Id,
                AccountSourceNumber = source.SourceAccountNumber,
                AccountDestinationNumber = source.DestinationAccountNumber,
                AccountSourceId = source.AccountSourceId,
                Amount = source.Amount
            };
        }

        public TransactionDTO MapBack(UpdateTransactionViewModel destination)
        {
            return new TransactionDTO()
            {
                Id = destination.Id,
                AccountDestinationId = destination.AccountSourceId,
                AccountSourceId = destination.AccountSourceId,
                DestinationAccountNumber = destination.AccountSourceNumber,
                SourceAccountNumber = destination.AccountSourceNumber,
                Amount = destination.Amount

            };
        }
    }

   
}




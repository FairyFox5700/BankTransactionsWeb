using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

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
                AccountSourceNumber = source.SourceAccount.Number,
                AccountDestinationNumber = source.DestinationAccount.Number,
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
                Amount = destination.Amount

            };
        }
    }

   
}




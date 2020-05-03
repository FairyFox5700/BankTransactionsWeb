using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class TransactionMapperList : IMapper<TransactionDTO, TransactionListViewModel>
    {
        public TransactionListViewModel Map(TransactionDTO source)
        {
            return new TransactionListViewModel()
            {
                Id = source.Id,
                DateOftransfering = source.DateOftransfering,
                AccountSourceId = source.AccountSourceId,
                AccountDestinationeNumber = source.DestinationAccount?.Number,
                AccountSourceNumber = source.SourceAccount?.Number,
                Amount = source.Amount

            };
        }

        public TransactionDTO MapBack(TransactionListViewModel destination)
        {
            return new TransactionDTO()
            {
                Id = destination.Id,
                DateOftransfering = destination.DateOftransfering,
                AccountSourceId = destination.AccountSourceId,
                //AccountDestinationeNumber = source.DestinationAccount?.Number,
                //AccountSourceNumber = source.SourceAccount?.Number,
                Amount = destination.Amount

            };
        }
    }

   
}




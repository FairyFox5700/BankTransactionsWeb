using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
{
    public class AccountMapperUpdateModel : IMapper<AccountDTO, UpdateAccountViewModel>
    {

        public UpdateAccountViewModel Map(AccountDTO source)
        {
            return new UpdateAccountViewModel()
            {
                Id = source.Id,
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }


        public AccountDTO MapBack(UpdateAccountViewModel destination)
        {
            return new AccountDTO()
            {
                Id = destination.Id,
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




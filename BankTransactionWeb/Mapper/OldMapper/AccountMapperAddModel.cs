using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
{
    public class AccountMapperAddModel : IMapper<AccountDTO, AddAccountViewModel>
    {
        public AddAccountViewModel Map(AccountDTO source)
        {
            return new AddAccountViewModel()
            {
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }

        public AccountDTO MapBack(AddAccountViewModel destination)
        {
            return new AccountDTO()
            {
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




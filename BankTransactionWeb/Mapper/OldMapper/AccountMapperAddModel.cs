using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class AccountMapperAddModel : IMapper<AccountDTO, AddAccountViewModel>
    {
        private AccountMapperAddModel()
        {
        }

        public static AccountMapperAddModel Instance { get; private set; } = new AccountMapperAddModel();
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




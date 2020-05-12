using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class AddAccountToAccountDTOMapper : IMapper< AddAccountViewModel, AccountDTO>
    {
        private AddAccountToAccountDTOMapper() { }

        public static readonly AddAccountToAccountDTOMapper Instance = new AddAccountToAccountDTOMapper();
        public AccountDTO Map(AddAccountViewModel source)
        {
            return new AccountDTO()
            {
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }

        public AddAccountViewModel MapBack(AccountDTO destination)
        {
            return new AddAccountViewModel()
            {
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




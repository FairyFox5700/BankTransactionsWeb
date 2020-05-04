using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
{
    public class UpdateAccountToAccountDTOMapper : IMapper< UpdateAccountViewModel, AccountDTO>
    {
        private UpdateAccountToAccountDTOMapper() { }
        public static readonly UpdateAccountToAccountDTOMapper Instance = new UpdateAccountToAccountDTOMapper();
        public AccountDTO Map(UpdateAccountViewModel source)
        {
            return new AccountDTO()
            {
                Id = source.Id,
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }

        public UpdateAccountViewModel MapBack(AccountDTO destination)
        {
            return new UpdateAccountViewModel()
            {
                Id = destination.Id,
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




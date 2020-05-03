using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class AccountMapperUpdateModel : IMapper<AccountDTO, UpdateAccountViewModel>
    {
        private AccountMapperUpdateModel()
        {
        }

        public static AccountMapperUpdateModel Instance { get; private set; } = new AccountMapperUpdateModel();
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




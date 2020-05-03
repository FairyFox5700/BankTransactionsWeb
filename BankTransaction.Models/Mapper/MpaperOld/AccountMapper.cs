using System.Linq;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper.MpaperOld
{
    public class AccountMapper : IMapper<Account, AccountDTO>
    {
        public AccountDTO Map(Account source)
        {
            return new AccountDTO()
            {
                Id = source.Id,
                Balance = source.Balance,
                Number = source.Number,
                Transactions = source.Transactions.Select(tr => new TransactionMapper().Map(tr)).ToList()
            };
        }

        public Account MapBack(AccountDTO destination)
        {
            return new Account()
            {
                Id = destination.Id,
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
                Transactions = destination.Transactions.Select(tr => new TransactionMapper().MapBack(tr)).ToList(),
            };
        }
    }
}

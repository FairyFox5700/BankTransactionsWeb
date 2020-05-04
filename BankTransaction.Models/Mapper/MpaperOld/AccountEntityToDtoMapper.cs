using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using System.Linq;

namespace BankTransaction.Models.Mapper
{
    public class AccountEntityToDtoMapper : IMapper<Account, AccountDTO>
    {
        private AccountEntityToDtoMapper() { }
        public static readonly AccountEntityToDtoMapper Instance = new AccountEntityToDtoMapper();
        public AccountDTO Map(Account source)
        {
            return new AccountDTO()
            {
                Id = source.Id,
                Balance = source.Balance,
                PersonId = source.PersonId,
                Number = source.Number,
                Transactions = source.Transactions?.Select(tr => TransactionEntityToDtoMapper.Instance.Map(tr)).ToList()
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
                Transactions = destination.Transactions?.Select(tr => TransactionEntityToDtoMapper.Instance.MapBack(tr)).ToList(),
            };
        }
    }
}

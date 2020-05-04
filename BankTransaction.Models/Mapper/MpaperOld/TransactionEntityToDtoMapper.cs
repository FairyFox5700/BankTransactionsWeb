using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Mapper
{

    public class TransactionEntityToDtoMapper : IMapper<Transaction, TransactionDTO>
    {
        private TransactionEntityToDtoMapper() { }
        public static readonly TransactionEntityToDtoMapper Instance = new TransactionEntityToDtoMapper();
        public TransactionDTO Map(Transaction source)
        {
            return new TransactionDTO()
            {
                Id = source.Id,
                AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                DateOftransfering = source.DateOftransfering,
                Amount = source.Amount,
                SourceAccount = source.SourceAccount
            };
        }

        public Transaction MapBack(TransactionDTO destination)
        {
            return new Transaction()
            {
                Id = destination.Id,
                AccountDestinationId = destination.AccountDestinationId,
                AccountSourceId = destination.AccountSourceId,
                DateOftransfering = destination.DateOftransfering,
                Amount = destination.Amount,
                SourceAccount = destination.SourceAccount,
            };
        }
    }
}

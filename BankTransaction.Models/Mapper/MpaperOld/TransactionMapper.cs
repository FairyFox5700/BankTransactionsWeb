﻿using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper.MpaperOld
{

    public class TransactionMapper : IMapper<Transaction, TransactionDTO>
    {
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

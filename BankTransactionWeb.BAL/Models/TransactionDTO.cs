using System;
using BankTransaction.Entities;

namespace BankTransaction.Models.DTOModels
{
    public class TransactionDTO:BaseModel
    {
        public int AccountSourceId { get; set; }
        public Account SourceAccount { get; set; }
        public int AccountDestinationId { get; set; }
        public Account DestinationAccount { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }
    }
}
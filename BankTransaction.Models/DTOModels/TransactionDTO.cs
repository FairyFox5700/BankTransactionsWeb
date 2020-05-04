using BankTransaction.Entities;
using System;

namespace BankTransaction.BAL.Implementation.DTOModels
{
    public class TransactionDTO:BaseModel
    {
        public int AccountSourceId { get; set; }
        //public Account SourceAccount { get; set; }
        public string SourceAccountNumber { get; set; }
        public int AccountDestinationId { get; set; }
        //public Account DestinationAccount { get; set; }
        public string DestinationAccountNumber { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }
    }
}
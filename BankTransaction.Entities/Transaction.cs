using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTransaction.Entities
{
    public class Transaction : BaseEntity
    {
        public int AccountSourceId { get; set; }
        public int AccountDestinationId { get; set; }
        public Account SourceAccount { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }
    }
}
//public int AccountSourceId {get;set;}
//public int AccountDestinationId { get; set; }
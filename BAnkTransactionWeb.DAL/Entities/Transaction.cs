using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTransactionWeb.DAL.Entities
{
    public class Transaction : BaseEntity
    {
        [Required]
        public string Number { get; set; }
        public virtual Account AccountSource { get; set; }
        public virtual Account AccountDestination { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }
    }
}
//public int AccountSourceId {get;set;}
//public int AccountDestinationId { get; set; }
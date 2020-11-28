using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Entities
{
    public class Account:BaseEntity
    {
        [Required]
        [MaxLength(16)]
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public int? PersonId { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        //public virtual ICollection<Transaction> TransactionsForSource { get; set; }
        //public virtual ICollection<Transaction> TransactionsForDestination { get; set; }
    }
}
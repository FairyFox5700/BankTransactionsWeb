using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTransaction.Entities
{
    public class Company:BaseEntity
    {
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public virtual ICollection<Shareholder> Shareholders { get; set; }
    }
}

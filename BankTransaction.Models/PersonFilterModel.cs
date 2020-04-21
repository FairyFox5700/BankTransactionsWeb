using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models
{
    public class PersonFilterModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string CompanyName { get; set; }
    }
}

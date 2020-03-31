using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Models
{
    public class PersonDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public DateTime DataOfBirth { get; set; }
        private List<AccountDTO> accounts;
        public List<AccountDTO> Accounts 
        { get
            {
                return accounts ?? (accounts = new List<AccountDTO>());
            }
            set
            {
                accounts = value;
            }
        }
    }
}

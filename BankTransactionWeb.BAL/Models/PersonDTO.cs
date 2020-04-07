using System;
using System.Collections.Generic;

namespace BankTransactionWeb.BAL.Models
{
    public class PersonDTO : BaseModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public DateTime DataOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool RememberMe { get; set; }
        private List<AccountDTO> accounts;
        public List<AccountDTO> Accounts
        {
            get
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

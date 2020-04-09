using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Models
{
    public class PersonDTO:BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public DateTime DataOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public bool RememberMe { get; set; }
        public string PhoneNumber { get; set; }
        public string ApplicationUserId { get; set; }
        public string Code{ get; set; }
        //private List<AccountDTO> accounts;
        //public List<AccountDTO> Accounts 
        //{ get
        //    {
        //        return accounts ?? (accounts = new List<AccountDTO>());
        //    }
        //    set
        //    {
        //        accounts = value;
        //    }
        //}
    }
}

using System;

namespace BankTransaction.Models.DTOModels
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
        public bool RememberMe { get; set; } = false;
        public string PhoneNumber { get; set; }
        public string ApplicationUserFkId { get; set; }
        public string Token{ get; set; }
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

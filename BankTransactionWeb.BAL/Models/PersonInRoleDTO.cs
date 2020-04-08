using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Models
{
    public class PersonInRoleDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public bool IsSelected { get; set; }
    }
}

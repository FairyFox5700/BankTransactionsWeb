using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Models
{
    public class PersonSearchModel
    {
        public string Name { get; set; }
        [Display(Name = "Surname")]
        public string SurName { get; set; }
        [Display(Name = "Lastname")]
        public string LastName { get; set; }
        [Display(Name = "Account number")]
        public string AccoutNumber { get; set; }
        [Display(Name = "Transaction number")]
        public string AccountTransactionNumber { get; set; }
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }
    }
}

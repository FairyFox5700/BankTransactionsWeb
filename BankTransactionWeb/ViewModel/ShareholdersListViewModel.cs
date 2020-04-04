using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.ViewModel
{
    public class ShareholdersListViewModel
    {
        public List<ShareholderDTO> Shareholders { get; set; }
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of company creation")]
        public DateTime? DateOfCompanyCreation { get; set; }
    }
}

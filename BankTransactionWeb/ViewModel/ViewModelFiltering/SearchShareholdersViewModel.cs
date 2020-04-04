using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.ViewModel.ViewModelFiltering
{
    public class SearchShareholdersViewModel
    {
        [Display(Name ="Company name")]
        public string CompanyName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Date of company creation")]
        public DataType DateOfCompnayCreation { get; set; }
    }
}

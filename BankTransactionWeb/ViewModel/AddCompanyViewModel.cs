using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public  class AddCompanyViewModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name must be no longer than 30  characters long")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Date of creation")]
        [Range(typeof(DateTime), "1/1/1870", "1/1/2012",
    ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime DateOfCreation { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransactionWeb.ViewModel
{
    public  class AddCompanyViewModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name must be no longer than 30  characters long")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Date of creation")]
        public DateTime DateOfCreation { get; set; }
    }
}
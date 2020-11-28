using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public class UpdateCompanyViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Name must be no longer than 30  characters long")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of creation")]
        public DateTime DateOfCreation { get; set; }
    }
}
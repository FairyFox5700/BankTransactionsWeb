using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankTransactionWeb.ViewModel
{
    public class ExecuteTransactionViewModel
    {
        [Required]
        [Display(Name ="Account source number")]
        public int AccountSourceId { get; set; }
        [Required]
        [MinLength(16, ErrorMessage ="Length must be not less then 16 numbers")]
        [MaxLength(16, ErrorMessage = "Length must be not more then 16 numbers")]
        [Display(Name = "Account destination number")]
        public string AccountDestinationNumber { get; set; }
        public SelectList Accounts { get; set; }
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Amount { get; set; }
    }
}
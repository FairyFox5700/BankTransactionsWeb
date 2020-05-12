using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public class UpdateTransactionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Account source number")]
        public int AccountSourceId { get; set; }
        [Required]
        [MaxLength(16, ErrorMessage = "Length must be not more then 16 numbers")]
        [Display(Name = "Account destination number")]
        public string AccountDestinationNumber { get; set; }
        public string AccountSourceNumber { get; set; }
        public SelectList Accounts { get; set; }
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Amount { get; set; }
        //public int AccountSourceId { get; set; }
        //public int AccountDestinationId { get; set; }
        //public SelectList Accounts { get; set; }
        //[Range(0, 99999999999)]
        //[DataType(DataType.Currency)]
        //[Required]
        //public decimal Amount { get; set; }
    }
}
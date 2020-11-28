using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public class AddTransactionViewModel
    {
        [Required]
        public int AccountSourceId { get; set; }
        [Required]
        public int AccountDestinationId { get; set; }
        public SelectList Accounts { get; set; }
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
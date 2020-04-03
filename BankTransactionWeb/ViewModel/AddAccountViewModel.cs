using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankTransactionWeb.ViewModel
{
    public class AddAccountViewModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Number must be 16 characters long")]
        public string Number { get; set; }
        [Required]
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; } = 0;
        public SelectList People { get; set; }
        [Required]
        [Display(Name ="Current person id")]
        public int PersonId { get;set;}
    }
}
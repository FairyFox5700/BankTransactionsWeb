using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankTransactionWeb.ViewModel
{
    public class UpdateTransactionViewModel
    {
        public int Id { get; set; }
        public int AccountSourceId { get; set; }
        public int AccountDestinationId { get; set; }
        public SelectList Accounts { get; set; }
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Amount { get; set; }
    }
}
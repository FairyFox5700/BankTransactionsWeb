using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BankTransactionWeb.ViewModel
{
    public class UpdateTransactionViewModel
    {
        public int Id { get; set; }
        public int AccountSourceId { get; set; }
        public int AccountDestinationId { get; set; }
        public SelectList Accounts { get; set; }
        public decimal Amount { get; set; }
    }
}
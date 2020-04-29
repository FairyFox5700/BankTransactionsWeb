using System;
using System.Collections.Generic;

namespace BankTransactionWeb.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public List<string>  Message { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public ErrorViewModel()
        {
        }
        public ErrorViewModel(List<string> message)
        {
            Message = message;
        }
    }
}

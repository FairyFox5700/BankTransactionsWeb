using System.Collections.Generic;

namespace BankTransaction.Web.Models
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

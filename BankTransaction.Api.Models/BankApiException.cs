using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models
{
    public class BankApiException:Exception
    {
        public BankApiException()
        { }

        public BankApiException(string message)
            : base(message)
        { }

        public BankApiException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

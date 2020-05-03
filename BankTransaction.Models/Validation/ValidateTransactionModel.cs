using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Validation
{
    public class ValidateTransactionModel
    {
        public bool IsError { get; protected set; }
        public string Message{ get; protected set; }
        public ValidateTransactionModel(string message, bool isError)
        {
            Message = message;
            IsError = isError;
        }
    }
}

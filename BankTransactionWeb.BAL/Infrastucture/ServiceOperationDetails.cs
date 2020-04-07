using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class ServiceOperationDetails
    {
        public ServiceOperationDetails(bool succedeed, string message, string property)
        {
            Succedeed = succedeed;
            Message = message;
            Property = property;
        }
        public bool Succedeed { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.ViewModel
{
    public class TransactionListViewModel
    {
        public int AccountSourceId { get; set; }
        public string  AccountSourceNumber {get;set;}
        public string  AccountDestinationeNumber { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }

    }
}

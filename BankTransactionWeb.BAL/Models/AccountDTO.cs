using System.Collections.Generic;

namespace BankTransactionWeb.BAL.Models
{
    public class AccountDTO:BaseModel
    {
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public int? PersonId { get; set; }
        private List<TransactionDTO> transactions;
        public List<TransactionDTO> Transactions
        {
            get
            {
                return transactions ?? (transactions = new List<TransactionDTO>());
            }
            set
            {
                transactions = value;
            }
        }
    }
}
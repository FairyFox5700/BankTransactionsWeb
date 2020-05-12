using System.Collections.Generic;

namespace BankTransaction.BAL.Implementation.DTOModels
{
    public class AccountDTO:BaseModel
    {
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public int PersonId { get; set; }
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
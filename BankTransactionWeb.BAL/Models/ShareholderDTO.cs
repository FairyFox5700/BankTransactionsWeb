namespace BankTransactionWeb.BAL.Models
{
    public class ShareholderDTO:BaseModel
    {
        public int? PersonId { get; set; }
        public int? CompanyId { get; set; }
    }
}
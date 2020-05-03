namespace BankTransaction.Entities
{
    public class Shareholder:BaseEntity
    {
        public int? PersonId { get; set; }
        public int? CompanyId { get; set; }
        public virtual Person Person { get; set; }
        public virtual Company Company { get; set; }
    }
}
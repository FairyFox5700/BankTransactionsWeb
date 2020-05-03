namespace BankTransaction.Models.DTOModels
{
    public class ShareholderDTO : BaseModel
    {
        public PersonDTO Person { get; set; }
        public CompanyDTO Company {get;set;}
        public int? PersonId { get; set; }
        public int? CompanyId { get; set; }
    }
}
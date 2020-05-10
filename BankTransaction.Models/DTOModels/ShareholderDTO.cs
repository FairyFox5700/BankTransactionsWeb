namespace BankTransaction.Models.DTOModels
{
    public class ShareholderDTO : BaseModel
    {
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonSurName{ get; set; }
       // public PersonDTO Person { get; set; }
        //public CompanyDTO Company {get;set;}
        public string CompanyName { get; set; }
        public int PersonId { get; set; }
        public int CompanyId { get; set; }
    }
}
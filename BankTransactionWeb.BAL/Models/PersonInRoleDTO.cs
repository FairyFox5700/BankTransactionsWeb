namespace BankTransaction.Models.DTOModels
{
    public class PersonInRoleDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string AppUserId { get; set; }
        public bool IsSelected { get; set; }
    }
}

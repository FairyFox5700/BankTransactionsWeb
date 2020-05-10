using System.Collections.Generic;

namespace BankTransaction.Models.DTOModels
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}

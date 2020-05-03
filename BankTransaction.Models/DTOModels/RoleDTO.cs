using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.DTOModels
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}

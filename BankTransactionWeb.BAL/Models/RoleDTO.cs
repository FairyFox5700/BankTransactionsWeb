using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Models
{
    public class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}

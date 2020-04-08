using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Admin.Models
{
    public class AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

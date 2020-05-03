using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Areas.Admin.Models.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Areas.Admin.Models.ViewModels
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<string> Users { get; set; } = new List<string>();
    }
}

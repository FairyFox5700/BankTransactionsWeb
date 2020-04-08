using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Admin.Models.ViewModels
{
    public class ListRoleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}


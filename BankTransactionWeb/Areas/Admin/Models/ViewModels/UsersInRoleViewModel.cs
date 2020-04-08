using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Admin.Models.ViewModels
{
    public class UsersInRoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "User name")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Surname { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        public bool IsSelected { get; set; }
    }
}

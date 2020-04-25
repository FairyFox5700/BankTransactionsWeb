using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.ViewModel
{
    public class UpdateAccountViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Number must be 16 characters long")]
        public string Number { get; set; }
        [Required]
        [Range(0, 99999999999)]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; } = 0;
        public PersonDTO Person { get; set; }
        [Required]
        [Display(Name = "Current person id")]
        public int PersonId { get; set; }
       // public string ApplicationUserFkId { get; set; }
    }
}

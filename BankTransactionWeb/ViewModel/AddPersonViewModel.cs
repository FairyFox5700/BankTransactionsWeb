using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.ViewModel
{
    public class AddPersonViewModel
    {
        [Required]
        [MaxLength(30,ErrorMessage = "Name cannot be longer than 30 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Surname cannot be longer than 30 characters")]
        public string Surname { get; set; }
        [Required]
        [MaxLength(35, ErrorMessage = "LastName cannot be longer than 35 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name="Date of birth")]
        public DateTime DataOfBirth { get; set; }
    }
}

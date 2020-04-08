using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.ViewModel
{
    public class UpdatePersonViewModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "User name")]
        [MaxLength(20)]//, ErrorMessage = "Your user name must be  max 20  characters long.")
        public string UserName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]//, ErrorMessage = "Name cannot be longer than 30 characters and no less than 3 characters")
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]//, ErrorMessage = "Surname cannot be longer than 30 characters  and no less than 3 characters"
        public string Surname { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]//, ErrorMessage = "Last name cannot be longer than 30 characters and no less than 3 characters"
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DataOfBirth { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
    }
}

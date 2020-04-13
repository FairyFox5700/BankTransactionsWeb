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

        [Display(Name = "User name")]
        [MaxLength(20)]//, ErrorMessage = "Your user name must be  max 20  characters long.")
        public string UserName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]//, ErrorMessage = "Name cannot be longer than 30 characters and no less than 3 characters")
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Surname { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Date of birth")]
        [Range(typeof(DateTime), "1/1/1870", "1/1/2012",
    ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime DataOfBirth { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber{ get; set; }
        public string ApplicationUserFkId { get; set; }
    }
}

using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public class AddShareholderViewModel
    {
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public PersonDTO Person { get; set; }
        public string CompanyName { get; set; }
       // public SelectList People{ get; set; }
        public SelectList Comapnanies { get; set; }
    }
}
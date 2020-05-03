using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankTransaction.Web.ViewModel
{
    public class UpdateShareholderViewModel
    {
        public int Id { get; set; }
        [Required]
        public int? PersonId { get; set; }
        [Required]
        public int? CompanyId { get; set; }
        public PersonDTO Person { get; set; }
        public CompanyDTO Company { get; set; }
        //public SelectList People { get; set; }
        public SelectList Comapnanies { get; set; }
        public UpdateShareholderViewModel()
        {
            this.Comapnanies = new SelectList(new List<CompanyDTO>()) ;
        }
    }
}
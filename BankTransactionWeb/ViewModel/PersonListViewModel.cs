using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.ViewModel
{
    public class PersonListViewModel
    {
        public List<PersonDTO> Persons { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

    }
}

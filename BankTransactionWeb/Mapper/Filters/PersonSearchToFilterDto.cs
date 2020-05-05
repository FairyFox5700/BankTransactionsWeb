using BankTransaction.Configuration;
using BankTransaction.Models;
using BankTransaction.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper.Filters
{
    public class PersonSearchToFilterDto : IMapper<PersonSearchModel, PersonFilterModel>
    {
        private PersonSearchToFilterDto() { }

        public static readonly PersonSearchToFilterDto Instance = new PersonSearchToFilterDto();
        public PersonFilterModel Map(PersonSearchModel source)
        {
            return new PersonFilterModel()
            {
                Name = source.Name,
                Surname = source.SurName,
                LastName = source.LastName,
                CompanyName = source.CompanyName
            };
        }

        public PersonSearchModel MapBack(PersonFilterModel destination)
        {
            return new PersonSearchModel()
            {
                Name = destination.Name,
                SurName = destination.Surname,
                LastName = destination.LastName,
                CompanyName = destination.CompanyName
            };
        }
    }
}

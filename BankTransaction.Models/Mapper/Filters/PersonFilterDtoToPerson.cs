using BankTransaction.Configuration;
using BankTransaction.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Mapper.Filters
{
    public class PersonFilterDtoToPerson : IMapper<PersonFilterModel, PersonFilter>
    {
        private PersonFilterDtoToPerson() { }
        public static readonly PersonFilterDtoToPerson Instance = new PersonFilterDtoToPerson();
        public PersonFilter Map(PersonFilterModel source)
        {
            return new PersonFilter()
            {
                Name = source.Name,
                Surname = source.Surname,
                LastName = source.LastName,
                AccountNumber = source.AccountNumber,
                TransactionNumber = source.TransactionNumber,
                CompanyName = source.CompanyName
            };
        }

        public PersonFilterModel MapBack(PersonFilter destination)
        {
            return new PersonFilterModel()
            {
                Name = destination.Name,
                Surname = destination.Surname,
                LastName = destination.LastName,
                AccountNumber = destination.AccountNumber,
                TransactionNumber = destination.TransactionNumber,
                CompanyName = destination.CompanyName
            };
        }
    }
}

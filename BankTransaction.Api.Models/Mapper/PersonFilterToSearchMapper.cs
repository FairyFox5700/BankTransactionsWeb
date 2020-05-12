using BankTransaction.Api.Models.Queries;
using BankTransaction.Configuration;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class PersonFilterToSearchMapper : IMapper<PersonFilterModel, SearchPersonQuery>
    {
        private PersonFilterToSearchMapper() { }

        public static readonly PersonFilterToSearchMapper Instance = new PersonFilterToSearchMapper();
        public SearchPersonQuery Map(PersonFilterModel source)
        {
            return new SearchPersonQuery()
            {
                Name = source.Name,
                Surname = source.Surname,
                LastName = source.LastName,
                AccountNumber = source.AccountNumber,
                TransactionNumber = source.TransactionNumber,
                CompanyName = source.CompanyName
            };
        }

        public PersonFilterModel MapBack(SearchPersonQuery destination)
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

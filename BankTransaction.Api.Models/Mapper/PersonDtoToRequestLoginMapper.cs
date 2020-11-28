using BankTransaction.Api.Models.Queries;
using BankTransaction.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Models.Mapper
{
    public class PersonDtoToRequestLoginMapper : IMapper<PersonDTO, RequestLoginModel>
    {
        private PersonDtoToRequestLoginMapper() { }

        public static readonly PersonDtoToRequestLoginMapper Instance = new PersonDtoToRequestLoginMapper();
        public RequestLoginModel Map(PersonDTO source)
        {
            return new RequestLoginModel()
            {
                Email = source.Email,
                Password = source.Password
            };
        }

        public PersonDTO MapBack(RequestLoginModel destination)
        {
            return new PersonDTO()
            {
                Email = destination.Email,
                Password = destination.Password
            };
        }
    }
}

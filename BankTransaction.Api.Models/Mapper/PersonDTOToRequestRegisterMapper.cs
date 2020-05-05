using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class PersonDTOToRequestRegisterMapper : IMapper<PersonDTO, RequestRegisterModel>
    {
        private PersonDTOToRequestRegisterMapper() { }

        public static readonly PersonDTOToRequestRegisterMapper Instance = new PersonDTOToRequestRegisterMapper();
        public RequestRegisterModel Map(PersonDTO source)
        {
            return new RequestRegisterModel()
            {
                Email = source.Email,
                Password = source.Password,
                ConfirmPassword = source.ConfirmPassword,
                UserName = source.UserName,
                Name = source.Name,
                Surname = source.Surname,
                LastName = source.LastName,
                DataOfBirth = source.DataOfBirth,
                PhoneNumber = source.PhoneNumber
            };
        }

        public PersonDTO MapBack(RequestRegisterModel destination)
        {
            return new PersonDTO()
            {
                Name = destination.Name,
                Surname = destination.Surname,
                LastName = destination.LastName,
                DataOfBirth = destination.DataOfBirth,
                Email = destination.Email,
                Password = destination.Password,
                ConfirmPassword = destination.ConfirmPassword,
                UserName = destination.UserName,
                PhoneNumber = destination.PhoneNumber
            };
        }
    }
}

using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using System;

namespace BankTransaction.Models.Mapper
{
    public class PersonRoleToDtoMapper : IMapper<PersonInRoleDTO, Person>
    {
        private PersonRoleToDtoMapper() { }
        public static readonly PersonRoleToDtoMapper Instance = new PersonRoleToDtoMapper();
        public Person Map(PersonInRoleDTO source)
        {
            return new Person()
            {
                ApplicationUserFkId = source.AppUserId,
                Id = Convert.ToInt32(source.Id),
                LastName = source.LastName,
                Name = source.Name,
                Surname = source.Surname,
            };

        }

        public PersonInRoleDTO MapBack(Person destination)
        {
            return new PersonInRoleDTO()
            {
                AppUserId = destination.ApplicationUserFkId,
                LastName = destination.LastName,
                Name = destination.Name,
                Surname = destination.Surname,
                UserName = destination.ApplicationUser?.UserName,

            };
        }


    }
}

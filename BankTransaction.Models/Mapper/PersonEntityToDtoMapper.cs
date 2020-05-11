using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper
{
    public class PersonEntityToDtoMapper : IMapper<Person, PersonDTO>
    {
        private PersonEntityToDtoMapper() { }
        public static readonly PersonEntityToDtoMapper Instance = new PersonEntityToDtoMapper();
        public PersonDTO Map(Person source)
        {
            return new PersonDTO()
            {
                Id = source.Id,
                DataOfBirth = source.DataOfBirth,
                ApplicationUserFkId = source.ApplicationUserFkId,
                LastName = source.LastName,
                Name = source.Name,
                Surname = source.Surname
            };
        }

        public Person MapBack(PersonDTO destination)
        {
            return new Person()
            {
                Id = destination.Id,
                DataOfBirth = destination.DataOfBirth,
                ApplicationUserFkId = destination.ApplicationUserFkId,
                LastName = destination.LastName,
                Name = destination.Name,
                Surname = destination.Surname
            };
        }
    }
}

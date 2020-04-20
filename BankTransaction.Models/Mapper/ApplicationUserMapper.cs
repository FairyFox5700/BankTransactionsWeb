using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;

namespace BankTransaction.Models.Mapper
{
    public class ApplicationUserMapper : IMapper<ApplicationUser, PersonDTO>
    {
        public PersonDTO Map(ApplicationUser source)
        {
            return new PersonDTO()
            {
                ApplicationUserFkId = source.Id,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber
            };
        }

        public ApplicationUser MapBack(PersonDTO destination)
        {
            return new ApplicationUser()
            {
                Id = destination.ApplicationUserFkId,
                Email = destination.Email,
                PhoneNumber = destination.PhoneNumber
            };

        }
    }
}

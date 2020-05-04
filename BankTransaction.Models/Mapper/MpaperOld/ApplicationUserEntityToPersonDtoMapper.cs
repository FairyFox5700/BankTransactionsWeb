using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;

namespace BankTransaction.Models.Mapper
{
    public class ApplicationUserEntityToPersonDtoMapper : IMapper<ApplicationUser, PersonDTO>
    {
        private ApplicationUserEntityToPersonDtoMapper() { }
        public static readonly ApplicationUserEntityToPersonDtoMapper Instance = new ApplicationUserEntityToPersonDtoMapper();
        public PersonDTO Map(ApplicationUser source)
        {
            return new PersonDTO()
            {
                ApplicationUserFkId = source.Id,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                UserName = source.UserName
               
          
            };
        }

        public ApplicationUser MapBack(PersonDTO destination)
        {
            return new ApplicationUser()
            {
                Id = destination.ApplicationUserFkId,
                Email = destination.Email,
                PhoneNumber = destination.PhoneNumber,
                UserName = destination.UserName


            };

        }
    }
}

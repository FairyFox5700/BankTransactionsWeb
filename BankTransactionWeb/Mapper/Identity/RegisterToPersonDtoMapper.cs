using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Identity
{

    public class RegisterToPersonDtoMapper : IMapper<RegisterViewModel, PersonDTO>
    {
        private RegisterToPersonDtoMapper() { }

        public static readonly RegisterToPersonDtoMapper Instance = new RegisterToPersonDtoMapper();
        public PersonDTO Map(RegisterViewModel source)
        {
            return new PersonDTO()
            {
                ConfirmPassword = source.ConfirmPassword,
                DataOfBirth = source.DataOfBirth,
                Email = source.Email,
                LastName = source.LastName,
                Name = source.Name,
                Password = source.Password,
                PhoneNumber = source.PhoneNumber,
                Surname = source.Surname,
                UserName = source.UserName,
            };
        }

        public RegisterViewModel MapBack(PersonDTO destination)
        {
            return new RegisterViewModel()
            {
                ConfirmPassword = destination.ConfirmPassword,
                DataOfBirth = destination.DataOfBirth,
                Email = destination.Email,
                LastName = destination.LastName,
                Name = destination.Name,
                Password = destination.Password,
                PhoneNumber = destination.PhoneNumber,
                Surname = destination.Surname,
                UserName = destination.UserName,
            };

        }
    }

   
}




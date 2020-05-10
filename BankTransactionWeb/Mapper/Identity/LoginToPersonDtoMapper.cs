using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Identity
{

    public class LoginToPersonDtoMapper : IMapper<LoginViewModel, PersonDTO>
    {
        private LoginToPersonDtoMapper() { }

        public static readonly LoginToPersonDtoMapper Instance = new LoginToPersonDtoMapper();
        public PersonDTO Map(LoginViewModel source)
        {
            return new PersonDTO()
            {
                Email = source.Email,
                Password = source.Password,
                RememberMe = source.RememberMe
            };
        }

        public LoginViewModel MapBack(PersonDTO destination)
        {
            return new LoginViewModel()
            {
                Email = destination.Email,
                Password = destination.Password,
                RememberMe = destination.RememberMe
            };
        }
    }

   
}




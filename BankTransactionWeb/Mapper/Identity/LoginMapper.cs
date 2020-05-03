using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Identity
{

    public class LoginMapper : IMapper<LoginViewModel, PersonDTO>
    {
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




using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Identity
{

    public class ResetPasswordMapper : IMapper<ResetPasswordViewModel, PersonDTO>
    {
        public PersonDTO Map(ResetPasswordViewModel source)
        {
            return new PersonDTO()
            {
                ConfirmPassword = source.ConfirmPassword,
                Email = source.Email,
                Password = source.Password,
                Token = source.Token
            };
        }

        public ResetPasswordViewModel MapBack(PersonDTO destination)
        {
            return new ResetPasswordViewModel()
            {
                ConfirmPassword = destination.ConfirmPassword,
                Email = destination.Email,
                Password = destination.Password,
                Token = destination.Token
            };
        }
    }

   
}




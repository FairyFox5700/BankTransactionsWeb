using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Identity
{

    public class ResetPasswordToPersonDtoMapper : IMapper<ResetPasswordViewModel, PersonDTO>
    {
        private ResetPasswordToPersonDtoMapper() { }

        public static readonly ResetPasswordToPersonDtoMapper Instance = new ResetPasswordToPersonDtoMapper();
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




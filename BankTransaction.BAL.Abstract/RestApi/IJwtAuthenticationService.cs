using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;

namespace BankTransaction.BAL.Abstract.RestApi
{
    public interface IJwtAuthenticationService
    {
       // Task RefreshToken(RefreshTokenDTO model);
        Task<AuthResult> LoginPerson(PersonDTO person);
        Task<AuthResult> RegisterPersonWithJwtToken(PersonDTO person);
    }
}
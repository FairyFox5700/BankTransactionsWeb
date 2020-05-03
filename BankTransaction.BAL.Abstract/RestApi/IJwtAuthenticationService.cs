using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;

namespace BankTransaction.BAL.Abstract.RestApi
{
    public interface IJwtAuthenticationService
    {
       // Task RefreshToken(RefreshTokenDTO model);
        Task<AuthResult> LoginPerson(string email, string password);
        Task<AuthResult> RegisterPersonWithJwtToken(PersonDTO person);
    }
}
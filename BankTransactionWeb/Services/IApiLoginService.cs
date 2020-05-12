using BankTransaction.Api.Models.Responces;
using BankTransaction.Models.DTOModels;
using System.Threading.Tasks;

namespace BankTransaction.Web.Services
{
    public interface IApiLoginService
    {
        Task<ApiDataResponse<AuthSuccesfullModel>> LoginPerson(PersonDTO person);
        Task<ApiDataResponse<AuthSuccesfullModel>> RefreshToken(string token, string refreshToken);
    }
}

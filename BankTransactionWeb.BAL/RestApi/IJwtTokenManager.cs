using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public interface IJwtTokenManager
    {
        Task<bool> IsCurrentTokenActive();
        Task DeactivateCurrentTokenAsync();
        Task<bool> IsTokenActiveAsync(string token);
        Task DeactivateTokenAsync(string token);
    }
}

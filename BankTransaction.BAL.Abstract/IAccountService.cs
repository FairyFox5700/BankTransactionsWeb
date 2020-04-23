using BankTransaction.BAL.Implementation.DTOModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IAccountService : IDisposable
    {
        Task<IEnumerable<AccountDTO>> GetAllAccounts();
        Task<AccountDTO> GetAccountById(int id);
        Task AddAccount(AccountDTO account);
        Task UpdateAccount(AccountDTO account);
        Task DeleteAccount(AccountDTO account);
        Task<IEnumerable<AccountDTO>> GetMyAccounts(ClaimsPrincipal user);
        string GenerateCardNumber(int numberOfDigits);
    }
}

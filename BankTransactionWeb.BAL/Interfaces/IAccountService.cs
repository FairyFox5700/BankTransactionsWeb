using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<IEnumerable<AccountDTO>> GetAllAccounts();
        Task<AccountDTO> GetAccountById(int id);
        Task AddAccount(AccountDTO account);
        Task UpdateAccount(AccountDTO account);
        Task DeleteAccount(AccountDTO account);
        Task<IEnumerable<AccountDTO>> GetMyAccounts(ClaimsPrincipal user);
        string GenrateCardNumber(int numberOfDigits);
    }
}

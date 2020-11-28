using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
    public interface IAccountService : IDisposable
    {
        Task<PaginatedModel<AccountDTO>> GetAllAccounts(int pageNumber, int pageSize);
        Task<AccountDTO> GetAccountById(int id);
        Task AddAccount(AccountDTO account);
        Task UpdateAccount(AccountDTO account);
        Task DeleteAccount(AccountDTO account);
        Task<IEnumerable<AccountDTO>> GetMyAccounts(ClaimsPrincipal user);
        string GenerateCardNumber(int numberOfDigits);
    }
}

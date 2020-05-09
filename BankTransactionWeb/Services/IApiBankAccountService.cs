using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Implementation.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Services
{
    public  interface IApiBankAccountService
    {
        Task<ApiDataResponse<List<AccountDTO>>> GetAllAccounts(PageQueryParameters pageQueryParameters = null);
        Task<ApiDataResponse<AccountDTO>> UpdateAccount(AccountDTO account);
        Task<ApiDataResponse<AccountDTO>> DeleteAccount(int id);
        Task<ApiDataResponse<AccountDTO>> AddAccount(AccountDTO account);
    }
}

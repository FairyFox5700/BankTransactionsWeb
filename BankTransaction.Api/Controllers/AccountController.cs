using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Api.Helpers;
using System.Net;
using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BankTransaction.Api.Models.Queries;

namespace BankTransaction.Api.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }
        // GET /api/Account
        [HttpGet]
        [Cached(2000)]
        public async Task<ApiResponse<IEnumerable<AccountDTO>>> GetAllAccounts([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var accounts = (await accountService.GetAllAccounts(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return new ApiResponse<IEnumerable<AccountDTO>>(accounts);
        }
        // PUT /api/Account/{id}
        [HttpPut("{id}")]
        public async Task<ApiResponse<int>> UpdateAccount(int id, AccountDTO account)
        {
            if (id != account.Id)
            {
                return ApiResponse<int>.BadRequest;
            }
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                return ApiResponse<int>.NotFound;
            }
            await accountService.UpdateAccount(account);
            return new ApiResponse<int>(id);

        }

        // POST: api/Account
        [HttpPost]
        public async Task<ApiResponse<AccountDTO>> AddAccount(AccountDTO account)
        {
                await accountService.AddAccount(account);
            return new ApiResponse<AccountDTO>(account);
        }
        // DELETE /api/Account/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResponse<AccountDTO>> DeleteAccount(int id)
        {
            var account = await accountService.GetAccountById(id);
            if (account == null)
            {
                return ApiResponse<AccountDTO>.NotFound;
            }
            await accountService.DeleteAccount(account);
            return new ApiResponse<AccountDTO>(account);
        }
    }
}
using BankTransaction.BAL.Abstract;
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
using BankTransaction.Api.Models.Responces;
using BankTransaction.Models.DTOModels;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BankTransaction.Api.Controllers
{
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ApiDataResponse<PaginatedList<AccountDTO>>> GetAllAccounts([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var accounts = (await accountService.GetAllAccounts(pageQueryParameters.PageNumber, pageQueryParameters.PageSize));
            var listOfaccounts= new PaginatedList<AccountDTO>(accounts);
            return new ApiDataResponse<PaginatedList<AccountDTO>>(listOfaccounts);
        }
        // PUT /api/Account/{id}
        [HttpPut("{id}")]
        public async Task<ApiDataResponse<AccountDTO>> UpdateAccount(int id, [FromBody]AccountDTO account)
        {
            if (id != account.Id)
            {
                return ApiDataResponse<AccountDTO>.BadRequest;
            }
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                return ApiDataResponse<AccountDTO>.NotFound;
            }
            await accountService.UpdateAccount(account);
            return new ApiDataResponse<AccountDTO>(account);

        }

        // POST: api/Account
        [HttpPost]
        public async Task<ApiDataResponse<AccountDTO>> AddAccount([FromBody]AccountDTO account)
        {
                await accountService.AddAccount(account);
            return new ApiDataResponse<AccountDTO>(account);
        }
        // DELETE /api/Account/{id}
        [HttpDelete("{id}")]
        public async Task<ApiDataResponse<AccountDTO>> DeleteAccount(int id)
        {
            var account = await accountService.GetAccountById(id);
            if (account == null)
            {
                return ApiDataResponse<AccountDTO>.NotFound;
            }
            await accountService.DeleteAccount(account);
            return new ApiDataResponse<AccountDTO>(account);
        }
    }
}
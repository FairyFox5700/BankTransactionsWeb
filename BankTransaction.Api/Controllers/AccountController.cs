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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
       // [Authorize(Roles = "Admin")]
        [Cached(2000)]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var accounts = (await accountService.GetAllAccounts(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return Ok(accounts);
        }
        // PUT /api/Account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDTO account)
        {
            if (id != account.Id)
            {
                return  BadRequest(new ApiErrorResponse { Message = "Id and request id does not match" });
            }
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                return NotFound(new ApiErrorResponse { Message = "Current account not found" });
            }
            await accountService.UpdateAccount(account);
            return Ok(currentAccount);

        }

        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> AddAccount(AccountDTO account)
        {
            if (account == null)
            {
                return BadRequest(new ApiErrorResponse { Message = "Query object is null" });
            }
            else
            {
                await accountService.AddAccount(account);
                return Ok(account);
            }
        }
        // DELETE /api/Account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {

            var account = await accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound(new ApiErrorResponse { Message = "Query object is null" ,ValidationErrors=null});
            }
            await accountService.DeleteAccount(account);
            //return Ok("Deleted succesfully");
            return NotFound(new ApiErrorResponse { Message = "Query object is nu2ll", ValidationErrors = null });

        }
    }
}
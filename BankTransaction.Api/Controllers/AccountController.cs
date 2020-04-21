using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Api.Helpers;

namespace BankTransaction.Api.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin")]
        [Cached(200)]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {

            var accounts = (await accountService.GetAllAccounts()).ToList();
            logger.LogInformation("Successfully returned all accounts");
            return accounts;
        }
        // PUT /api/Account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDTO account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                logger.LogError($"Account with id {id} not find");
                return NotFound();
            }

            await accountService.UpdateAccount(account);
            return Ok(currentAccount);

        }

        // POST: api/AAccount
        [HttpPost]
        public async Task<IActionResult> AddAccount(AccountDTO account)
        {
            if (account == null)
            {
                logger.LogError("Object of type account send by client was null.");
                return BadRequest("Object of type account is null");
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
                logger.LogError($"Account with id {id} not find");
                return NotFound();
            }
            await accountService.DeleteAccount(account);
            return Ok("Deleted succesfully");

        }
    }
}
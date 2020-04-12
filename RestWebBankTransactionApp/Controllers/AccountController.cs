using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RestWebBankTransactionApp.Controllers
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
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            try
            {
                var accounts = (await accountService.GetAllAccounts()).ToList();
                logger.LogInformation("Successfully returned all accounts");
                return accounts;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllAccounts)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
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
            try
            {
                await accountService.UpdateAccount(account);
                return Ok(currentAccount);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to update account becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                return NotFound();
            }

        }

        // POST: api/AAccount
        [HttpPost]
        public async Task<IActionResult> AddAccount(AccountDTO account)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddAccount)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // DELETE /api/Account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var account = await accountService.GetAccountById(id);
                if (account == null)
                {
                    logger.LogError($"Account with id {id} not find");
                    return NotFound();
                }
                try
                {
                    await accountService.DeleteAccount(account);
                    return Ok("Deleted succesfully");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update account becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteAccount)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
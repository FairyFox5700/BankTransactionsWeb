using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankTransactionWeb.BAL.Infrastucture;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankTransactionWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IPersonService personService;
        private readonly ILogger<AccountController> logger;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IPersonService personService, ILogger<AccountController> logger, IMapper mapper)
        {
        
            this.accountService = accountService;
            this.personService = personService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts =(await accountService.GetAllAccounts()).ToList();//maybe sort them
                logger.LogInformation("Successfully returned all accounts");
                return View(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllAccounts)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> AddAccount()
        {
            var accountVM = new AddAccountViewModel()
            {
                People = new SelectList(await personService.GetAllPersons(), "Id", "Name", "Surname", "LastName")
            };

            return View(accountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccount(AddAccountViewModel accountModel)
        {
            try
            {
                if (accountModel == null)
                {
                    logger.LogError($"Object of type {typeof(AddAccountViewModel)} send by client was null.");
                    return BadRequest("Object of type account is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError($"Account model send by client is not valid.");
                    return BadRequest("Account model is not valid.");
                }
                else
                {
                    var account = mapper.Map<AccountDTO>(accountModel);
                    await accountService.AddAccount(account);
                    return RedirectToAction(nameof(GetAllAccounts));
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddAccount)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int id)
        {
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                logger.LogError($"Account with id {id} not find");
                return NotFound();
            }
            else
            {
                var accountModel = mapper.Map<UpdateAccountViewModel>(currentAccount);
                accountModel.People = new SelectList(await personService.GetAllPersons(), "Id", "Name", "Surname", "LastName");
                return View(accountModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount([FromForm]UpdateAccountViewModel accountModel)
        {
            try
            {
                if (accountModel == null)
                {
                    logger.LogError($"Object of type {typeof(AddAccountViewModel)} send by client was null.");
                    return BadRequest("Object of type account is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Account model send by client is not valid.");
                    return View(accountModel);
                }
                else
                {
                    try
                    {
                        var account = await accountService.GetAccountById(accountModel.Id);
                        if (account == null)
                        {
                            logger.LogError($"Account with id {accountModel.Id} not find");
                            return NotFound();
                        }
                        else
                        {
                            var updatedAccount= mapper.Map<UpdateAccountViewModel, AccountDTO>(accountModel, account);
                            await accountService.UpdateAccount(updatedAccount);
                            return RedirectToAction(nameof(GetAllAccounts));
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError($"Unable to update person becuase of {ex.Message}");
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                        return View(accountModel);
                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateAccount)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


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
                    return RedirectToAction(nameof(GetAllAccounts));
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update person becuase of {ex.Message}");
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


        protected override void Dispose(bool disposing)
        {
            accountService.Dispose();
            personService.Dispose();
            base.Dispose(disposing);
        }

    }
}
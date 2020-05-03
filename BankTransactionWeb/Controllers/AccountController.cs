using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankTransaction.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> logger;
        private readonly IMapper mapper;
        private readonly IPersonService personService;

        public AccountController(IAccountService accountService, IPersonService personService,
            ILogger<AccountController> logger, IMapper mapper)
        {
            this.accountService = accountService;
            this.personService = personService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts(PageQueryParameters pageQueryParameters = null)
        {
            var allAccounts =
                await accountService.GetAllAccounts(pageQueryParameters.PageNumber, pageQueryParameters.PageSize);
            var listOfaccountsVM = new PaginatedList<AccountDTO>(allAccounts);
            return View(listOfaccountsVM);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddAccount(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person != null)
            {
                var accountVM = new AddAccountViewModel
                {
                    Number = accountService.GenerateCardNumber(16),
                    Person = person,
                    PersonId = id
                };
                return View(accountVM);
            }

            ;
            return NotFound("Sorry. Current user not found");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddAccount(AddAccountViewModel accountModel)
        {
            if (!ModelState.IsValid) return BadRequest("Account model is not valid.");

            var account = mapper.Map<AccountDTO>(accountModel);
            await accountService.AddAccount(account);
            return RedirectToAction(nameof(GetAllAccounts));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount(int id)
        {
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                logger.LogError($"Account with id {id} not find");
                return NotFound();
            }

            var accountModel = mapper.Map<UpdateAccountViewModel>(currentAccount);
            return View(accountModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountViewModel accountModel)
        {
            if (accountModel == null) return BadRequest("Object of type account is null");
            if (!ModelState.IsValid)
                return View(accountModel);
            try
            {
                var account = await accountService.GetAccountById(accountModel.Id);
                if (account == null) return NotFound();

                var updatedAccount = mapper.Map(accountModel, account);
                await accountService.UpdateAccount(updatedAccount);
                return RedirectToAction(nameof(GetAllAccounts));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Unable to update account becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists, " +
                                             "see your system administrator.");
                return View(accountModel);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int id)
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

        [HttpGet]
        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 30000)]
        public async Task<IActionResult> MyAccounts()
        {
            var accounts = (await accountService.GetMyAccounts(HttpContext.User)).ToList();
            return View(accounts);
        }

        protected override void Dispose(bool disposing)
        {
            accountService.Dispose();
            personService.Dispose();
            base.Dispose(disposing);
        }
    }
}
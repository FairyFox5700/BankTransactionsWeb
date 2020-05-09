using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Web.Mapper;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    public class ApiBankAccountController : Controller
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly UrlsConfiguration urlsConfiguration;
        private readonly IPersonService personService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAccountService accountService;

        public ApiBankAccountController(IRestApiHelper restApiHelper, UrlsConfiguration urlsConfiguration, IPersonService personService, IHttpContextAccessor  httpContextAccessor, IAccountService accountService)
        {
            this.restApiHelper = restApiHelper;
            this.urlsConfiguration = urlsConfiguration;
            this.personService = personService;
            this.httpContextAccessor = httpContextAccessor;
            this.accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts(PageQueryParameters pageQueryParameters = null)
        {
            string token = httpContextAccessor.HttpContext.Request.Cookies["BankWeb.AspNetCore.ProductKey"];
            // if (token != null)
            //var allAccounts = await accountService.GetAllAccounts(pageQueryParameters.PageNumber, pageQueryParameters.PageSize);
            var allAccount = await restApiHelper.ExecuteAsync<ApiResponse<List<AccountDTO>>>(urlsConfiguration.Account, null, Method.GET, parameters: new { pageQueryParameters.PageSize, pageQueryParameters.PageNumber }, token);

            var validateReult = ValidateApiResult(allAccount);
            if (validateReult != null)
            {
                return View(allAccount.Data);

            }
            return validateReult;
        }

        public async Task<IActionResult> AddAccount(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person != null)
            {
                var accountVM = new AddAccountViewModel()
                {
                    Number = accountService.GenerateCardNumber(16),
                    Person = person,
                    PersonId = id
                };
                return View(accountVM);
            };
            return NotFound("Sorry. Current user not found");
        }

        [Authorize]
        public async Task<IActionResult> AddAccount(AddAccountViewModel accountModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Account model is not valid.");
            }
            else
            {
                var account = AddAccountToAccountDTOMapper.Instance.Map(accountModel);
                var result = await restApiHelper.ExecuteAsync<ApiResponse<AccountDTO>>(urlsConfiguration.Account, body: account,  Method.POST);
                var validateReult = ValidateApiResult(result);
                if (validateReult != null)
                {
                    return RedirectToAction(nameof(GetAllAccounts));

                }
                return validateReult;
            }

        }
        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int id)
        {
            var currentAccount = await accountService.GetAccountById(id);
            if (currentAccount == null)
            {
                return NotFound();
            }
            else
            {
                var accountModel = UpdateAccountToAccountDTOMapper.Instance.MapBack(currentAccount);
                return View(accountModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount([FromForm]UpdateAccountViewModel accountModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountModel);
            }
            else
            {
                var updatedAccount = UpdateAccountToAccountDTOMapper.Instance.Map(accountModel);
                var result = await restApiHelper.ExecuteAsync<ApiResponse<AccountDTO>>(urlsConfiguration.Account, body: updatedAccount, Method.PUT, parameters:new { accountModel.Id });
                var validateReult = ValidateApiResult(result);
                if (validateReult != null)
                {
                    return RedirectToAction(nameof(GetAllAccounts));

                }
                return validateReult;
            }
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await restApiHelper.ExecuteAsync<ApiResponse<AccountDTO>>(urlsConfiguration.Account, null, Method.DELETE, parameters: new { id });
            var validateReult = ValidateApiResult(result);
            if (validateReult != null)
            {
                return RedirectToAction(nameof(GetAllAccounts));

            }
            return validateReult;


        }

        private ActionResult ValidateApiResult<T>(ApiResponse<T> result)
        {
            if (result.IsError)
            {
                return RedirectToAction("Error", "Home", result.ResponseException.Message);
            }
            return null;
        }
    }
}

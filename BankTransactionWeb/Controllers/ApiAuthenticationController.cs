using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.Helpers;
using BankTransaction.Web.Localization;
using BankTransaction.Web.Mapper.Identity;
using BankTransaction.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    public class ApiAuthenticationController : Controller
    {
        private readonly IApiLoginService apiLoginService;
        private readonly ICookieHelperService helperService;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public ApiAuthenticationController(IApiLoginService apiLoginService, ICookieHelperService helperService, IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.apiLoginService = apiLoginService;
            this.helperService = helperService;
            this.sharedLocalizer = sharedLocalizer;
        }

        [HttpGet]
        [AllowAnonymous]

        public ActionResult SignIn(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        #region test data to remove
        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string Test_PASSWORD = "qWerty1_";
        static readonly string RESOURCE = "Company";
        #endregion
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SignIn(LoginViewModel model)//change to login and then mapp
        {
            var person = LoginToPersonDtoMapper.Instance.Map(model);
            var authResult = await apiLoginService.LoginPerson(person);
            ValidateApiResult(authResult);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            return RedirectToAction("Index", "Home");
        }


        private ActionResult ValidateApiResult<T>(ApiDataResponse<T> result)//ValidateCustomnLogic
        {
            if (result.IsError)
            {
                return RedirectToAction("Error", "Home", result.ResponseException.Message);
            }
            return null;
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankTransaction.BAL.Implementation.Extensions
{
    class RedirectToLoginCookieAuthentication : CookieAuthenticationEvents
    {
        private IUrlHelperFactory helper;
        private IActionContextAccessor accessor;
        public RedirectToLoginCookieAuthentication(IUrlHelperFactory helper,IActionContextAccessor accessor)
        {
            this.helper = helper;
            this.accessor = accessor;
        }
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var routeData = context.Request.HttpContext.GetRouteData();
            var routeValues = routeData.Values;
            var uri = new Uri(context.RedirectUri);
            var returnUrl = HttpUtility.ParseQueryString(uri.Query)[context.Options.ReturnUrlParameter];
            var parameters = $"id={Guid.NewGuid().ToString()}";
            //routeValues.Add(context.Options.ReturnUrlParameter, $"{returnUrl}{parameters}");
            var urlHelper = helper.GetUrlHelper(accessor.ActionContext);
            context.RedirectUri = UrlHelperExtensions.Action(urlHelper, "SignIn", "ApiAuthentication", routeValues);
            return base.RedirectToLogin(context);
        }
    }
}

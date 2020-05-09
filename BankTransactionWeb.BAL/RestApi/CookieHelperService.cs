using BankTransaction.BAL.Abstract.RestApi;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public class CookieHelperService : ICookieHelperService
    {
        IHttpContextAccessor httpContextAccessor;

        public HttpContext HttpContext { get; }

        public CookieHelperService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.HttpContext = httpContextAccessor.HttpContext;
        }
        public void AddReplaceCookie(string cookieName, string cookieValue)
        {
           
            if (HttpContext.Request.Cookies[cookieName] == null)
            {
                // add cookie
                AddCookie(cookieName, cookieValue);
            }
            else
            {
                // ensure cookie value is correct 
                var existingSchoolCookie = HttpContext.Request.Cookies[cookieName];
                HttpContext.Response.Cookies.Delete(cookieName);
                AddCookie(cookieName, cookieValue);
            }
        }

        private void AddCookie(string cookieName, string cookieValue)
        {
            var options = new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60),
                IsEssential = true,
                HttpOnly = true
            };
           
            HttpContext.Response.Cookies.Append(cookieName, cookieValue, options);

        }


    }
   
   
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.BAL.Abstract.RestApi
{
    public interface ICookieHelperService
    {
        void AddReplaceCookie(string cookieName, string cookieValue);
    }
}

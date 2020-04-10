using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IAuthenticationService:IDisposable
    {
        public bool IsUserSignedIn(ClaimsPrincipal user);
        Task SignOutPerson();
        Task<SignInResult> LoginPerson(PersonDTO person);
        Task<IdentityResult> RegisterPerson(PersonDTO person);
        Task<IdentityResult> ConfirmUserEmailAsync(string email, string code);
        Task<IdentityResult> ResetPasswordForPerson(PersonDTO person);
        Task<bool> SendReserPasswordUrl(PersonDTO person);
    }
}

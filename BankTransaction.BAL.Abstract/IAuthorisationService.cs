using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IAuthenticationService:IDisposable
    {
        public bool IsUserSignedIn(ClaimsPrincipal user);
        Task SignOutPerson();
        Task<IdentityUserResult> LoginPerson(PersonDTO person);
        Task<IdentityUserResult> RegisterPerson(PersonDTO person);
        Task<IdentityUserResult> ConfirmUserEmailAsync(string email, string code);
        Task<IdentityUserResult>  ResetPasswordForPerson(PersonDTO person);
        Task<bool> SendResetPasswordUrl(PersonDTO person);
    }
}

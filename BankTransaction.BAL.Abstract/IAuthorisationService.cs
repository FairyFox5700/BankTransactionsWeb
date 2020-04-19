using BankTransaction.BAL.Implementation.DTOModels;
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
        Task<AuthResult> LoginPerson(PersonDTO person);
        Task<IdentityResult> RegisterPerson(PersonDTO person);
        Task<IdentityResult> ConfirmUserEmailAsync(string email, string code);
        Task<IdentityResult> ResetPasswordForPerson(PersonDTO person);
        public  Task<AuthResult> RegisterPersonWithJwtToken(PersonDTO person);
        Task<bool> SendReserPasswordUrl(PersonDTO person);
        Task RefreshToken(RefreshTokenDTO model);
    }
}

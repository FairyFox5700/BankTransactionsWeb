using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IAuthenticationService
    {
        Task SignOutPerson();
        Task<SignInResult> LoginPerson(PersonDTO person);
        Task<IdentityResult> RegisterPerson(PersonDTO person);
        Task<IdentityResult> ConfirmUserEmailAsync(string userId, string code);
    }
}

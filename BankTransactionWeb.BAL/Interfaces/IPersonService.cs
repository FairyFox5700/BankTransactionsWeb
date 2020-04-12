using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
   public interface IPersonService: IDisposable
    {
       
        Task<List<PersonDTO>> GetAllPersons(string name = null, string surname = null, string lastname = null,
            string accountNumber = null, string transactionNumber = null, string companyName = null);
        Task<PersonDTO> GetPersonById(int id);
        Task<PersonDTO> GetPersonById(ClaimsPrincipal user);
        Task AddPerson(PersonDTO person);
        Task<IdentityResult> UpdatePerson(PersonDTO person);
        Task<IdentityResult> DeletePerson(PersonDTO person);
        Task<decimal> TotalBalanceOnAccounts(int id);
    }
}

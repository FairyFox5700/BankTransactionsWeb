using BankTransaction.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
   public interface IPersonService: IDisposable
    {

        Task<PaginatedModel<PersonDTO>> GetAllPersons(int pageNumber, int pageSize, PersonFilterModel personFilter = null);
        Task<PersonDTO> GetPersonById(int id);
        Task<PersonDTO> GetPersonById(ClaimsPrincipal user);
        Task AddPerson(PersonDTO person);
        Task<IdentityResult> UpdatePerson(PersonDTO person);
        Task<IdentityResult> DeletePerson(PersonDTO person);
        Task<decimal> TotalBalanceOnAccounts(int id);
    }
}

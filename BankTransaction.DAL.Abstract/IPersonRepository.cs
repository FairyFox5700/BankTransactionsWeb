using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IPersonRepository:IRepository<Person>
    {
        Task<PaginatedPlainModel<Person>> GetAll(int startIndex, int pageSize, PersonFilter personFilter = null);
        Task<Person> GetPersonByAccount(string applicatioUserID);
        Task<IEnumerable<Person>> GetAllUsersInCurrentRole(IdentityRole identityRole);
    }
}

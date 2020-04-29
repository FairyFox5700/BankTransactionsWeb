using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BankTransaction.DAL.Implementation.Extensions;
using System.Threading.Tasks;
using BankTransaction.Entities.Filter;
using Microsoft.AspNetCore.Identity;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        private readonly BankTransactionContext context;
        public PersonRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }
        public override async Task<PaginatedPlainModel<Person>> GetAll(int startIndex, int pageSize)
        {
            var persons = await PaginatedPlainModel<Person>.Paginate(context.Persons.Include(p => p.ApplicationUser), startIndex, pageSize);
            return persons;
        }
        public async Task<PaginatedPlainModel<Person>> GetAll(int startIndex, int pageSize, PersonFilter personFilter)
        {
            var filteredPersons = SearchByFilters(personFilter, context.Persons.Include(p => p.ApplicationUser).AsQueryable());
            var persons = await PaginatedPlainModel<Person>.Paginate(filteredPersons, startIndex, pageSize);
            return persons;
        }
        private IQueryable<Person> SearchByFilters(PersonFilter personFilter, IQueryable<Person> persons)
        {
            if(personFilter!=null)
            {
                if (!String.IsNullOrEmpty(personFilter?.Name))
                {
                    persons = persons.Where(s => s.Name.Contains(personFilter.Name));
                }
                if (!String.IsNullOrEmpty(personFilter?.Surname))
                {
                    persons = persons.Where(s => s.Surname.Contains(personFilter.Surname));
                }
                if (!String.IsNullOrEmpty(personFilter?.LastName))
                {
                    persons = persons.Where(s => s.LastName.Contains((personFilter.LastName)));
                }
                if (!String.IsNullOrEmpty(personFilter?.AccountNumber))
                {
                    persons = persons.Where(s => s.Accounts.Select(e => e.Number).Contains(personFilter.AccountNumber));
                }
                if (!String.IsNullOrEmpty(personFilter?.TransactionNumber))
                {
                    persons = persons.Where(p => p.Accounts.Contains(p.Accounts.Where
                        (a => a.Transactions.Contains(a.Transactions.Where
                        (e => e.Id.ToString() == personFilter.TransactionNumber).FirstOrDefault())).FirstOrDefault()));
                }
                if (!String.IsNullOrEmpty(personFilter?.CompanyName))
                {
                    persons = context.Shareholders.Where(sh => sh.Company.Name.Contains(personFilter.CompanyName)).Select(sh => sh.Person);
                }
            }
            return persons;
        }

        public override async Task<Person> GetById(int id)
        {
            try
            {
                var entity = await context.Persons.Include(p => p.ApplicationUser).Include(p => p.Accounts).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<Person> GetPersonByAccount(string applicatioUserID)
        {
            return context.Persons.Include(p => p.ApplicationUser).Include(p => p.Accounts).FirstOrDefaultAsync(e => e.ApplicationUserFkId == applicatioUserID);
        }
        //TODO
        public Task<IEnumerable<Person>> GetAllUsersInCurrentRole(IdentityRole identityRole)
        {
            throw new NotImplementedException();
          
        }

    }

}
//foreach( var user in context.Users)
//     var userRoleViewModel = new UserRoleViewModel
//     {
//         UserId = user.Id,
//         UserName = user.UserName
//     };

// if (await userManager.IsInRoleAsync(user, role.Name))
// {
//     userRoleViewModel.IsSelected = true;
// }
// else
// {
//     userRoleViewModel.IsSelected = false;
// }

// model.Add(userRoleViewModel);
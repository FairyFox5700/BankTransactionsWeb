using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public void Add(Account entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}

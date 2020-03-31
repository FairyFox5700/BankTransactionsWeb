using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Repositories
{
    public class ShareholderRepository : IShareholderRepository
    {
        public void Add(Shareholder entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Shareholder entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Shareholder>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Shareholder> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Shareholder entity)
        {
            throw new NotImplementedException();
        }
    }
}

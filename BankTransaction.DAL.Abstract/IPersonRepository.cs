using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IPersonRepository:IRepository<Person>
    {
        Task<IEnumerable<Person>> GetAll(int startIndex, int pageSize, PersonFilter personFilter=null);

    }
}

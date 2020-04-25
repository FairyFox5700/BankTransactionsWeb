using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IShareholderRepository : IRepository<Shareholder>
    {
        Task<PaginatedPlainModel<Shareholder>> GetAll(int startIndex, int pageSize, ShareholderFilter shareholderFilter = null);
    }
}

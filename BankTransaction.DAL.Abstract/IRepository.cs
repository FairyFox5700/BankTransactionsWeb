using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IRepository<TEntity> where TEntity:BaseEntity
    {
        Task<PaginatedPlainModel<TEntity>> GetAll(int startIndex, int pageSize);
        Task<TEntity> GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}

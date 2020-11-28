using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Entities.Filter;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public abstract class BaseRepository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BankTransactionContext context;
        DbSet<TEntity> dbSet;

        private DbSet<TEntity> DBSet
        { 
            get { return dbSet ?? (dbSet = context.Set<TEntity>()); }
        }

        public  BaseRepository(BankTransactionContext context)
        {
            this.context = context;
        }
        public virtual void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            DBSet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {

            var dbEntityEntry = this.context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {

                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DBSet.Attach(entity);
                DBSet.Remove(entity);
            }
        }

        public virtual  async Task<PaginatedPlainModel<TEntity>> GetAll(int startIndex, int pageSize)
        {
            return await PaginatedPlainModel<TEntity>.Paginate(DBSet.AsTracking(), startIndex,pageSize);
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DBSet.FindAsync(id);

        }

        public virtual void Update(TEntity entity)
        {
            DBSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

        }
  

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

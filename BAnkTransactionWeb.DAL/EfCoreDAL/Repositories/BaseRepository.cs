using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
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
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (context.Entry(entity).State == EntityState.Detached)
            {
                DBSet.Attach(entity);
            }
            DBSet.Remove(entity);
        }
        //    if (context.Entry<TEntity>(entity).State == EntityState.Detached)
        //    {
        //        DBSet.Attach(entity);
        //    }
        //    DBSet.Remove(entity);
        //}

        public virtual  async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DBSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DBSet.FindAsync(id);

        }

        public virtual void Update(TEntity entity)
        {
            DBSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            //var existingEntity = DBSet.Find(entity);
            //if (existingEntity != null)
            //{
            //    //already in context
            //    var attachedEntry = context.Entry(existingEntity);
            //    attachedEntry.CurrentValues.SetValues(entity);
            //}
            //else
            //{
            //    DBSet.Update(entity);
            //}
        }
        //    if(context.Entry<TEntity>(entity).State ==EntityState.Detached)
        //    {
        //        DBSet.Attach(entity);
        //    }
        //    DBSet.Update(entity);
        //}

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

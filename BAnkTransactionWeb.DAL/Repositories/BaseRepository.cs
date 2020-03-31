using BankTransactionWeb.DAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BankTransactionContext context;
        DbSet<TEntity> dbSet;

        private DbSet<TEntity> Entities
        {
            get { return dbSet ?? (dbSet = context.Set<TEntity>()); }
        }

        public  BaseRepository(BankTransactionContext context)
        {
            this.context = context;
        }
        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
            //await context.SaveChangesAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            //var entityToDelete =  dbSet.Find(id);
            //if(entityToDelete!=null)
            //{
                dbSet.Remove(entity);
                //await context.SaveChangesAsync();
            //}
        }

        public virtual  async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Update(entity);
            //await context.SaveChangesAsync();
        }
    }
}

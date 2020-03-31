﻿using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.InMemoryDAL;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.InMemoryDAL.Repositories
{ 
    public class CompanyInMemoryRepository : ICompanyRepository
    {
        private readonly InMemoryContainer container;

        public CompanyInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Company entity)
        {
            container.Companies.Add(entity);
        }

        public void Delete(Company entity)
        {
            container.Companies.Remove(entity);
        }

        public async Task<Company> GetById(int id)
        {
            var company = container.Companies.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Company>(company)
                .ConfigureAwait(false);
        }

        public void Update(Company entity)
        {
            var entityToUpdate = container.Companies.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.DateOfCreation = entity.DateOfCreation;
                entityToUpdate.Name = entity.Name;
                entityToUpdate.Shareholders = entity.Shareholders;
            }
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            var companies = container.Companies;
            return await Task.FromResult<ICollection<Company>>(companies)
                .ConfigureAwait(false);
        }
    }
}

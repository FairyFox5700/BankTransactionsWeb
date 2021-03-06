﻿using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
    public interface ICompanyService:IDisposable
    {
        Task<PaginatedModel<CompanyDTO>> GetAllCompanies(int pageNumber, int pageSize);
        Task<CompanyDTO> GetCompanyById(int id);
        Task AddCompany(CompanyDTO company);
        Task UpdateCompany(CompanyDTO company);
        Task DeleteCompany(CompanyDTO company);
        Task<IEnumerable<CompanyDTO>> GetAllCompanies();
    }
}

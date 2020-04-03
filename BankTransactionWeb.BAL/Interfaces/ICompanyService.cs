using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface ICompanyService:IDisposable
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompanies();
        Task<CompanyDTO> GetCompanyById(int id);
        Task AddCompany(CompanyDTO company);
        Task UpdateCompany(CompanyDTO company);
        Task DeleteCompany(CompanyDTO company);
    }
}

using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IShareholderService:IDisposable
    {
        Task<IEnumerable<ShareholderDTO>> GetAllShareholders(string companyName, DateTime? dateOfCompanyCreation);
        Task<ShareholderDTO> GetShareholderById(int id);
        Task AddShareholder(ShareholderDTO shareholder);
        Task UpdateShareholder(ShareholderDTO shareholder);
        Task DeleteShareholder(ShareholderDTO shareholder);
    }
}

using BankTransaction.BAL.Implementation.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IShareholderService:IDisposable
    {
        Task<IEnumerable<ShareholderDTO>> GetAllShareholders(string companyName=null, DateTime? dateOfCompanyCreation=null);
        Task<ShareholderDTO> GetShareholderById(int id);
        Task AddShareholder(ShareholderDTO shareholder);
        Task UpdateShareholder(ShareholderDTO shareholder);
        Task DeleteShareholder(ShareholderDTO shareholder);
    }
}

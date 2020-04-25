using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IShareholderService:IDisposable
    {
        Task<PaginatedModel<ShareholderDTO>> GetAllShareholders(int pageIndex, int pageSize,  ShareholderFilterModel shareholderFilterModel = null);
        Task<ShareholderDTO> GetShareholderById(int id);
        Task AddShareholder(ShareholderDTO shareholder);
        Task UpdateShareholder(ShareholderDTO shareholder);
        Task DeleteShareholder(ShareholderDTO shareholder);
    }
}

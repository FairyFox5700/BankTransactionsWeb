﻿using BankTransaction.Models;
using BankTransaction.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
    public interface IShareholderService:IDisposable
    {
        Task<PaginatedModel<ShareholderDTO>> GetAllShareholders(int pageIndex, int pageSize,  ShareholderFilterModel shareholderFilterModel = null);
        Task<ShareholderDTO> GetShareholderById(int id);
        Task<ValidationModel> AddShareholder(ShareholderDTO shareholder);
        Task<ValidationModel> UpdateShareholder(ShareholderDTO shareholder);
        Task DeleteShareholder(ShareholderDTO shareholder);
    }
}

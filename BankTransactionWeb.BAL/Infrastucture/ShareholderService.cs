
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.BAL.Implementation.Extensions;
using BankTransaction.Models;
using BankTransaction.Entities.Filter;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Mapper.Filters;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly ILogger<ShareholderService> logger;

        public ShareholderService(IUnitOfWork unitOfWork,  ILogger<ShareholderService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task AddShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = ShareholderEntityToDtoMapper.Instance.MapBack(shareholder);
            unitOfWork.ShareholderRepository.Add(shareholderMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = ShareholderEntityToDtoMapper.Instance.MapBack(shareholder);
            unitOfWork.ShareholderRepository.Delete(shareholderMapped);
            await unitOfWork.Save();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<PaginatedModel<ShareholderDTO>> GetAllShareholders(int pageNumber, int pageSize,ShareholderFilterModel shareholderFilterModel =null )
        {
            PaginatedPlainModel<Shareholder> shareholders = null;

            if (shareholderFilterModel != null)
            {
                var filter = ShareholderFilterDtoToShareholder.Instance.Map(shareholderFilterModel);
                shareholders = await unitOfWork.ShareholderRepository.GetAll(pageNumber, pageSize, filter);
            }
            else
            {
                shareholders = await unitOfWork.ShareholderRepository.GetAll(pageNumber, pageSize);
            }
            //TODO smt better
            return new PaginatedModel<ShareholderDTO>(shareholders.Select(shareholder => ShareholderEntityToDtoMapper.Instance.Map(shareholder)), shareholders.PageNumber, shareholders.PageSize, shareholders.TotalCount, shareholders.TotalPages);
        }

        public async Task<ShareholderDTO> GetShareholderById(int id)
        {
            try
            {
                var shareholderFinded = await unitOfWork.ShareholderRepository.GetById(id);
                return ShareholderEntityToDtoMapper.Instance.Map(shareholderFinded);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetShareholderById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task UpdateShareholder(ShareholderDTO shareholder)
        {
            try
            {
                var shareholderMapped = ShareholderEntityToDtoMapper.Instance.MapBack(shareholder);
                unitOfWork.ShareholderRepository.Update(shareholderMapped);
                await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateShareholder)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
    }
}

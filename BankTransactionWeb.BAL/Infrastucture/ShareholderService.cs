using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ShareholderService> logger;

        public ShareholderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ShareholderService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddShareholder(ShareholderDTO shareholder)
        {
            try
            {
                var shareholderMapped = mapper.Map<Shareholder>(shareholder);
                unitOfWork.ShareholderRepository.Add(shareholderMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddShareholder)} instance of sahreholder successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddShareholder)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task DeleteShareholder(ShareholderDTO shareholder)
        {
            try
            {
                var shareholderMapped = mapper.Map<Shareholder>(shareholder);
                unitOfWork.ShareholderRepository.Delete(shareholderMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(DeleteShareholder)} instance of sahreholder successfully deleted");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteShareholder)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<IEnumerable<ShareholderDTO>> GetAllShareholders()
        {
            try
            {
                var shareholders = (await unitOfWork.ShareholderRepository.GetAll());
               return shareholders.Select(shareholder => mapper.Map<ShareholderDTO>(shareholder)).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllShareholders)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task<ShareholderDTO> GetShareholderById(int id)
        {
            try
            {
                var shareholderFinded = await unitOfWork.ShareholderRepository.GetById(id);
            return mapper.Map<ShareholderDTO>(shareholderFinded);
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

                var shareholderMapped = mapper.Map<Shareholder>(shareholder);
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

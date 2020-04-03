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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<CompanyService> logger;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CompanyService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddCompany(CompanyDTO company)
        {
            try
            {
                var companyMapped = mapper.Map<Company>(company);
                unitOfWork.CompanyRepository.Add(companyMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddCompany)} instance of company successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddCompany)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task DeleteCompany(CompanyDTO company)
        {
            try
            {
                var companyMapped = mapper.Map<Company>(company);
                unitOfWork.CompanyRepository.Delete(companyMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(DeleteCompany)} instance of company successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteCompany)} in class {nameof(PersonService)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies()
        {

            try
            {
                var companys = await unitOfWork.CompanyRepository.GetAll();
                return companys.Select(company => mapper.Map<CompanyDTO>(company)).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllCompanies)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task<CompanyDTO> GetCompanyById(int id)
        {
            try
            {
                var companyFinded = await unitOfWork.CompanyRepository.GetById(id);
                return mapper.Map<CompanyDTO>(companyFinded);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetCompanyById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task UpdateCompany(CompanyDTO company)
        {
            try
            {
                var companyMapped = mapper.Map<Company>(company);
                unitOfWork.CompanyRepository.Update(companyMapped);
                await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                    logger.LogError($"Catch an exception in method {nameof(UpdateCompany)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
    }
}

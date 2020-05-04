
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models;
using BankTransaction.Models.Mapper;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CompanyService> logger;

        public CompanyService(IUnitOfWork unitOfWork, ILogger<CompanyService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task AddCompany(CompanyDTO company)
        {
                var companyMapped = CompanyDtoToEntityMapper.Instance.MapBack(company);
                unitOfWork.CompanyRepository.Add(companyMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddCompany)} instance of company successfully added");
        }

        public async Task DeleteCompany(CompanyDTO company)
        {
            var companyMapped = CompanyDtoToEntityMapper.Instance.MapBack(company);
            unitOfWork.CompanyRepository.Delete(companyMapped);
            await unitOfWork.Save();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<PaginatedModel<CompanyDTO>> GetAllCompanies(int pageNumber, int pageSize)
        {

            var companies = await unitOfWork.CompanyRepository.GetAll(pageNumber,pageSize);
            //TODO smth better here
            return new PaginatedModel<CompanyDTO>(companies.Select(company => CompanyDtoToEntityMapper.Instance.Map(company)), companies.PageNumber, companies.PageSize, companies.TotalCount, companies.TotalPages);
           
        }
        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies()
        {

            var companies= await unitOfWork.CompanyRepository.GetAllCompanies();
            return companies.Select(company => CompanyDtoToEntityMapper.Instance.Map(company));

        }

        public async Task<CompanyDTO> GetCompanyById(int id)
        {
                var companyFinded = await unitOfWork.CompanyRepository.GetById(id);
                return CompanyDtoToEntityMapper.Instance.Map(companyFinded);
           
        }

        public async Task UpdateCompany(CompanyDTO company)
        {
                var companyMapped = CompanyDtoToEntityMapper.Instance.MapBack(company);
                unitOfWork.CompanyRepository.Update(companyMapped);
                await unitOfWork.Save();
         
        }
    }
}

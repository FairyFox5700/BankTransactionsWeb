using AutoMapper;
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

namespace BankTransaction.BAL.Implementation.Infrastucture
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
                var companyMapped = mapper.Map<Company>(company);
                unitOfWork.CompanyRepository.Add(companyMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddCompany)} instance of company successfully added");
        }

        public async Task DeleteCompany(CompanyDTO company)
        {
            var companyMapped = mapper.Map<Company>(company);
            unitOfWork.CompanyRepository.Delete(companyMapped);
            await unitOfWork.Save();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies()
        {

var companies = await unitOfWork.CompanyRepository.GetAll();
                return companies.Select(company => mapper.Map<CompanyDTO>(company)).ToList();
           
        }

        public async Task<CompanyDTO> GetCompanyById(int id)
        {
                var companyFinded = await unitOfWork.CompanyRepository.GetById(id);
                return mapper.Map<CompanyDTO>(companyFinded);
           
        }

        public async Task UpdateCompany(CompanyDTO company)
        {
                var companyMapped = mapper.Map<Company>(company);
                unitOfWork.CompanyRepository.Update(companyMapped);
                await unitOfWork.Save();
         
        }
    }
}

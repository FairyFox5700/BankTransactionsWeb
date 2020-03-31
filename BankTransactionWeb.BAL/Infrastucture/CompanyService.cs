using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
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

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddCompany(CompanyDTO company)
        {
            var companyMapped = mapper.Map<Company>(company);
            unitOfWork.CompanyRepository.Add(companyMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteCompany(CompanyDTO company)
        {
            var companyMapped = mapper.Map<Company>(company);
            unitOfWork.CompanyRepository.Delete(companyMapped);
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompanies()
        {
            var companys = await unitOfWork.CompanyRepository.GetAll();
            return companys.Select(product => mapper.Map<CompanyDTO>(companys));
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

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
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ShareholderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = mapper.Map<Shareholder>(shareholder);
            unitOfWork.ShareholderRepository.Add(shareholderMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = mapper.Map<Shareholder>(shareholder);
            unitOfWork.ShareholderRepository.Delete(shareholderMapped);
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<ShareholderDTO>> GetAllCompanies()
        {
            var shareholders = await unitOfWork.ShareholderRepository.GetAll();
            return shareholders.Select(product => mapper.Map<ShareholderDTO>(shareholders));
        }

        public async Task<ShareholderDTO> GetShareholderById(int id)
        {
            var shareholderFinded = await unitOfWork.ShareholderRepository.GetById(id);
            return mapper.Map<ShareholderDTO>(shareholderFinded);
        }

        public async Task UpdateShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = mapper.Map<Shareholder>(shareholder);
            unitOfWork.ShareholderRepository.Update(shareholderMapped);
            await unitOfWork.Save();
        }
    }
}

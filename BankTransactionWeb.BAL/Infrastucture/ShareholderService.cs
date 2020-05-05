
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using BankTransaction.Models;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Mapper.Filters;
using BankTransaction.Models.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class ShareholderService : IShareholderService
    {
        private readonly IUnitOfWork unitOfWork;
        public ShareholderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ValidationModel> AddShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = ShareholderEntityToDtoMapper.Instance.MapBack(shareholder);
            var result = await ValidateShareholder(shareholderMapped);
            if (result == null)
            {
                unitOfWork.ShareholderRepository.Add(shareholderMapped);
                await unitOfWork.Save();
                return new ValidationModel("Succesfully added person to shareholder", false);
            }
            else
            {
                return result;
            }

        }

        private async Task<ValidationModel> ValidateShareholder(Shareholder shareholderMapped)
        {
            var companyFinded = await unitOfWork.CompanyRepository.GetById(shareholderMapped.CompanyId);
            var shareholdersOfPerson = await unitOfWork.ShareholderRepository.GetShareholderByPersonId(shareholderMapped.PersonId);
            if (shareholdersOfPerson.Any() && shareholdersOfPerson.Where(e => e.Company == companyFinded).Any())
            {
                return new ValidationModel("Current person is already shareholder of current company", true);
            }
            return null;
        }

        public async Task<ValidationModel> UpdateShareholder(ShareholderDTO shareholder)
        {
            var shareholderMapped = ShareholderEntityToDtoMapper.Instance.MapBack(shareholder);
            var result = await ValidateShareholder(shareholderMapped);
            if (result == null)
            {
                unitOfWork.ShareholderRepository.Update(shareholderMapped);
                await unitOfWork.Save();
                return new ValidationModel("Succesfully added person to shareholder", false);
            }
            else
            {
                return result;
            }
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

        public async Task<PaginatedModel<ShareholderDTO>> GetAllShareholders(int pageNumber, int pageSize, ShareholderFilterModel shareholderFilterModel = null)
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
            var shareholderFinded = await unitOfWork.ShareholderRepository.GetById(id);
            shareholderFinded.Company = await unitOfWork.CompanyRepository.GetById(shareholderFinded.CompanyId);
            return ShareholderEntityToDtoMapper.Instance.Map(shareholderFinded);
        }


    }
}

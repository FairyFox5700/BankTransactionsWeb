using System.Linq;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper.MpaperOld
{
    public class CompanyDtoToEntityMapper : IMapper<Company, CompanyDTO>
    {
        private CompanyDtoToEntityMapper() { }
        public static readonly CompanyDtoToEntityMapper Instance = new CompanyDtoToEntityMapper();
        public CompanyDTO Map(Company source)
        {
            return new CompanyDTO()
            {
                Id = source.Id,
                DateOfCreation = source.DateOfCreation,
                //TODO smt here look better
                Shareholders = source.Shareholders?.Select(sh => ShareholderEntityToDtoMapper.Instance.Map(sh)).ToList(),
                Name = source.Name
            };
        }

        public Company MapBack(CompanyDTO destination)
        {
            return new Company()
            {
                Id = destination.Id,
                DateOfCreation = destination.DateOfCreation,
                Shareholders = destination.Shareholders?.Select(sh =>ShareholderEntityToDtoMapper.Instance.MapBack(sh)).ToList(),
                Name = destination.Name
            };
        }
    }
}

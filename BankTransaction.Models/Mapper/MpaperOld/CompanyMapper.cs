using System.Linq;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper.MpaperOld
{
    public class CompanyMapper : IMapper<Company, CompanyDTO>
    {
        public CompanyDTO Map(Company source)
        {
            return new CompanyDTO()
            {
                Id = source.Id,
                DateOfCreation = source.DateOfCreation,
                Shareholders = source.Shareholders.Select(sh => new ShareholderMapper().Map(sh)).ToList(),
                Name = source.Name
            };
        }

        public Company MapBack(CompanyDTO destination)
        {
            return new Company()
            {
                Id = destination.Id,
                DateOfCreation = destination.DateOfCreation,
                Shareholders = destination.Shareholders.Select(sh => new ShareholderMapper().MapBack(sh)).ToList(),
                Name = destination.Name
            };
        }
    }
}

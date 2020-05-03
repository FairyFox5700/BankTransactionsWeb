using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Models.Mapper.MpaperOld
{
    public class ShareholderMapper : IMapper<Shareholder, ShareholderDTO>
    {
        public ShareholderDTO Map(Shareholder source)
        {
            return new ShareholderDTO()
            {
                Id = source.Id,
                Company = new CompanyMapper().Map(source.Company),
                Person = new PersonMapper().Map(source.Person),
                PersonId = source.PersonId,
                CompanyId = source.CompanyId
            };
        }

        public Shareholder MapBack(ShareholderDTO destination)
        {
            return new Shareholder()
            {
                Id = destination.Id,
                Company = new CompanyMapper().MapBack(destination.Company),
                Person = new PersonMapper().MapBack(destination.Person),
                PersonId = destination.PersonId,
                CompanyId = destination.CompanyId
            };
        }
    }
}

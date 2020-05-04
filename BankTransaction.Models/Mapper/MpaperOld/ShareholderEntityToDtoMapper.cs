using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;

namespace BankTransaction.Models.Mapper
{
    public class ShareholderEntityToDtoMapper : IMapper<Shareholder, ShareholderDTO>
    {
        private ShareholderEntityToDtoMapper() { }
        public static readonly ShareholderEntityToDtoMapper Instance = new ShareholderEntityToDtoMapper();
        public ShareholderDTO Map(Shareholder source)
        {
            return new ShareholderDTO()
            {
                Id = source.Id,
                Company = CompanyDtoToEntityMapper.Instance.Map(source.Company),
                Person = PersonEntityToDtoMapper.Instance.Map(source.Person),
                PersonId = source.PersonId,
                CompanyId = source.CompanyId
            };
        }

        public Shareholder MapBack(ShareholderDTO destination)
        {
            return new Shareholder()
            {
                Id = destination.Id,
                Company = CompanyDtoToEntityMapper.Instance.MapBack(destination.Company),
                Person = PersonEntityToDtoMapper.Instance.MapBack(destination.Person),
                PersonId = destination.PersonId,
                CompanyId = destination.CompanyId
            };
        }
    }
}

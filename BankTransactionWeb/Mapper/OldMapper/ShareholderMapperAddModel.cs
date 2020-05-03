using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class ShareholderMapperAddModel : IMapper<AddShareholderViewModel, ShareholderDTO>
    {
        public ShareholderDTO Map(AddShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                Company = source.Company,
                CompanyId = source.CompanyId,
                Person = source.Person,
                PersonId = source.PersonId,
            };
        }

        public AddShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new AddShareholderViewModel()
            {
                Company = destination.Company,
                CompanyId = destination.CompanyId,
                Person = destination.Person,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




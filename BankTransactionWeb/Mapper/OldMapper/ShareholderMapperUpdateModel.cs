using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class ShareholderMapperUpdateModel : IMapper<UpdateShareholderViewModel, ShareholderDTO>
    {
        public ShareholderDTO Map(UpdateShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                Id = source.Id,
                Company = source.Company,
                CompanyId = source.CompanyId,
                Person = source.Person,
                PersonId = source.PersonId,
            };
        }

        public UpdateShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new UpdateShareholderViewModel()
            {
                Id = destination.Id,
                Company = destination.Company,
                CompanyId = destination.CompanyId,
                Person = destination.Person,
                PersonId = destination.PersonId,
            };
        }
    }

   
}




using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class UpdateShareholderToShareholderDTOMapper : IMapper<UpdateShareholderViewModel, ShareholderDTO>
    {
        private UpdateShareholderToShareholderDTOMapper() { }
        public static readonly UpdateShareholderToShareholderDTOMapper Instance = new UpdateShareholderToShareholderDTOMapper();
        public ShareholderDTO Map(UpdateShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                PersonName = source.PersonName,
                PersonLastName = source.PersonLastName,
                PersonSurName = source.PersonSurName,
                CompanyName = source.CompanyName,
                PersonId = source.PersonId,
                CompanyId = source.CompanyId,
                Id = source.Id
            };
        }

        public UpdateShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new UpdateShareholderViewModel()
            {
                Id = destination.Id,
                PersonId = destination.PersonId,
                CompanyId = destination.CompanyId,
                PersonName = destination.PersonName,
                PersonLastName = destination.PersonLastName,
                PersonSurName = destination.PersonSurName,
                CompanyName = destination.CompanyName
            };
        }
    }

   
}




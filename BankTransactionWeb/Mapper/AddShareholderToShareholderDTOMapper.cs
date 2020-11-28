using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class AddShareholderToShareholderDTOMapper : IMapper<AddShareholderViewModel, ShareholderDTO>
    {
        private AddShareholderToShareholderDTOMapper() { }
        public static readonly AddShareholderToShareholderDTOMapper Instance = new AddShareholderToShareholderDTOMapper();
        public ShareholderDTO Map(AddShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                PersonName = source.PersonName,
                PersonLastName = source.PersonLastName,
                PersonSurName = source.PersonSurName,
                CompanyName = source.CompanyName,
                PersonId = source.PersonId,
                CompanyId = source.CompanyId
            };
        }

        public AddShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new AddShareholderViewModel()
            {
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




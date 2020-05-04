using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

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




using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
{
    public class UpdateShareholderToShareholderDTOMapper : IMapper<UpdateShareholderViewModel, ShareholderDTO>
    {
        private UpdateShareholderToShareholderDTOMapper() { }
        public static readonly UpdateShareholderToShareholderDTOMapper Instance = new UpdateShareholderToShareholderDTOMapper();
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




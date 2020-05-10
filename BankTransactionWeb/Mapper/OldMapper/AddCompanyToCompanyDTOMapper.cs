using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class AddCompanyToCompanyDTOMapper : IMapper<AddCompanyViewModel, CompanyDTO>
    {
        private AddCompanyToCompanyDTOMapper() { }
        public static readonly AddCompanyToCompanyDTOMapper Instance = new AddCompanyToCompanyDTOMapper();
        public CompanyDTO Map(AddCompanyViewModel source)
        {
            return new CompanyDTO()
            {
                DateOfCreation = source.DateOfCreation,
                Name = source.Name
            };
        }

        public AddCompanyViewModel MapBack(CompanyDTO destination)
        {
            return new AddCompanyViewModel()
            {
                DateOfCreation = destination.DateOfCreation,
                Name = destination.Name
            };
        }
    }

   
}




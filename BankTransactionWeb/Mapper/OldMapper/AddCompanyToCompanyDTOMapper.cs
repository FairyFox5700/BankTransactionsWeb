using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
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




using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class CompanyMapperAddModel : IMapper<AddCompanyViewModel, CompanyDTO>
    {
        private CompanyMapperAddModel()
        {
        }

        public static CompanyMapperAddModel Instance { get; private set; } = new CompanyMapperAddModel();
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




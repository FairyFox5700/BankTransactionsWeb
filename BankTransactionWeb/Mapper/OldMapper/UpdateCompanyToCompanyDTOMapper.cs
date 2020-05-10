using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public class UpdateCompanyToCompanyDTOMapper : IMapper<UpdateCompanyViewModel, CompanyDTO>
    {
        private UpdateCompanyToCompanyDTOMapper() { }
        public static readonly UpdateCompanyToCompanyDTOMapper Instance = new UpdateCompanyToCompanyDTOMapper();
        public CompanyDTO Map(UpdateCompanyViewModel source)
        {
            return new CompanyDTO()
            {
                Id = source.Id,
                DateOfCreation = source.DateOfCreation,
                Name = source.Name
            };
        }

        public UpdateCompanyViewModel MapBack(CompanyDTO destination)
        {
            return new UpdateCompanyViewModel()
            {
                Id = destination.Id,
                DateOfCreation = destination.DateOfCreation,
                Name = destination.Name
            };
        }
    }

   
}




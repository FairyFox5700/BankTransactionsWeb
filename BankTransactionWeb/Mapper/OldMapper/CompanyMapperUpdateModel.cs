using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper
{
    public class CompanyMapperUpdateModel : IMapper<UpdateCompanyViewModel, CompanyDTO>
    {
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




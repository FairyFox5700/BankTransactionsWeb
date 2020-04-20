using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleMapperAddModel : IMapper<RoleDTO, AddRoleViewModel>
    {
        public AddRoleViewModel Map(RoleDTO source)
        {
            return new AddRoleViewModel()
            {
                Name = source.Name
            };
        }

        public RoleDTO MapBack(AddRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Name = destination.Name
            };
        }
    }

   
}




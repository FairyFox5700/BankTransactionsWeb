using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Admin
{
    public class UserInRoleMapperListModel : IMapper< RoleDTO, UsersInRoleViewModel>
    {
        public UsersInRoleViewModel Map(RoleDTO source)
        {
            return new UsersInRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public RoleDTO MapBack(UsersInRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name
            };
        }
    }

   
}




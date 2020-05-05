using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleDTOToAddRoleMapper: IMapper<RoleDTO, AddRoleViewModel>
    {
        private RoleDTOToAddRoleMapper() { }

        public static readonly RoleDTOToAddRoleMapper Instance = new RoleDTOToAddRoleMapper();
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




using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleDTOToListRoleMapper : IMapper<RoleDTO, ListRoleViewModel>
    {
        private RoleDTOToListRoleMapper() { }

        public static readonly RoleDTOToListRoleMapper Instance = new RoleDTOToListRoleMapper();
        public ListRoleViewModel Map(RoleDTO source)
        {
            return new ListRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public RoleDTO MapBack(ListRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name,
            };
        }
    }

   
}




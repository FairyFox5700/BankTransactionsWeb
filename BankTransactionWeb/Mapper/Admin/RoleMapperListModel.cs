using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleMapperListModel : IMapper<RoleDTO, ListRoleViewModel>
    {
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




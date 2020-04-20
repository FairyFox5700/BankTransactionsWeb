using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleMapperUpdateModel : IMapper<RoleDTO, UpdateRoleViewModel>
    {
        public UpdateRoleViewModel Map(RoleDTO source)
        {
            return new UpdateRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Users = source.Users
            };
        }

        public RoleDTO MapBack(UpdateRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name,
                Users = destination.Users
            };
        }
    }

   
}




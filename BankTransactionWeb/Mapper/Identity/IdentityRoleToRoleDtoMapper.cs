using Microsoft.AspNetCore.Identity;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Identity
{
    public class IdentityRoleToRoleDtoMapper : IMapper<IdentityRole, RoleDTO>
    {
        private IdentityRoleToRoleDtoMapper() { }

        public static readonly IdentityRoleToRoleDtoMapper Instance = new IdentityRoleToRoleDtoMapper();
        public RoleDTO Map(IdentityRole source)
        {
            return new RoleDTO()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public IdentityRole MapBack(RoleDTO destination)
        {
            return new IdentityRole()
            {
                Id = destination.Id,
                Name = destination.Name
            };
        }
    }

   
}




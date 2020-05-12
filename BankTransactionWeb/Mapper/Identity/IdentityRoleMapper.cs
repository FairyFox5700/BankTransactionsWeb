using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Identity;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Identity
{
    public class IdentityRoleMapper : IMapper<IdentityRole, RoleDTO>
    {
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




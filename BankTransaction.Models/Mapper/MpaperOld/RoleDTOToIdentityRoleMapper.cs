using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Identity;
using BankTransaction.Configuration;

namespace BankTransaction.Models.Mapper
{
    public class RoleDTOToIdentityRoleMapper : IMapper<RoleDTO, IdentityRole>
    {
        private RoleDTOToIdentityRoleMapper() { }
        public static readonly RoleDTOToIdentityRoleMapper Instance = new RoleDTOToIdentityRoleMapper();
        public IdentityRole Map(RoleDTO source)
        {
            return new IdentityRole()
            {
                Id = source.Id,
                Name = source.Name
            };
        }

        public RoleDTO MapBack(IdentityRole destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name
            };
        }
    }


}





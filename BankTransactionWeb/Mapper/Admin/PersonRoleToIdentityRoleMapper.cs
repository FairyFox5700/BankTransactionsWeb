using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Identity;
using BankTransaction.Configuration;

namespace BankTransaction.Web.Mapper.Admin
{
    public class PersonRoleToIdentityRoleMapper : IMapper<PersonInRoleDTO, IdentityRole>
    {
        public IdentityRole Map(PersonInRoleDTO source)
        {
            return new IdentityRole()
            {
                Name = source.Name,
                Id = source.Id
            };

        }

        public PersonInRoleDTO MapBack(IdentityRole destination)
        {
            return new PersonInRoleDTO()
            {
                Name = destination.Name,
                Id = destination.Id
            };
        }
    }

   
}




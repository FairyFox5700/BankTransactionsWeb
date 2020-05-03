using Microsoft.AspNetCore.Identity;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Admin
{
    public class PersonRoleMapper : IMapper<PersonInRoleDTO, IdentityRole>
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




using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Entities;

namespace BankTransaction.Models.Mapper
{
    public class PersonRoleToApplicationUser : IMapper<PersonInRoleDTO, ApplicationUser>
    {
        private PersonRoleToApplicationUser() { }
        public static readonly PersonRoleToApplicationUser Instance = new PersonRoleToApplicationUser();


        public ApplicationUser Map(PersonInRoleDTO source)
        {
            return new ApplicationUser()
            {
                Id = source.Id,
                UserName = source.UserName
            };
        }

        public PersonInRoleDTO MapBack(ApplicationUser destination)
        {
            return new PersonInRoleDTO()
            {
                Id = destination.Id,
                UserName = destination.UserName
            };
        }
    }
}








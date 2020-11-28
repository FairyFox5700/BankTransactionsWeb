using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Admin
{

    public class PersonInRoleDTOToUserRolesMapper : IMapper<PersonInRoleDTO, UsersInRoleViewModel>
    {
        private PersonInRoleDTOToUserRolesMapper() { }

        public static readonly PersonInRoleDTOToUserRolesMapper Instance = new PersonInRoleDTOToUserRolesMapper();
        public UsersInRoleViewModel Map(PersonInRoleDTO source)
        {
            return new UsersInRoleViewModel
            {
                Name = source.Name,
                Id = source.Id,
                IsSelected = source.IsSelected,
                AppUserId = source.AppUserId,
                Surname = source.Surname,
                LastName = source.LastName,
                UserName = source.UserName
            };

        }

        public PersonInRoleDTO MapBack(UsersInRoleViewModel destination)
        {
            return new PersonInRoleDTO()
            {
                Name = destination.Name,
                Id = destination.Id,
                IsSelected = destination.IsSelected,
                AppUserId = destination.AppUserId,
                Surname = destination.Surname,
                LastName = destination.LastName,
                UserName = destination.UserName
            };
        }
    }

   
}




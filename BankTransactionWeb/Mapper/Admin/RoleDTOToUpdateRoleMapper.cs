﻿using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper.Admin
{
    public class RoleDTOToUpdateRoleMapper : IMapper<RoleDTO, UpdateRoleViewModel>
    {
        private RoleDTOToUpdateRoleMapper() { }

        public static readonly RoleDTOToUpdateRoleMapper Instance = new RoleDTOToUpdateRoleMapper();
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




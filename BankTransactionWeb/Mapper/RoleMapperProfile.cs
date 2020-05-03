using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper
{
    public class RoleMapperProfile:Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<RoleDTO, AddRoleViewModel>().ReverseMap();
            CreateMap<RoleDTO, UpdateRoleViewModel>().ReverseMap();
            CreateMap<RoleDTO, ListRoleViewModel>().ReverseMap();
            CreateMap<UsersInRoleViewModel, PersonDTO>().ReverseMap();
            CreateMap<ResetPasswordViewModel, PersonDTO>().ReverseMap();
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();
            CreateMap<PersonInRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<UsersInRoleViewModel, PersonInRoleDTO>().ReverseMap();
        }
    }
}

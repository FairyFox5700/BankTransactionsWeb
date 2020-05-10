using AutoMapper;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper
{
    public class IdentityMapperProfile:Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<LoginViewModel, PersonDTO>().ReverseMap();
            CreateMap<RegisterViewModel, PersonDTO>().ReverseMap();
        }
    }
}

using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

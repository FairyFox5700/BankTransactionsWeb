using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper
{
    public class CompanyMapperProfile:Profile
    {
        public CompanyMapperProfile()
        {
            CreateMap<UpdateCompanyViewModel, CompanyDTO>().ReverseMap();
            CreateMap<AddCompanyViewModel, CompanyDTO>().ReverseMap();
        }
    }
}

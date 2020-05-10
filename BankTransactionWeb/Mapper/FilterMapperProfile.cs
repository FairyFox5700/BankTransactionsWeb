using AutoMapper;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class FilterMapperProfile:Profile
    {
        public FilterMapperProfile()
        {
            CreateMap<PersonSearchModel, PersonFilterModel>().ReverseMap();
           // CreateMap<PageQueryParameters, PersonFilterModel>().ReverseMap();
            CreateMap<PaginatedModel<PersonDTO>, PageQueryParameters>().ReverseMap();
            CreateMap<ShareholderFilterModel, ShareholderSearchModel>().ReverseMap();
        }
    }
}

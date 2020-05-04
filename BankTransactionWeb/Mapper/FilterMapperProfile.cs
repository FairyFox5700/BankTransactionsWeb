using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Models;
using BankTransaction.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

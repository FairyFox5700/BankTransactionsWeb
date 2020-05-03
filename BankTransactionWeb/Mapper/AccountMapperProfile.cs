using AutoMapper;
using BankTransaction.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper
{
    public class AccountMapperProfile:Profile
    {
        public AccountMapperProfile()
        {
            CreateMap<AccountDTO, AddAccountViewModel>().ReverseMap();
            CreateMap<UpdateAccountViewModel, AccountDTO>().ReverseMap();
        }
    }
}

using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

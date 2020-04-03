using AutoMapper;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Configuration
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration ConfigureAutoMapper()
        {

            MapperConfiguration configuration = new MapperConfiguration(confg =>
            {
                confg.CreateMap<AddPersonViewModel, PersonDTO>();
                confg.CreateMap< PersonDTO, AddPersonViewModel>();

            });
            return configuration;
        }
    }
}

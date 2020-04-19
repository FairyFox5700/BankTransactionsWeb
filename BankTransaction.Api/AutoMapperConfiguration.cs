using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using BankTransaction.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration ConfigureAutoMapper()
        {

            MapperConfiguration configuration = new MapperConfiguration(confg =>
            {
                confg.CreateMap<Person, PersonDTO>();
                confg.CreateMap<PersonDTO, Person>();
                confg.CreateMap<Company, CompanyDTO>();
                confg.CreateMap<CompanyDTO, Company>();
                confg.CreateMap<Account, AccountDTO>();
                confg.CreateMap<AccountDTO, Account>();
                confg.CreateMap<Shareholder, ShareholderDTO>();
                confg.CreateMap<ShareholderDTO, Shareholder>();
                confg.CreateMap<Transaction, TransactionDTO>();
                confg.CreateMap<TransactionDTO, Transaction>();
                confg.CreateMap<LoginModel, PersonDTO>();
                confg.CreateMap<PersonDTO, LoginModel>();
                confg.CreateMap<RegisterModel, PersonDTO>();
                confg.CreateMap<PersonDTO, RegisterModel>();

            });
            return configuration;
        }
    }
}

using AutoMapper;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Cofiguration
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
            });
            return configuration;
        }
    }
}

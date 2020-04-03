using AutoMapper;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Configuration
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
                //for person
                confg.CreateMap<PersonDTO, AddPersonViewModel>();
                confg.CreateMap<AddPersonViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, UpdatePersonViewModel>();
                confg.CreateMap<UpdatePersonViewModel, Person>();
                //for acccount
                confg.CreateMap<AccountDTO, AddAccountViewModel>();
                confg.CreateMap<AddAccountViewModel, AccountDTO>();
                confg.CreateMap<AccountDTO, UpdateAccountViewModel>();
                confg.CreateMap<UpdateAccountViewModel, AccountDTO>();
            });
            return configuration;
        }
    }
}

using AutoMapper;
using BankTransactionWeb.Areas.Identity.Models.ViewModels;
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
                //for company
                confg.CreateMap<UpdateCompanyViewModel, CompanyDTO>();
                confg.CreateMap<CompanyDTO, UpdateCompanyViewModel>();
                confg.CreateMap<AddCompanyViewModel, CompanyDTO>();
                confg.CreateMap<CompanyDTO, AddCompanyViewModel>();
                //for shareholder
                confg.CreateMap<UpdateShareholderViewModel, ShareholderDTO>();
                confg.CreateMap<ShareholderDTO, UpdateShareholderViewModel>();
                confg.CreateMap<AddShareholderViewModel, ShareholderDTO>();
                confg.CreateMap<ShareholderDTO, AddShareholderViewModel>();
                //for transaction
                confg.CreateMap<TransactionDTO, TransactionListViewModel>()
                .ForMember(vm => vm.AccountDestinationeNumber, dto => dto.MapFrom(s => s.DestinationAccount.Number))
                .ForMember(vm => vm.AccountSourceNumber, dto => dto.MapFrom(s => s.SourceAccount.Number));
                //confg.CreateMap<TransactionListViewModel,TransactionDTO>()
                //.ForPath(dto=>dto.)
                confg.CreateMap<TransactionDTO, UpdateTransactionViewModel>();
                confg.CreateMap<AddTransactionViewModel, TransactionDTO>();
                confg.CreateMap<UpdateTransactionViewModel, TransactionDTO>();
                confg.CreateMap<TransactionDTO, UpdateTransactionViewModel>();
                //Identity
                confg.CreateMap<RegisterViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, RegisterViewModel>();
            });
            return configuration;
        }
    }
}

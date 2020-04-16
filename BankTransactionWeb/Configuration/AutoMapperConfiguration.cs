using AutoMapper;
using BankTransactionWeb.Areas.Admin.Models.ViewModels;
using BankTransactionWeb.Areas.Identity.Models.ViewModels;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Identity;

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
                confg.CreateMap<UpdatePersonViewModel, PersonDTO>();

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

                confg.CreateMap<TransactionDTO, UpdateTransactionViewModel>();
                confg.CreateMap<AddTransactionViewModel, TransactionDTO>();
                confg.CreateMap<UpdateTransactionViewModel, TransactionDTO>();
                confg.CreateMap<TransactionDTO, UpdateTransactionViewModel>();
                //Identity
                confg.CreateMap<LoginViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, LoginViewModel>();
                confg.CreateMap<RegisterViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, RegisterViewModel>();
                confg.CreateMap<ApplicationUser, PersonDTO>()
                .ForMember(e => e.ApplicationUserFkId, s => s.MapFrom(a => a.Id));
                confg.CreateMap<PersonDTO, ApplicationUser>()
                .ForMember(a => a.Id, s => s.MapFrom(e => e.ApplicationUserFkId));
                //admin
                confg.CreateMap<RoleDTO, AddRoleViewModel>();
                confg.CreateMap<AddRoleViewModel, RoleDTO>();
                confg.CreateMap<RoleDTO, UpdateRoleViewModel>();
                confg.CreateMap<UpdateRoleViewModel, RoleDTO>();
                confg.CreateMap<RoleDTO, ListRoleViewModel>();
                confg.CreateMap<ListRoleViewModel, RoleDTO>();
                confg.CreateMap<RoleDTO, ApplicationUser>();
                confg.CreateMap<ApplicationUser, RoleDTO>();
                confg.CreateMap<UsersInRoleViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, UsersInRoleViewModel>();
                confg.CreateMap<ResetPasswordViewModel, PersonDTO>();
                confg.CreateMap<PersonDTO, ResetPasswordViewModel>();
                confg.CreateMap<IdentityRole, RoleDTO>();
                confg.CreateMap<RoleDTO, IdentityRole>();
                confg.CreateMap<PersonInRoleDTO, IdentityRole>();
                confg.CreateMap<IdentityRole, PersonInRoleDTO>();
                confg.CreateMap<UsersInRoleViewModel, PersonInRoleDTO>();
                confg.CreateMap<PersonInRoleDTO, UsersInRoleViewModel>();
                confg.CreateMap<ApplicationUser, PersonDTO>()
                 .ForMember(x => x.Id, opt => opt.Ignore()); ;
                confg.CreateMap<PersonDTO, ApplicationUser>()
                 .ForMember(x => x.Id, opt => opt.Ignore());
                confg.CreateMap<PersonInRoleDTO, Person>()
                 .ForMember(x => x.ApplicationUserFkId , s => s.MapFrom(e => e.AppUserId)); ;
                confg.CreateMap<Person, PersonInRoleDTO>()
                .ForMember(x=>x.AppUserId, s=>s.MapFrom(e=>e.ApplicationUserFkId));
            });
            return configuration;
        }
    }
}

using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            //for person
            CreateMap<PersonDTO, AddPersonViewModel>();
            CreateMap<AddPersonViewModel, PersonDTO>();
            CreateMap<PersonDTO, UpdatePersonViewModel>();
            CreateMap<UpdatePersonViewModel, PersonDTO>();

            //for acccount
            CreateMap<AccountDTO, AddAccountViewModel>();
            CreateMap<AddAccountViewModel, AccountDTO>();
            CreateMap<AccountDTO, UpdateAccountViewModel>();
            CreateMap<UpdateAccountViewModel, AccountDTO>();
            //for company
            CreateMap<UpdateCompanyViewModel, CompanyDTO>();
            CreateMap<CompanyDTO, UpdateCompanyViewModel>();
            CreateMap<AddCompanyViewModel, CompanyDTO>();
            CreateMap<CompanyDTO, AddCompanyViewModel>();
            //for shareholder
            CreateMap<UpdateShareholderViewModel, ShareholderDTO>();
            CreateMap<ShareholderDTO, UpdateShareholderViewModel>();
            CreateMap<AddShareholderViewModel, ShareholderDTO>();
            CreateMap<ShareholderDTO, AddShareholderViewModel>();
            //for transaction
            CreateMap<TransactionDTO, TransactionListViewModel>()
            .ForMember(vm => vm.AccountDestinationeNumber, dto => dto.MapFrom(s => s.DestinationAccount.Number))
            .ForMember(vm => vm.AccountSourceNumber, dto => dto.MapFrom(s => s.SourceAccount.Number));

            CreateMap<TransactionDTO, UpdateTransactionViewModel>();
            CreateMap<AddTransactionViewModel, TransactionDTO>();
            CreateMap<UpdateTransactionViewModel, TransactionDTO>();
            CreateMap<TransactionDTO, UpdateTransactionViewModel>();
            //Identity
            CreateMap<LoginViewModel, PersonDTO>();
            CreateMap<PersonDTO, LoginViewModel>();
            CreateMap<RegisterViewModel, PersonDTO>();
            CreateMap<PersonDTO, RegisterViewModel>();
            //role
            CreateMap<RoleDTO, AddRoleViewModel>();
            CreateMap<AddRoleViewModel, RoleDTO>();
            CreateMap<RoleDTO, UpdateRoleViewModel>();
            CreateMap<UpdateRoleViewModel, RoleDTO>();
            CreateMap<RoleDTO, ListRoleViewModel>();
            CreateMap<ListRoleViewModel, RoleDTO>();
            CreateMap<UsersInRoleViewModel, PersonDTO>();
            CreateMap<PersonDTO, UsersInRoleViewModel>();
            CreateMap<ResetPasswordViewModel, PersonDTO>();
            CreateMap<PersonDTO, ResetPasswordViewModel>();
            CreateMap<IdentityRole, RoleDTO>();
            CreateMap<RoleDTO, IdentityRole>();
            CreateMap<PersonInRoleDTO, IdentityRole>();
            CreateMap<IdentityRole, PersonInRoleDTO>();
            CreateMap<UsersInRoleViewModel, PersonInRoleDTO>();
            CreateMap<PersonInRoleDTO, UsersInRoleViewModel>();
        }
       
       
    }
}

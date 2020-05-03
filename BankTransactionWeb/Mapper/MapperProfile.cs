using AutoMapper;
using BankTransaction.Models;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Web.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            //filter
            CreateMap<PersonSearchModel, PersonFilterModel> ().ReverseMap();
            CreateMap<PageQueryParameters, PersonFilterModel>().ReverseMap(); 
            CreateMap<PaginatedModel<PersonDTO>, PageQueryParameters>().ReverseMap();
            CreateMap<ShareholderFilterModel, ShareholderSearchModel>().ReverseMap();
            //for person
            CreateMap<PersonDTO, AddPersonViewModel>().ReverseMap(); 
            CreateMap<PersonDTO, UpdatePersonViewModel>().ReverseMap(); 
            //for acccount
            CreateMap<AccountDTO, AddAccountViewModel>().ReverseMap(); 
            CreateMap<UpdateAccountViewModel, AccountDTO>().ReverseMap();
            //for company
            CreateMap<UpdateCompanyViewModel, CompanyDTO>().ReverseMap();
            CreateMap<AddCompanyViewModel, CompanyDTO>().ReverseMap();
            //for shareholder
            CreateMap<UpdateShareholderViewModel, ShareholderDTO>().ReverseMap();
            CreateMap<AddShareholderViewModel, ShareholderDTO>().ReverseMap();
            //for transaction
            CreateMap<TransactionDTO, UpdateTransactionViewModel>()
             .ForMember(vm => vm.AccountDestinationNumber, dto => dto.MapFrom(s => s.DestinationAccount.Number))
            .ForMember(vm => vm.AccountSourceNumber, dto => dto.MapFrom(s => s.SourceAccount.Number)).ReverseMap();
            CreateMap<AddTransactionViewModel, TransactionDTO>();
            //Identity
            CreateMap<LoginViewModel, PersonDTO>().ReverseMap();
            CreateMap<RegisterViewModel, PersonDTO>().ReverseMap();
            //role
            CreateMap<RoleDTO, AddRoleViewModel>().ReverseMap();
            CreateMap<RoleDTO, UpdateRoleViewModel>().ReverseMap();
            CreateMap<RoleDTO, ListRoleViewModel>().ReverseMap();
            CreateMap<UsersInRoleViewModel, PersonDTO>().ReverseMap();
            CreateMap<ResetPasswordViewModel, PersonDTO>().ReverseMap();
            CreateMap<IdentityRole, RoleDTO>().ReverseMap();
            CreateMap<PersonInRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<UsersInRoleViewModel, PersonInRoleDTO>().ReverseMap();

        }
       
       
    }
}

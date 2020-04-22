using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Mapper
{
    public class BisnessToDomainProfile:Profile
    {
        public BisnessToDomainProfile()
        {

            CreateMap<Person, PersonDTO>();
            CreateMap<PersonDTO, Person>();
            CreateMap<Company, CompanyDTO>();
            CreateMap<CompanyDTO, Company>();
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountDTO, Account>();
            CreateMap<Shareholder, ShareholderDTO>();
            CreateMap<ShareholderDTO, Shareholder>();
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<ApplicationUser, PersonDTO>()
            .ForMember(e => e.ApplicationUserFkId, s => s.MapFrom(a => a.Id));
            CreateMap<PersonDTO, ApplicationUser>()
            .ForMember(a => a.Id, s => s.MapFrom(e => e.ApplicationUserFkId));
            //admin

            CreateMap<RoleDTO, ApplicationUser>();
            CreateMap<ApplicationUser, RoleDTO>();

            CreateMap<ApplicationUser, PersonDTO>()
             .ForMember(x => x.Id, opt => opt.Ignore()); ;
            CreateMap<PersonDTO, ApplicationUser>()
             .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<PersonInRoleDTO, Person>()
             .ForMember(x => x.ApplicationUserFkId, s => s.MapFrom(e => e.AppUserId)); ;
            CreateMap<Person, PersonInRoleDTO>()
            .ForMember(x => x.AppUserId, s => s.MapFrom(e => e.ApplicationUserFkId));
        }
    }
}

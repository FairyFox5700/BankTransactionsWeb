using AutoMapper;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BankTransaction.Entities.Filter;

namespace BankTransaction.Models.Mapper
{
    public class BisnessToDomainProfile:Profile
    {
        public BisnessToDomainProfile()
        {

            CreateMap<PersonDTO, Person>().ReverseMap();
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<AccountDTO, Account>().ReverseMap();
            CreateMap<Shareholder, ShareholderDTO>();
            CreateMap<ShareholderDTO, Shareholder>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();;
            //fitering
            CreateMap<PersonFilterModel, PersonFilter>().ReverseMap();
            CreateMap<ShareholderFilterModel, ShareholderFilter>().ReverseMap();
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

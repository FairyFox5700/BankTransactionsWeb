﻿using AutoMapper;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper
{
    public class TransactioMapperProfile : Profile
    {
        public TransactioMapperProfile()
        {
            CreateMap<TransactionDTO, UpdateTransactionViewModel>()
      .ForMember(vm => vm.AccountDestinationNumber, dto => dto.MapFrom(s => s.DestinationAccount.Number))
     .ForMember(vm => vm.AccountSourceNumber, dto => dto.MapFrom(s => s.SourceAccount.Number)).ReverseMap();
            CreateMap<AddTransactionViewModel, TransactionDTO>();
        }
    }
}
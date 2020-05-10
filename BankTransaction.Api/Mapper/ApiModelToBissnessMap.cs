using AutoMapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.Models;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Mapper
{
    public class ApiModelToBissnessMap : Profile
    {
        public ApiModelToBissnessMap()
        {
            CreateMap<RequestLoginModel, PersonDTO>();
            CreateMap<PersonDTO, RequestLoginModel>();
            CreateMap<RequestRegisterModel, PersonDTO>();
            CreateMap<PersonDTO, RequestRegisterModel>();
            CreateMap<RefreshTokenDTO,AuthSuccesfullModel>();
            CreateMap<AuthSuccesfullModel,RefreshTokenDTO>();
            CreateMap<PaginatedModel<PersonDTO>, PageQueryParameters>().ReverseMap();
            CreateMap<PaginatedModel<ShareholderDTO>, PageQueryParameters>().ReverseMap();
            CreateMap<SearchPersonQuery,PersonFilterModel>();
            CreateMap<ShareholderFilterModel, SearchShareholderQuery>().ReverseMap();
            CreateMap<PersonFilterModel, SearchPersonQuery>();
             
        }
    }
}

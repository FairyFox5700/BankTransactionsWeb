using AutoMapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Models.Mapper
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
        }
    }
}

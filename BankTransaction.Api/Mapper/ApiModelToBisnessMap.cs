using AutoMapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Implementation.DTOModels;

namespace BankTransaction.Api.Models.Mapper
{
    public class ApiModelToBisnessMap : Profile
    {
        public ApiModelToBisnessMap()
        {
            CreateMap<LoginModel, PersonDTO>();
            CreateMap<PersonDTO, LoginModel>();
            CreateMap<RegisterModel, PersonDTO>();
            CreateMap<PersonDTO, RegisterModel>();
        }
    }
}

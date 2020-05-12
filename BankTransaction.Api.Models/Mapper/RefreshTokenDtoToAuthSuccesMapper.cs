using BankTransaction.Api.Models.Responces;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class RefreshTokenDtoToAuthSuccesMapper : IMapper<RefreshTokenDTO, AuthSuccesfullModel>
    {
        private RefreshTokenDtoToAuthSuccesMapper() { }

        public static readonly RefreshTokenDtoToAuthSuccesMapper Instance = new RefreshTokenDtoToAuthSuccesMapper();
        public AuthSuccesfullModel Map(RefreshTokenDTO source)
        {
            return new AuthSuccesfullModel()
            {
                Token = source.Token,
                RefreshToken = source.RefreshToken
            };
        }

        public RefreshTokenDTO MapBack(AuthSuccesfullModel destination)
        {
            return new RefreshTokenDTO()
            {
                Token = destination.Token,
                RefreshToken = destination.RefreshToken
            };
        }
    }
}

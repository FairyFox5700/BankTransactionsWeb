using BankTransaction.Api.Models;
using BankTransaction.Configuration;
using BankTransaction.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Mapper
{
    public class AuthResultToApiErrorResponceMapper : IMapper<AuthResult, ApiErrorResponse>
    {
        private AuthResultToApiErrorResponceMapper() { }

        public static readonly AuthResultToApiErrorResponceMapper Instance = new AuthResultToApiErrorResponceMapper();
        public ApiErrorResponse Map(AuthResult source)
        {
            return new ApiErrorResponse(message: source.Message, messageType: source.MessageType);
        }

        public AuthResult MapBack(ApiErrorResponse destination)
        {
            return new AuthResult()
            {
                MessageType = destination.MessageType,
                Message = destination.Message
            };
        }
    }
}

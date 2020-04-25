using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;

namespace BankTransaction.Api.Helpers
{

    [DataContract]
    public class ApiResponse
    {

        public static readonly ApiResponse Forbidden = new ApiResponse ( 403, new ApiErrorResponse { Message = "Forbidden", ValidationErrors = null }) ;
        public static readonly ApiResponse Unauthorized = new ApiResponse(401, new ApiErrorResponse { Message = "Unauthorized", ValidationErrors = null });
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public bool IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiErrorResponse ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }
        public ApiResponse( object result = null, int statusCode = 200)
        {
            StatusCode = statusCode;
            Result = result;
            this.IsError = false;
        }
        public ApiResponse(int statusCode, ApiErrorResponse apiError)
        {
            this.StatusCode = statusCode;
            this.ResponseException = apiError;
            this.IsError = true;
        }

    }
}


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
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public bool IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiErrorResonse ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Result { get; set; }
        public ApiResponse(int statusCode = 200, object result = null)
        {
            StatusCode = statusCode;
            Result = result;
            this.IsError = false;
        }
        public ApiResponse(int statusCode, ApiErrorResonse apiError)
        {
            this.StatusCode = statusCode;
            this.ResponseException = apiError;
            this.IsError = true;
        }

    }
}


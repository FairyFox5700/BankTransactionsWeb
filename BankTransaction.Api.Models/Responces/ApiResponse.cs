using System.Runtime.Serialization;

namespace BankTransaction.Api.Models.Responces
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public bool IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiErrorResponse ResponseException { get; set; }
   
        public ApiResponse()
        {
        }
        public ApiResponse(int statusCode)
        {
            this.StatusCode = statusCode;
        }
        public ApiResponse(int statusCode, ApiErrorResponse apiError)
        {
            this.StatusCode = statusCode;
            this.ResponseException = apiError;
            this.IsError = true;
        }
    }
}


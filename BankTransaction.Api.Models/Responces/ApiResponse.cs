using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BankTransaction.Api.Models
{

    [DataContract]
    public class ApiResponse<T>
    {
        public static readonly ApiResponse<T> Forbidden = new ApiResponse<T>(403, new ApiErrorResponse ( "Forbidden"));
        public static readonly ApiResponse<T> BadRequest = new ApiResponse<T>(400, new ApiErrorResponse ( "Bad request") );
        public static readonly ApiResponse<T> NotFound = new ApiResponse<T>(404, new ApiErrorResponse ("Object not found"));
        public static readonly ApiResponse<T> Unauthorized = new ApiResponse<T>(401, new ApiErrorResponse ("Unauthorized"));
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public bool IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiErrorResponse ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Data { get; set; }

        public ApiResponse()
        {

        }
        public ApiResponse(T result, int statusCode = 200)
        {
            StatusCode = statusCode;
            Data = result;
            this.IsError = false;
        }
        public ApiResponse(int statusCode, ApiErrorResponse apiError)
        {
            this.StatusCode = statusCode;
            this.ResponseException = apiError;
            this.IsError = true;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }
       

    }
}


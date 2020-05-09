using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BankTransaction.Api.Models
{


    [DataContract]
    public class ApiDataResponse<T>: ApiResponse
    {
        public static readonly ApiDataResponse<T> Forbidden = new ApiDataResponse<T>(ErrorMessage.Forbidden.GetHttpStatusCode(),
      new ApiErrorResponse(message: ErrorMessage.Forbidden.GetDescription(),
          messageType: nameof(ErrorMessage.Forbidden)));
        public static readonly ApiDataResponse<T> BadRequest = new ApiDataResponse<T>(ErrorMessage.BadRequest.GetHttpStatusCode(),
            new ApiErrorResponse(message: ErrorMessage.BadRequest.GetDescription(),
                messageType: nameof(ErrorMessage.BadRequest)));
        public static readonly ApiDataResponse<T> NotFound = new ApiDataResponse<T>(ErrorMessage.NotFound.GetHttpStatusCode(),
            new ApiErrorResponse(message: ErrorMessage.NotFound.GetDescription(),
                messageType: nameof(ErrorMessage.NotFound)));
        public static readonly ApiDataResponse<T> Unauthorized = new ApiDataResponse<T>(ErrorMessage.Unauthorized.GetHttpStatusCode(),
            new ApiErrorResponse(message: ErrorMessage.Unauthorized.GetDescription(),
                messageType: nameof(ErrorMessage.Unauthorized)));
        public static readonly ApiDataResponse<T> ServerError = new ApiDataResponse<T>(ErrorMessage.ServerError.GetHttpStatusCode(),
            new ApiErrorResponse(message: ErrorMessage.ServerError.GetDescription(),
                messageType: nameof(ErrorMessage.ServerError)));
        [DataMember(EmitDefaultValue = false)]
        public T Data { get; set; }

        public ApiDataResponse()
        {
        }
        public ApiDataResponse(T result, int statusCode = 200):base(statusCode)
        {
            StatusCode = statusCode;
            Data = result;
            this.IsError = false;
        }
        public ApiDataResponse(int statusCode, ApiErrorResponse apiError) : base(statusCode, apiError) { }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this).ToString();
        }
       
    }
}


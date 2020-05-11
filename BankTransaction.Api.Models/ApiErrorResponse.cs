using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    public class ApiErrorResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MessageType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidationError> ValidationErrors { get; set; }
        public ApiErrorResponse()
        {
                
        }
        public ApiErrorResponse(string message, string messageType)
        {
            Message = message;
            MessageType = messageType;
        }
        public ApiErrorResponse(string message, string messageType, List<ValidationError> validationErrors):this(message,messageType)
        {
            ValidationErrors = new List<ValidationError>();
            ValidationErrors = validationErrors;
            this.MessageType = messageType;
        }
    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{

    //public class ErrorDetails
    //{
    //    public int StatusCode { get; set; }
    //    public string Message { get; set; }
    //    public string Name { get; set; }
    //   // public IEnumerable<ValidationError> ValidationErrors { get; set; }

    //    //public override string ToString()
    //    //{
    //    //    return JsonConvert.SerializeObject(this);
    //    //}
    //}

    public class ApiErrorResonse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }

}

using System.Collections.Generic;

namespace BankTransactionWeb.BAL.Models
{
    public abstract class BaseGatewayResponse
    {
        public bool Success { get; }
        //public bool Failed { get; }
        //public bool LockedOut { get; }
        public List<Error> Errors { get; }

        protected BaseGatewayResponse(bool success =false ,List<Error> errors = null)
        {
            Success = success;
            Errors = errors;
        }

        
    }
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace BankTransaction.Api.Models
{
    public static class ModelStateExtension
    {
        public static IEnumerable<ValidationError> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Keys
                .SelectMany(name => modelState[name].Errors
                .Select(x => new ValidationError { Name = name, Message = x.ErrorMessage }))
                .ToList();
        }
    }


}

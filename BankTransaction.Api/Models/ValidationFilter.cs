using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    //public class ValidationFilter
    //{
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        if (!context.ModelState.IsValid)
    //        {
    //            var errorListModel = context.ModelState
    //                .Where(x => x.Value.Errors.Count > 0)
    //                .ToDictionary(er => er.Key, er => er.Value.Errors.Select(x => x.ErrorMessage))
    //                .ToList();
    //            var errorResponce = new ApiErrorResonse();
    //            foreach (var error in errorListModel)
    //            {
    //                foreach (var erValue in error.Value)
    //                {
    //                    var errorModel = new ValidationError()
    //                    {
    //                        Name = error.Key,
    //                        Message = erValue
    //                    };
    //                    errorResponce.ValidationErrors.Add(errorModel);
    //                }
    //            }

    //            context.Result = new BadRequestObjectResult(errorResponce);
    //            return;
    //        }

    //        await next();
    //    }
    //}


}

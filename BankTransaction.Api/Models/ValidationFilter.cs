using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    public class ModelStateFeature
    {
        public ModelStateDictionary ModelState { get; set; }

        public ModelStateFeature(ModelStateDictionary state)
        {
            this.ModelState = state;
        }
    }
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var state = context.ModelState;
            context.HttpContext.Features.Set<ModelStateFeature>(new ModelStateFeature(state));
            await next();
            //if (!context.ModelState.IsValid)
            //{
            //    var errorListModel = context.ModelState
            //        .Where(x => x.Value.Errors.Count > 0)
            //        .ToDictionary(er => er.Key, er => er.Value.Errors.Select(x => x.ErrorMessage))
            //        .ToList();
            //    var errorResponce = new ApiErrorResonse();
            //    foreach (var error in errorListModel)
            //    {
            //        foreach (var erValue in error.Value)
            //        {
            //            var errorModel = new ValidationError()
            //            {
            //                Name = error.Key,
            //                Message = erValue
            //            };
            //            errorResponce.ValidationErrors.Add(errorModel);
            //        }
            //    }

            //    context.Result = new BadRequestObjectResult(errorResponce);
            //    return;
            //}

            //await next();
        }
    }


}

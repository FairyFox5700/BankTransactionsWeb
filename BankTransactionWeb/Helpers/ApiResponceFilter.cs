using BankTransaction.Api.Models.Responces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankTransaction.Web.Helpers
{
    public class ApiResponceFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
          

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var objectResult = context.Result as ObjectResult;
            var apiResult = objectResult?.Value as ApiResponse;
            if (apiResult == null)
                return;
            context.HttpContext.Response.StatusCode = apiResult.StatusCode;
           //if(context.HttpContext.Response.StatusCode==(int)HttpStatusCode.Unauthorized)
                
        }

        
    }
}

using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Api.Models.Responces;

namespace BankTransaction.Api.Helpers
{
    public class StatusCodeFilter : Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
      
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var objectResult = context.Result as ObjectResult;
            var apiResult = objectResult?.Value as ApiResponse;
            if (apiResult == null)
                return;
            context.HttpContext.Response.StatusCode = apiResult.StatusCode;
        }
    }
}

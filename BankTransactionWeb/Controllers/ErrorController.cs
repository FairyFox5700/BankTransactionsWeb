using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    public class ErrorController:Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    ViewBag.QueryString = statusCodeData.OriginalQueryString;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    ViewBag.QueryString = statusCodeData.OriginalQueryString;
                    break;
            }

            return View();
        }

        [Route("Error/{message}")]
        public IActionResult HandleErrorCode(string message)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewBag.ErrorMessage = message;
            ViewBag.RouteOfException = statusCodeData.OriginalPath;
            ViewBag.QueryString = statusCodeData.OriginalQueryString;

            return View();
        }
    }
}

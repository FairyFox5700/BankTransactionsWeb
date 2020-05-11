using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Text;

namespace BankTransaction.Api.Models
{
    public enum ErrorMessage
    {
        [Description("Non authorized attempt to get resource")]
        [HttpStatusCode(401)]
        Unauthorized,
        [Description("Forbidden")]
        [HttpStatusCode(403)]
        Forbidden,
        [Description("Your request can not be proceeded")]
        [HttpStatusCode(400)]
        BadRequest,
        [Description("The object you are trying to find was not found")]
        [HttpStatusCode(404)]
        NotFound,
        [Description("Your request cannot be processed. Please contact a support.")]
        [HttpStatusCode(500)]
        ServerError,
        [Description("Login attempt is not successful")]
        LoginAttemptNotSuccesfull,
        [Description("User with this email does not exists.")]
        EmailNotValid,
        [Description("User with this email is already registered")]
        EmailIsAlreadyInUse,
        [Description("Token validation failed")]
        TokenValidationFailed,
        [Description("Token has not expired yet")]
        TokenNotExpired,
        [Description("This refresh token does not exists")]
        RefreshTokenNotExists,
        [Description("Current token is invalidated")]
        TokenIsInvalidated,
        [Description("Current token already used")]
        TokenIsUsed,
        [Description("Refresh token jwt identifier is not match with this token id")]
        TokenIdMismatch

    }
    public class HttpStatusCodeAttribute : Attribute
    {
        public int StatusCode { get; private set; }

        public HttpStatusCodeAttribute(int statusCode)
        {
            StatusCode = statusCode;
        }
    }

}

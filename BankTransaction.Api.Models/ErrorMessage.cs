using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BankTransaction.Api.Models
{
    public enum ErrorMessage
    {
        [Description("Login attempt is not successful")]
        LoginAttemptNotSuccesfull ,
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

  
}

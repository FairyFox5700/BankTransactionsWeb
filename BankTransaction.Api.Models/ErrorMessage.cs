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
        [Description("Current tokenhas already used"]
        TokenIsUsed,
        [Description("Refresh token jwt identifier is not match with this token id"]
        TokenIdMismatch

    }

    public static class EnumExtension
    {
        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            var type = enumValue.GetType();
            if( type.IsEnum)
            {
                throw new ArgumentException("Passed parameter must be of enum type");
            }
            var enumInfo = type.GetMember(enumValue.ToString());
            if(enumInfo!=null && enumInfo.Length>0)
            {
                var atrrbts = enumInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if(atrrbts!=null && atrrbts.Length>0)
                {
                    return ((DescriptionAttribute)atrrbts[0]).Description;
                }
            }
            return enumValue.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("Passed parameter must be of enum type");
            }
            var enumInfo = type.GetMember(enumValue.ToString());
            if (enumInfo != null && enumInfo.Length > 0)
            {
                var atrrbts = enumInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atrrbts != null && atrrbts.Length > 0)
                {
                    var description = ((DescriptionAttribute)atrrbts[0]).Description.ToString();
                    return description;
                }
            }
            return enumValue.ToString();
        }
        public static int GetHttpStatusCode(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            HttpStatusCodeAttribute[] attributes = (HttpStatusCodeAttribute[])fieldInfo.GetCustomAttributes(typeof(HttpStatusCodeAttribute), false);
            return (attributes.Length > 0) ? attributes[0].StatusCode : (int)HttpStatusCode.OK;
        }
    }
}

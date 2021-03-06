﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.DAL.Implementation.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool EqualsUpToSeconds(this DateTime dt1, DateTime dt2)
        {
            return dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day &&
                   dt1.Hour == dt2.Hour && dt1.Minute == dt2.Minute && dt1.Second == dt2.Second;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTransaction.DAL.Implementation.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            return source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}

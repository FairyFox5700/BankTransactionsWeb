using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTransaction.DAL.Implementation.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> Paginate<T>(this ICollection<T> source, int pageIndex, int pageSize)
        {
          return source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}

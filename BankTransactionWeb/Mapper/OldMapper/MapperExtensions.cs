using BankTransaction.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public static class MapperExtensions
    {
        public static TDestination Map<TSource, TDestination>(this IMapper<TSource, TDestination> mapper, TSource from) where TDestination : new()
        {
            return mapper.Map(from);

        }

        public static TSource MapBack<TSource, TDestination>(this IMapper<TSource, TDestination> mapper, TDestination from) where TSource : new()
        {
            return mapper.MapBack(from);

        }

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Mapper
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource MapBack(TDestination destination);
    }
}

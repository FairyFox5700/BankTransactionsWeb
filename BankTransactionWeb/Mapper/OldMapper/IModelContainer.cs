using System.Diagnostics;
using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;

namespace BankTransaction.Web.Mapper.OldMapper
{
    public interface IMapperService
    {
         TDestination Map<TSource, TDestination>();
         TDestination MapBack<TSource, TDestination>();
    }
    

}
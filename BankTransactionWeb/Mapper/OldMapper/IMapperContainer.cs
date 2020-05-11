using BankTransaction.Configuration;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Mapper
{
    public interface IMapperContainer
    {
      
    }
    public class MapperContainer : IMapperContainer
    {
        private Dictionary<string, object> mappers;
        public IMapper<TSource, TDestination> GetMapper<TSource, TDestination>(string type) where TDestination : new()
        {
            if (mappers == null)
            {
                mappers = new Dictionary<string, object>();
            }
            if (mappers.ContainsKey(type))
            {
                return (IMapper<TSource, TDestination>)mappers[type];
            }
            else
                return null;
        }

       
    }
}

using RestSharp;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract.RestApi
{
    public interface IRestApiHelper
    {
        T Execute<T>(string resource, object body,  Method method, object parameters = null);

        T ExecuteApiRequest<T>(string resource, object body, Method method, object parameters = null);
        Task<T> ExecuteAsync<T>(string resource, object body, Method method, object parameters = null, string token = null);
        Task<T> ExecuteApiRequestAsync<T>(string resource, object body, Method method, object parameters = null, string token = null);
    }
}

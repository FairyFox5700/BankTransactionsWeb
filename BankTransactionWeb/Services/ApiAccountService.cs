using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.Helpers;

namespace BankTransaction.Web.Services
{
    public class ApiAccountService: IApiBankAccountService
    {
        private readonly IRestApiHelper restApiHelper;
        
       
        public ApiAccountService(IRestApiHelper restApiHelper)
        {
            this.restApiHelper = restApiHelper;
        }
        [ExceptionHandlerFilter]
        public async Task<ApiDataResponse<PaginatedList<AccountDTO>>> GetAllAccounts(PageQueryParameters pageQueryParameters = null)
        {
            var allAccount = (await restApiHelper.ExecuteAsync<ApiDataResponse<PaginatedList<AccountDTO>>>(API.Account.Acounts, null, Method.GET, parameters: new { pageQueryParameters.PageSize, pageQueryParameters.PageNumber }));//, tokenData.Data.Token
            return allAccount;

        }

        public async Task<ApiDataResponse<AccountDTO>> AddAccount(AccountDTO account)
        { 
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.Acounts, body: account, Method.POST,null);
            return result;
        }

        public async Task<ApiDataResponse<AccountDTO>> UpdateAccount(AccountDTO account)
        {
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.UpdateAccount(account.Id), account, Method.PUT,null);//API.Account.Acounts+"/"+ account.Id
            return result;
        }


        public async Task<ApiDataResponse<AccountDTO>> DeleteAccount( int id)
        {
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.DeleteAccount(id), null, Method.DELETE, null);//API.Account.Acounts+"/"+ account.Id
            return result;
        }
    }
}

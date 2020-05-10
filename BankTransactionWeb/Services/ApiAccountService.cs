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

namespace BankTransaction.Web.Services
{
    public class ApiAccountService: IApiBankAccountService
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<ApiAccountService> logger;
        private readonly PersonDTO model;
        #region test data to remove
        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string Test_PASSWORD = "qWerty1_";
        #endregion
        public ApiAccountService(IRestApiHelper restApiHelper, IHttpContextAccessor httpContextAccessor, ILogger<ApiAccountService> logger)
        {
            this.restApiHelper = restApiHelper;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.model = model = new PersonDTO() { Email = Test_EMAIL, Password = Test_PASSWORD };
        }

        public async Task<ApiDataResponse<List<AccountDTO>>> GetAllAccounts(PageQueryParameters pageQueryParameters = null)
        {
            ///FOR TEST ONLY 
            var tokenData = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login", body: model, Method.POST);
            var allAccount = await restApiHelper.ExecuteAsync<ApiDataResponse<List<AccountDTO>>>(API.Account.Acounts, null, Method.GET, parameters: new { pageQueryParameters.PageSize, pageQueryParameters.PageNumber }, tokenData.Data.Token);
            return allAccount;

        }

        public async Task<ApiDataResponse<AccountDTO>> AddAccount(AccountDTO account)
        { ///FOR TEST ONLY 
            var tokenData = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login", body: model, Method.POST);
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.Acounts, body: account, Method.POST, tokenData.Data.Token);
            return result;
        }

        public async Task<ApiDataResponse<AccountDTO>> UpdateAccount(AccountDTO account)
        {///FOR TEST ONLY 
            var tokenData = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login", body: model, Method.POST);
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.Acounts, body: account, Method.PUT, parameters: new { account.Id }, tokenData.Data.Token);
            return result;
        }


        public async Task<ApiDataResponse<AccountDTO>> DeleteAccount( int id)
        {///FOR TEST ONLY 
            var tokenData = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login", body: model, Method.POST);
            var result = await restApiHelper.ExecuteAsync<ApiDataResponse<AccountDTO>>(API.Account.Acounts, null, Method.DELETE, parameters: new { id }, tokenData.Data.Token);
            return result;
        }
    }
}

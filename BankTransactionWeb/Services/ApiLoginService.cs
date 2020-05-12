using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Configuration;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Services
{
    public class ApiLoginService: IApiLoginService
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly ICookieHelperService helperService;
        public ApiLoginService(IRestApiHelper restApiHelper, ICookieHelperService helperService)
        {
            this.restApiHelper = restApiHelper;
            this.helperService = helperService;
        }

        public async Task<ApiDataResponse<AuthSuccesfullModel>> LoginPerson(PersonDTO person)
        {
            var authResultw = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>(API.Authorization.Login, body: person, Method.POST);
            helperService.AddReplaceCookie("BankWeb.AspNetCore.ProductKey", authResultw.Data.Token);
            helperService.AddReplaceCookie("BankWeb.AspNetCore.ProductKeyFree", authResultw.Data.RefreshToken);
            return authResultw;
        }

        public async Task<ApiDataResponse<AuthSuccesfullModel>>  RefreshToken ( string token, string refreshToken )
        {
            var refeshedValue = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>(API.Authorization.Refresh, body: new { Token = token, RefreshToken = refreshToken }, Method.POST, null).ConfigureAwait(false);
            helperService.AddReplaceCookie("BankWeb.AspNetCore.ProductKey", refeshedValue.Data?.Token);
            helperService.AddReplaceCookie("BankWeb.AspNetCore.ProductKeyFree", refeshedValue.Data?.RefreshToken);
            return refeshedValue;
        }

        //public async Task LogoutPerson ()
        //{
        //    var authResultw = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>(API.Authorization.Logout, body: person, Method.POST);
        //}
    }
}

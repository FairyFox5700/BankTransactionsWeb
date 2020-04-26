using BankTransaction.Api.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public class RestApiHelper
    {
        static readonly string apiUrl = ConfigurationManager.AppSettings["api_url"] ?? "http://localhost:64943/api/";
        public static readonly RestClient Client = new RestClient();
       
        private static RestRequest ConstructRequest(string resource, object body, string token,object parameters)
        {
            var request = new RestRequest(apiUrl+resource, body == null ? Method.GET : Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            
            if(!String.IsNullOrEmpty( token))
            {
                //request.UseDefaultCredentials = true;
                //request.AddHeader("Authorization", string.Format("Bearer {0}", token));
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //Client.Authenticator = new JwtAuthenticator(token);
                //Client.Authenticator.Authenticate(Client, request);
                //Client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));
                //request.AddParameter("token", token ?? "", ParameterType.UrlSegment);
                // request.ContentType = "application/json";

            }
            if (body != null)
                request.AddBody(body);
            if (parameters != null)
            {
                Dictionary<string, object> requestDictionary = CreateRequestDictionary(parameters);
                foreach (var keyValue in requestDictionary)
                {
                    request.AddParameter(keyValue.Key, keyValue.Value);
                }
            }
            return request;
        }

        private static Dictionary<string, object> CreateRequestDictionary(object parameters)
        {
            var objectType = parameters.GetType();
            var properties = objectType.GetProperties();
            var keyValue = properties.ToDictionary
                (p => p.Name,
                p => p.GetValue(parameters)
                );
            return keyValue;
        }

        public T Execute<T>(string resource, object body, string token, object parameters = null)
        {
            var request = ConstructRequest(resource, body,token ,parameters);
            var responce = Client.Execute<T>(request);
            ValidateResponce(responce);
            var result = responce.Data;
            return result;
        }
        public object ExecuteApiRequest<T>(string resource, object body, string token, object parameters = null)
        {
            //ApiResponse 
            var request = ConstructRequest(resource, body, token, parameters);
            //request.AddHeader("Barear", token);
            var responce = Client.Execute<ApiResponse>(request);
            ValidateApiResponce(responce);
            var result = responce.Data;
            return result.Result;
        }

        private void ValidateApiResponce(IRestResponse<ApiResponse> responce)
        {
            ValidateResponce(responce);
            var result = responce.Data;
            if (result.IsError)
                throw new Exception(result.ResponseException.Message);
        }

        private void ValidateResponce<T>(IRestResponse<T> responce)
        {
            if (responce.ResponseStatus != ResponseStatus.Completed)
                if (responce.ErrorException != null)
                    throw responce.ErrorException;
                else
                    throw new Exception(responce.ErrorMessage);
            if (!String.IsNullOrEmpty(responce.ErrorMessage))
                throw new Exception(responce.ErrorMessage);
            if (responce.StatusCode != HttpStatusCode.OK)
                throw new Exception(responce.Content);
        }
    }
}

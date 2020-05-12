using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract.RestApi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BankTransaction.Api.Models.Responces;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public class RestApiHelper: IRestApiHelper
    {
        static readonly string apiUrl = ConfigurationManager.AppSettings["api_url"] ?? "http://localhost:64943/api/";
        private readonly IRestClient Client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RestApiHelper(IHttpContextAccessor httpContextAccessor)
        {
            Client = new RestClient(apiUrl);
            Client.CookieContainer = new System.Net.CookieContainer();
            
            this.httpContextAccessor = httpContextAccessor;
        }
        private string GetAuthorizationHeader()
        {
            var authorizationHeader = httpContextAccessor
               .HttpContext.Request.Headers["Authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
        private RestRequest ConstructRequest(string resource, object body,  Method method, object parameters)//string token,
        {
            var request = new RestRequest(apiUrl+resource, method)
            {
                RequestFormat = DataFormat.Json
            };
            var tokenFromHeader = GetAuthorizationHeader();
            if ( !String.IsNullOrEmpty(tokenFromHeader))
                request.AddHeader("Authorization", "Bearer " + tokenFromHeader);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Accept", "application/json");
            if (body != null)
                request.AddJsonBody(body);
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

        public T Execute<T>(string resource, object body, Method method, object parameters = null)
        {
            var request = ConstructRequest(resource, body, method,parameters);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            var responce = Client.Execute<T>(request);
            ValidateResponce(responce);
            var result = responce.Content;
            var jsonResponse = JsonConvert.DeserializeObject<T>(result);
            return jsonResponse;
        }

        public async Task<T> ExecuteAsync<T>(string resource, object body,  Method method, object parameters = null)
        {
            var request = ConstructRequest(resource, body, method, parameters);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            var responce =  await Client.ExecuteAsync<T>(request);
            ValidateResponce(responce);
            var result = responce.Content;
            var jsonResponse = JsonConvert.DeserializeObject<T>(result);
            return jsonResponse;
        }
        
        //https://stackoverflow.com/questions/37329354/how-to-use-ihttpcontextaccessor-in-static-class-to-set-cookies
        public async Task<T> ExecuteApiRequestAsync<T>(string resource,  object body,  Method method, object parameters = null)
        {
            var request = ConstructRequest(resource, body,  method, parameters);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            IRestResponse<ApiDataResponse<T>> responce = await Client.ExecuteAsync<ApiDataResponse<T>>(request);
            ValidateApiResponce(responce);
            var result = responce.Content;
            var jsonResponse = JsonConvert.DeserializeObject<ApiDataResponse<T>>(result);
            return jsonResponse.Data;
        } 
        private string ValidateJSonString (string jsonString)
        {
            var index = jsonString.IndexOf("Formatters");
            if( index<0)
            {
                return jsonString;
            }
            return jsonString.Substring(0, index - 2)+"}";
        }
        public T ExecuteApiRequest<T>(string resource, object body,  Method method, object parameters = null)
        {
            var request = ConstructRequest(resource, body,method,parameters);
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            IRestResponse<ApiDataResponse<T>> responce = Client.Execute<ApiDataResponse<T>>(request);
            ValidateApiResponce(responce);
            var result = responce.Content;
            var jsonResponse = JsonConvert.DeserializeObject<ApiDataResponse<T>>(result);
            return jsonResponse.Data;
        }

        private void ValidateApiResponce<T>(IRestResponse<ApiDataResponse<T>> responce)
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
                    throw new BankApiException(responce.ErrorMessage);
            if (!String.IsNullOrEmpty(responce.ErrorMessage))
                throw new BankApiException(responce.ErrorMessage);
            //if (responce.StatusCode != HttpStatusCode.OK)
                //throw new BankApiException(responce.Content);
        }
    }

    public class NewtonsoftJsonSerializer : IDeserializer, ISerializer
    {
        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string ContentType
        {
            get { return "application/json"; }
            set { }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer();
    }

}

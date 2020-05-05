using BankTransaction.Api.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            if (!String.IsNullOrEmpty( token))
            {
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Accept", "application/json");

            }
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
            var responce = Client.Execute<ApiResponse<T>>(request);
            ValidateApiResponce(responce);
            var result = responce.Data;
            return result.Data;
        }

        private void ValidateApiResponce<T>(IRestResponse<ApiResponse<T>> responce)
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

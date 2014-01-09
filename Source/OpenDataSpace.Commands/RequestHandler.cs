using OpenDataSpace.Commands.RequestData;
using OpenDataSpace.RequestData.RequestData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands
{
    internal class RequestHandler
    {
        private readonly IRestClient _client;

        readonly string _username;
        readonly string _password;
        readonly string _sessionId;

        public RequestHandler(string username, string password, string hostname)
        {
            _client = new RestClient(UrlFromHostname(hostname));
            _username = username;
            _password = password;
        }

        public RequestHandler(string sessionId, string hostname)
        {
            _client = new RestClient(UrlFromHostname(hostname));
            _sessionId = sessionId;
        }

        public RequestHandler(IRestClient restClient)
        {   
            _client = restClient;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            request.RequestFormat = DataFormat.Json;
            var response = _client.Execute<T>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                string message = String.Format("Error retrieving response: {0}.", response.ErrorMessage);
                throw new ApplicationException(message, response.ErrorException);
            }
            return response.Data;
        }
        
        public string Login()
        {
            var request = new RestRequest(ResourceUris.Login, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new LoginRequest {
                username = _username,
                password = _password
            });
            var response = Execute<LoginResponse>(request);
            if (response == null)
            {
                // TODO: Connection failed
                return null;
            }
            if (!response.success)
            {
                // TODO: throw error, with error code and message
                return null;
            }
            return response.sessionId;
        }

        private string UrlFromHostname(string hostname)
        {
            return string.Format("https://{0}", hostname);
        }
    }
}

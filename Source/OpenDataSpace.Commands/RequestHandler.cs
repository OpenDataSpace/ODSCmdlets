using OpenDataSpace.Commands.Requests;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands
{
    internal class RequestHandler
    {
        private readonly IRestClient _client;

        readonly string _username;
        readonly SecureString _password;
        string _sessionId;

        public RequestHandler(string username, string password, string url)
            : this(username, Utility.StringToSecureString(password), url)
        {
        }

        public RequestHandler(string username, SecureString password, string url)
        {
            _client = new RestClient(VildateURL(url));
            _username = username;
            _password = password;
        }

        public RequestHandler(string sessionId, string url)
        {
            _client = new RestClient(VildateURL(url));
            _sessionId = sessionId;
        }

        public RequestHandler(IRestClient restClient)
        {
            _client = restClient;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            request.RequestFormat = DataFormat.Json;
            IRestResponse<T> response = _client.Execute<T>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                string message = String.Format("Error retrieving response: {0}. {1}",
                    response.ResponseStatus.ToString(), response.ErrorMessage);
                throw new RequestFailedException(message, "RequestFailed", response.ErrorException);
            }
            return response.Data;
        }

        public T Execute<T>(DataspaceRequest request) where T : DataspaceResponse, new()
        {
            var response = Execute<T>(request.CreateRestRequest(_sessionId));
            if (response == null)
            {
                string message = String.Format("{0} request failed. Maybe the URL is incorrect?",
                    request.RequestName);
                throw new RequestFailedException(message,
                    "ResponseDataIsNull");
            }
            return response;
        }

        public T SuccessfullyExecute<T>(DataspaceRequest request) where T : DataspaceResponse, new()
        {
            var response = Execute<T>(request);
            if (!response.Success)
            {
                string message = String.Format("{0} failed: {1}. Error Code: {2}",
                    request.RequestName, response.Message, response.ErrorCode);
                throw new RequestFailedException(message, "DataspaceRequestNotSuccessfull");
            }
            return response;
        }

        public T ExecuteAndUnpack<T>(ObjectRequest request) where T : new()
        {
            var response = SuccessfullyExecute<ObjectResponse<T>>(request);
            return response.Data;
        }

        public string Login()
        {
            if (String.IsNullOrEmpty(_username) || _password == null)
            {
                // TODO: throw exception
            }
            var request = new LoginRequest(_username, _password);
            var response = SuccessfullyExecute<LoginResponse>(request);
            _sessionId = response.SessionId;
            return _sessionId;
        }

        public bool Logout()
        {
            var request = new LogoutRequest();
            var response = Execute<DataspaceResponse>(request);
            return response.Success;
        }

        private string VildateURL(string url)
        {
            // TODO: check first if protocol is already part of url
            return string.Format("https://{0}", url);
        }
    }
}

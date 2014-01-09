using OpenDataSpace.Commands.RequestData;
using OpenDataSpace.RequestData.RequestData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands
{
    internal class RequestHandler
    {
        private readonly IRestClient _client;

        readonly string _username;
        readonly SecureString _password;
        readonly string _sessionId;

        public RequestHandler(string username, string password, string hostname)
            : this(username, ToSecureString(password), hostname)
        {
        }

        public RequestHandler(string username, SecureString password, string hostname)
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
                string message = String.Format("Error retrieving response: {0}. {1}",
                    response.ResponseStatus.ToString(), response.ErrorMessage);
                throw new ConnectionFailedException(message, "RequestFailed", response.ErrorException);
            }
            return response.Data;
        }
        
        public string Login()
        {
            var request = new RestRequest(ResourceUris.Login, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new LoginRequest {
                username = _username,
                password = ToInsecureString(_password)
            });
            var response = Execute<LoginResponse>(request);
            if (response == null)
            {
                throw new ConnectionFailedException("Login request failed. Maybe the URL is incorrect?",
                    "ResponseDataIsNull");
            }
            if (!response.success)
            {
                string message = String.Format("Login failed: {0}. Error Code: {1}",
                    response.message, response.errorCode);
                throw new ConnectionFailedException(message, "ODSLoginError");
            }
            return response.sessionId;
        }


        internal static SecureString ToSecureString(string str)
        {
            var ss = new SecureString();
            foreach (char c in str.ToCharArray())
            {
                ss.AppendChar(c);
            }
            ss.MakeReadOnly();
            return ss;
        }

        private static string ToInsecureString(SecureString secureStr)
        {
            var bstr = Marshal.SecureStringToBSTR(secureStr);
            try
            {
                string str = Marshal.PtrToStringBSTR(bstr);
                return str;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }

        private string UrlFromHostname(string hostname)
        {
            return string.Format("https://{0}", hostname);
        }
    }
}

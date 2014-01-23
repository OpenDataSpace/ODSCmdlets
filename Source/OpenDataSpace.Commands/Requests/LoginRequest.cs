using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    class LoginRequest : DataspaceRequest
    {
        private string _username;
        private SecureString _password;

        public LoginRequest(string username, SecureString password)
        {
            RequestName = "Login";
            _username = username;
            _password = password;
        }

        public override RestRequest CreateRestRequest(AuthMethod authMethod, string sessionId)
        {
            // authMethod and sessionId are useless, we're not logged in anyway!
            var request = new RestRequest(ResourceUris.Login, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {
                username = _username,
                password = Utility.SecureStringToString(_password)
            });
            return request;
        }
    }
}

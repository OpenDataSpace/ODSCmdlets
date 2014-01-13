using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class LoginRequest : DataspaceRequest
    {
        private string _username;
        private SecureString _password;

        public override string RequestName
        {
            get { return "Login"; }
        }

        public LoginRequest(string username, SecureString password)
        {
            _username = username;
            _password = password;
        }

        public override RestRequest CreateRestRequest()
        {
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

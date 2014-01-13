using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class LogoutRequest : DataspaceRequest
    {
        private string _sessionId;

        public override string RequestName
        {
            get { return "Logout"; }
        }

        public LogoutRequest(string sessionId)
        {
            _sessionId = sessionId;
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest(ResourceUris.Logout, Method.POST);
            request.AddParameter(SessionIdParameterName, _sessionId);
            return request;
        }
    }
}

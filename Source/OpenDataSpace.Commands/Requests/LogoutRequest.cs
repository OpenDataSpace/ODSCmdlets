using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    class LogoutRequest : DataspaceRequest
    {

        public LogoutRequest()
            : base()
        {
            RequestName = "Logout";
        }

        public override RestRequest CreateRestRequest(string sessionId)
        {
            var request = new RestRequest(ResourceUris.Logout, Method.POST);
            request.AddParameter(SessionIdParameterName, sessionId);
            return request;
        }
    }
}

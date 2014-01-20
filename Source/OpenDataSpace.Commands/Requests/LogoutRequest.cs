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

        public override RestRequest CreateRestRequest(AuthMethod authMethod, string sessionId)
        {
            var request = new RestRequest(ResourceUris.Logout, Method.POST);
            SetAuthentication(request, authMethod, sessionId);
            return request;
        }
    }
}

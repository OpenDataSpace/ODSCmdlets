using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public abstract class DataspaceRequest
    {
        public enum AuthMethod
        {
            SessionId,
            Cookie
        }

        internal const string SessionIdParameterName = "sessionId";
        internal const string SessionIdCookieName = "JSESSIONID";

        public string RequestName { get; set; }

        public abstract RestRequest CreateRestRequest(AuthMethod authMethod, string sessionId);

        protected void SetAuthentication(RestRequest request, AuthMethod authMethod, string sessionId)
        {
            if (String.IsNullOrEmpty(sessionId)) // Check if we have a valid id
            {
                return;
            }
            if (authMethod == AuthMethod.SessionId)
            {
                request.AddParameter(SessionIdParameterName, sessionId);
            }
            else // Cookie
            {
                request.AddCookie(SessionIdCookieName, sessionId);
            }
        }
    }
}

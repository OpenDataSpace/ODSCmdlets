using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    public abstract class DataspaceRequest
    {
        internal const string SessionIdParameterName = "sessionId";
        public string RequestName { get; set; }


        public abstract RestRequest CreateRestRequest(string sessionId);
    }
}

using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    public abstract class DataspaceRequest
    {
        public const string SessionIdParameterName = "sessionId";
        public abstract string RequestName { get; }

        public abstract RestRequest CreateRestRequest();
    }
}

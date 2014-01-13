using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class ObjectRequest : DataspaceRequest
    {
        private string _sessionId;
        private Method _method;
        private List<KeyValuePair<string, object>> _parameters;

        public override string RequestName
        {
            get { return "Object"; }
        }

        public ObjectRequest(string sessionId, Method method)
        {
            _sessionId = sessionId;
            _method = method;
            _parameters = new List<KeyValuePair<string, object>>();
        }

        public override RestRequest CreateRestRequest()
        {
            var request = new RestRequest(ResourceUris.Object, _method);
            request.AddParameter(SessionIdParameterName, _sessionId);
            foreach (var param in _parameters)
            {
                if (param.Value is string)
                {
                    request.AddParameter(param.Key, param.Value);
                }
                else if (param.Value is int)
                {
                    request.AddParameter(param.Key, param.Value.ToString());
                }
                else
                {
                    request.AddParameter(param.Key, request.JsonSerializer.Serialize(param.Value));
                }
            }
            return request;
        }

        public void AddParameter(string name, object value)
        {
            _parameters.Add(new KeyValuePair<string, object>(name, value));
        }

        public void AskForProperty(string name)
        {
            AddParameter("properties", name);
        }
    }
}

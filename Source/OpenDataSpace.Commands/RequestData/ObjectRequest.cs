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
        private Method _method;
        private List<KeyValuePair<string, object>> _parameters;

        public long ObjectId { get; set; }

        public ObjectRequest( Method method)
        {
            RequestName = "Object";
            _method = method;
            _parameters = new List<KeyValuePair<string, object>>();
        }

        public override RestRequest CreateRestRequest(string sessionId)
        {
            var request = new RestRequest(BuildUri(), _method);
            request.AddParameter(SessionIdParameterName, sessionId);
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
                    var serialized = request.JsonSerializer.Serialize(param.Value);
                    request.AddParameter(param.Key, serialized);
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

        public void SetData(object data)
        {
            AddParameter("data", data);
        }

        private string BuildUri()
        {
            if (ObjectId < 1)
            {
                return ResourceUris.Object;
            }
            return String.Format("{0}/{1}", ResourceUris.Object, ObjectId);
        }
    }
}

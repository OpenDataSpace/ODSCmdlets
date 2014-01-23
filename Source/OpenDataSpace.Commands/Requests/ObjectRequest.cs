﻿using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public class ObjectRequest : DataspaceRequest
    {
        private Method _method;
        private List<KeyValuePair<string, object>> _parameters;

        public long ObjectId { get; set; }
        public object Data { get; set; }
        public object Body { get; set; }

        public ObjectRequest(Method method)
            : this("Object", method)
        {
        }

        public ObjectRequest(string requestName, Method method)
        {
            RequestName = requestName;
            _method = method;
            _parameters = new List<KeyValuePair<string, object>>();
        }

        public override RestRequest CreateRestRequest(AuthMethod authMethod, string sessionId)
        {
            var request = new RestRequest(BuildUri(), _method);
            request.RequestFormat = DataFormat.Json;
            SetAuthentication(request, authMethod, sessionId);
            if (Data != null)
            {
                AddParameter("data", Data); // will be handled by upcoming loop
            }
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
            if (Body != null)
            {
                request.AddBody(Body);
            }
            return request;
        }

        public void AddParameter(string name, object value)
        {
            _parameters.Add(new KeyValuePair<string, object>(name, value));
        }

        public void AskForProperties(params string[] names)
        {
            foreach (var name in names)
            {
                AddParameter("properties", name);
            }
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

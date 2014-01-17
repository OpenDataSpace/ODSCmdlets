using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class GroupRequestFactory
    {
        private const string _handler = "dataspacegroup";
        private const string _provider = "dataspacedirectoryobjects";

        private static ObjectRequest CreateBasic(string requestName, Method method)
        {
            var request = new ObjectRequest(method);
            request.RequestName = requestName;
            return request;
        }

        private static ObjectRequest CreateNamedObjectRequest(string requestName, Method method)
        {
            var request = CreateBasic(requestName, method);
            request.AskForProperty("name");
            request.AskForProperty("id");
            return request;
        }

        public static ObjectRequest CreateGetGroupsRequest(bool globalGroups)
        {
            var request = CreateNamedObjectRequest("Get Groups", Method.GET);
            request.AddParameter("provider", _provider);
            request.AddParameter("parameters", new
            {
                resultset = globalGroups ? "globalgroups" : "privategroups"
            });
            return request;
        }

        public static ObjectRequest CreateAddGroupRequest(string groupName, bool globalGroup)
        {
            var request = CreateNamedObjectRequest("Add Group", Method.POST);
            request.SetData(new
            {
                name = groupName,
                globalgroup = globalGroup
            });
            request.AddParameter("handler", _handler);
            return request;
        }

        public static ObjectRequest CreateDeleteGroupRequest(long id)
        {
            var request = CreateBasic("Delete Group", Method.DELETE);
            request.SetData(new
            {
                id = id
            });
            request.ObjectId = id;
            return request;
        }
    }
}

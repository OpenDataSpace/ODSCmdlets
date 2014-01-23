using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public static class GroupRequestFactory
    {
        private const string _handler = "dataspacegroup";
        private const string _provider = "dataspacedirectoryobjects";

        private static ObjectRequest CreateNamedObjectRequest(string requestName, Method method)
        {
            var request = new ObjectRequest(requestName, method);
            request.AskForProperties("name", "id");
            return request;
        }

        public static ObjectRequest CreateGetGroupsRequest(bool globalGroups)
        {
            return CreateGetGroupsRequest("", globalGroups);
        }

        public static ObjectRequest CreateGetGroupsRequest(string query, bool globalGroups)
        {
            var request = CreateNamedObjectRequest("Get Groups", Method.GET);
            request.AddParameter("provider", _provider);
            request.AddParameter("query", query);
            request.AddParameter("sort", new
            {
                property = "name",
                direction = "ASC"
            });
            request.AddParameter("parameters", new
            {
                resultset = globalGroups ? "globalgroups" : "privategroups"
            });
            return request;
        }

        public static ObjectRequest CreateAddGroupRequest(string groupName, bool globalGroup)
        {
            var request = CreateNamedObjectRequest("Add Group", Method.POST);
            request.Data = new
            {
                name = groupName,
                globalgroup = globalGroup
            };
            request.AddParameter("handler", _handler);
            return request;
        }

        public static ObjectRequest CreateDeleteGroupRequest(long id)
        {
            var request = new ObjectRequest("Delete Group", Method.DELETE);
            request.AddParameter("handler", _handler);
            request.Data = new
            {
                id = id
            };
            request.ObjectId = id;
            return request;
        }


        /*
        * member management: add/delete
        POST/DELETE
        /groupmanagement/{groupname}
        globalgroup=<bool>
        ids=<id>
        ids=<id>
        */


        /*
         * get group members (object request)
            provider=dataspacegroupmember
            source=<groupname>
            properties=id
            properties=name
            parameters	{"globalgroup":<bool>}
         */
    }
}

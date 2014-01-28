using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public static class GroupMemberRequestFactory
    {
        private const string _defaultProvider = "dataspacegroupmember";

        public static DataspaceRequest CreateAddGroupMemberRequest(string groupName, bool globalGroup, long userId)
        {
            return new GroupManagementRequest(Method.POST, groupName, globalGroup, userId);
        }

        public static DataspaceRequest CreateRemoveGroupMemberRequest(string groupName, bool globalGroup, long userId)
        {
            return new GroupManagementRequest(Method.DELETE, groupName, globalGroup, userId);
        }

        public static ObjectRequest CreateGetGroupMembersRequest(int start, int limit, string groupName, bool globalGroup)
        {
            var request = new ObjectRequest("Get Group Members", Method.GET);
            request.AddParameter("provider", _defaultProvider);
            request.AskForProperties("id", "name");
            request.AddParameter("parameters", new { globalgroup = globalGroup });
            request.AddParameter("start", start);
            request.AddParameter("limit", limit);
            request.AddParameter("source", groupName);
            return request;
        }

    }
}

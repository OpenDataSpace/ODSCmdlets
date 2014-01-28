using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    class GroupManagementRequest : DataspaceRequest
    {
        private Method _method;
        private string _groupName;
        private bool _globalGroup;
        private long _userId;

        public GroupManagementRequest(Method method, string groupName, bool globalGroup, long userId)
        {
            RequestName = "Group Management";
            _method = method;
            _globalGroup = globalGroup;
            _groupName = groupName;
            _userId = userId;
        }

        public override RestRequest CreateRestRequest(AuthMethod authMethod, string sessionId)
        {
            var request = new RestRequest(BuildUrl(), _method);
            SetAuthentication(request, authMethod, sessionId);
            request.AddParameter("ids", _userId);
            request.AddParameter("globalgroup", _globalGroup);
            return request;
        }

        private string BuildUrl()
        {
            return ResourceUris.GroupManagement + "/" + _groupName;
        }
    }
}

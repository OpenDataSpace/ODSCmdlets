using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class GroupTestBase : TestBase
    {
        private HashSet<long> _autoRemoveGroups = new HashSet<long>();

        protected ObjectResponse<NamedObject> DoAddGroup(string name, bool globalGroup, bool autoRemove)
        {
            var req = GroupRequestFactory.CreateAddGroupRequest(name, globalGroup);
            var response = RequestHandler.Execute<ObjectResponse<NamedObject>>(req);
            if (autoRemove)
            {
                _autoRemoveGroups.Add(response.Data.Id); //for cleanup
            }
            return response;
        }

        protected void DoRemoveAddedGroups()
        {
            //TODO: get the group with _testGroupName and remove it when found instead of using _newGroups
            foreach (long id in _autoRemoveGroups)
            {
                var req = GroupRequestFactory.CreateDeleteGroupRequest(id);
                RequestHandler.ExecuteSuccessfully<DataspaceResponse>(req);
            }
            _autoRemoveGroups.Clear();
        }

        protected void AutoRemoveGroup(long id)
        {
            _autoRemoveGroups.Add(id);
        }
    }
}

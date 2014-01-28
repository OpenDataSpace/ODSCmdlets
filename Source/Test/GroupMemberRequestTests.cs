using NUnit.Framework;
using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    [TestFixture]
    class GroupMemberRequestTests : GroupTestBase
    {
        private const string _testGroupName = "__groupMemberTestGroup";

        [TearDown]
        public void RemoveAddedGroups()
        {
            DoRemoveAddedGroups();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAndGetMember(bool globalGroup)
        {
            var user = GetAbritraryUser();
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
            var addRequest = GroupMemberRequestFactory.CreateAddGroupMemberRequest(group.Name, globalGroup, user.Id);
            var addResponse = RequestHandler.Execute<DataspaceResponse>(addRequest);
            Assert.True(addResponse.Success, addResponse.Message);

            var request = GroupMemberRequestFactory.CreateGetGroupMembersRequest(0, 30, _testGroupName, globalGroup);
            var members = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
            Assert.AreEqual(1, members.Count);
            var member = members[0];
            Assert.NotNull(member);
            Assert.AreEqual(user.Id, member.Id);
            Assert.AreEqual(user.UserName, member.Name);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddAndRemoveMember(bool globalGroup)
        {
            var user = GetAbritraryUser();
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
            var addRequest = GroupMemberRequestFactory.CreateAddGroupMemberRequest(group.Name, globalGroup, user.Id);
            var addResponse = RequestHandler.Execute<DataspaceResponse>(addRequest);
            Assert.True(addResponse.Success, addResponse.Message);

            var removeRequest = GroupMemberRequestFactory.CreateRemoveGroupMemberRequest(group.Name, globalGroup, user.Id);
            var removeResponse = RequestHandler.Execute<DataspaceResponse>(removeRequest);
            Assert.True(removeResponse.Success);

            var request = GroupMemberRequestFactory.CreateGetGroupMembersRequest(0, 30, _testGroupName, globalGroup);
            var members = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
            Assert.AreEqual(0, members.Count);
        }
    }
}

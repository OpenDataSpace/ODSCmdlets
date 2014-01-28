using NUnit.Framework;
using OpenDataSpace.Commands;
using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    [TestFixture]
    class GroupMemberCmdletsTests : GroupTestBase
    {
        private const string _testGroupName = "__groupMemberTestGroup";

        [TearDown]
        public void RemoveAddedGroups()
        {
            DoRemoveAddedGroups();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddMemberCmdlet(bool globalGroup)
        {
            var user = GetAbritraryUser();
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
            var scope = globalGroup ? ODSGroupCommandBase.GroupScope.Global : ODSGroupCommandBase.GroupScope.Private;

            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(AddODSGroupMemberCommand)),
                    "-Scope",
                    scope.ToString(),
                    "-GroupName",
                    SingleQuote(group.Name),
                    "-Id",
                    user.Id.ToString()
                })
            };
            Shell.Execute(commands); // would throw an exception on failure

            // make sure the user is added successfully
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
        public void GetMemberCmdlet(bool globalGroup)
        {
            var user = GetAbritraryUser();
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
            var addRequest = GroupMemberRequestFactory.CreateAddGroupMemberRequest(group.Name, globalGroup, user.Id);
            var addResponse = RequestHandler.Execute<DataspaceResponse>(addRequest);
            Assert.True(addResponse.Success, addResponse.Message);
            var scope = globalGroup ? ODSGroupCommandBase.GroupScope.Global : ODSGroupCommandBase.GroupScope.Private;

            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSGroupMemberCommand)),
                    "-Scope",
                    scope.ToString(),
                    "-GroupName",
                    SingleQuote(group.Name)
                })
            };
            var members = Shell.Execute(commands); // would throw an exception on failure
            Assert.AreEqual(1, members.Count);
            var member = members[0] as NamedObject;
            Assert.NotNull(member);
            Assert.AreEqual(user.Id, member.Id);
            Assert.AreEqual(user.UserName, member.Name);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RemoveMemberCmdlet(bool globalGroup)
        {
            var user = GetAbritraryUser();
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
            var addRequest = GroupMemberRequestFactory.CreateAddGroupMemberRequest(group.Name, globalGroup, user.Id);
            var addResponse = RequestHandler.Execute<DataspaceResponse>(addRequest);
            var scope = globalGroup ? ODSGroupCommandBase.GroupScope.Global : ODSGroupCommandBase.GroupScope.Private;
            Assert.True(addResponse.Success, addResponse.Message);

            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(RemoveODSGroupMemberCommand)),
                    "-Scope",
                    scope.ToString(),
                    "-GroupName",
                    SingleQuote(group.Name),
                    "-Id",
                    user.Id.ToString()
                })
            };
            Shell.Execute(commands); // would throw an exception on failure

            var request = GroupMemberRequestFactory.CreateGetGroupMembersRequest(0, 30, _testGroupName, globalGroup);
            var members = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
            Assert.AreEqual(0, members.Count);
        }
    }
}

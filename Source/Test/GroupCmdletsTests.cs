using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenDataSpace.Commands.Requests;
using OpenDataSpace.Commands;
using OpenDataSpace.Commands.Objects;

namespace Test
{
    [TestFixture]
    class GroupCmdletsTests : TestBase
    {
        private string _testGroupName = "__testGroup";
        private string _testGroupName2 = "__testGroup2";
        private List<long> _removeGroups = new List<long>();

        private NamedObject DoAddGroup(string name, bool globalGroup)
        {
            return RequestHandler.ExecuteAndUnpack<NamedObject>(GroupRequestFactory.CreateAddGroupRequest(name, globalGroup));
        }

        private void DoRemoveGroup(long id)
        {
            RequestHandler.SuccessfullyExecute<DataspaceResponse>(GroupRequestFactory.CreateDeleteGroupRequest(id));
        }

        [TearDown]
        public void RemoveAddedGroups()
        {
            foreach (var id in _removeGroups)
            {
                DoRemoveGroup(id);
            }
            _removeGroups.Clear();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddGroupCmdlet(bool globalGroup)
        {
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(AddODSGroupCommand)),
                    "-Name",
                    SingleQuote(_testGroupName),
                    "-Scope",
                    globalGroup ? ODSGroupCommandBase.GroupScope.Global.ToString()
                                : ODSGroupCommandBase.GroupScope.Private.ToString()
                })
            };
            var group = Shell.Execute(commands);
            Assert.AreEqual(1, group.Count, "No group object returned after adding");
            var groupData = group[0] as NamedObject;
            Assert.IsNotNull(groupData, "Returned object is no NamedObject");
            Assert.Greater(groupData.Id, 0, "Group ID is invalid");
            _removeGroups.Add(groupData.Id);
            Assert.IsNotNullOrEmpty(groupData.Name);
            // TODO: get group(s) and check for scope
        }

        [Test]
        public void AddGroupCmdletViaPipeline()
        {
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    String.Format("{0},{1}", SingleQuote(_testGroupName), SingleQuote(_testGroupName2)),
                    "|",
                    CmdletName(typeof(AddODSGroupCommand)),
                    "-Scope",
                    ODSGroupCommandBase.GroupScope.Private.ToString()
                })
            };
            var groups = Shell.Execute(commands);
            Assert.AreEqual(2, groups.Count, "Not all groups were added!");
            _removeGroups.Add(((NamedObject)groups[0]).Id);
            _removeGroups.Add(((NamedObject)groups[1]).Id);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void RemoveGroupCmdlet(bool globalGroup)
        {
            var group = DoAddGroup(_testGroupName, globalGroup);
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(RemoveODSGroupCommand)),
                    "-Id",
                    group.Id.ToString()
                })
            };
            Shell.Execute(commands); //throws eception on error, no assert necessary
        }

        [Test]
        public void RemoveGroupCmdletViaPipelineId()
        {
            var group1 = DoAddGroup(_testGroupName, false);
            var group2 = DoAddGroup(_testGroupName2, false);
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    String.Format("{0},{1}", group1.Id, group2.Id),
                    "|",
                    CmdletName(typeof(RemoveODSGroupCommand))
                })
            };
            Shell.Execute(commands); //throws eception on error, no assert necessary
        }

        [Test]
        public void RemoveGroupCmdletViaPipelineObject()
        {
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    "$g = ",
                    CmdletName(typeof(AddODSGroupCommand)),
                    SingleQuote(_testGroupName),
                    ODSGroupCommandBase.GroupScope.Private.ToString()
                }),
                "$g | " + CmdletName(typeof(RemoveODSGroupCommand))
            };
            Shell.Execute(commands); //throws eception on error, no assert necessary
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetGroupCmdlet(bool globalGroup)
        {
            var group = DoAddGroup(_testGroupName, globalGroup);
            _removeGroups.Add(group.Id);
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSGroupCommand)),
                    "-Scope",
                    globalGroup ? ODSGroupCommandBase.GroupScope.Global.ToString()
                                : ODSGroupCommandBase.GroupScope.Private.ToString()
                    
                })
            };
            var groups = Shell.Execute(commands);
            Assert.Greater(groups.Count, 0);
            Assert.True(groups.Contains(group), "Added group wasn't retrieved!");
        }
    }
}

// ODSCmdlets - Cmdlets for Powershell and Pash for Open Data Space Management
// Copyright (C) GRAU DATA 2013-2014
//
// Author(s): Stefan Burnicki <stefan.burnicki@graudata.com>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenDataSpace.Commands.Requests;
using OpenDataSpace.Commands;
using OpenDataSpace.Commands.Objects;
using System.Management.Automation;

namespace Test
{
    [TestFixture]
    class GroupCmdletsTests : GroupTestBase
    {
        private const string _testGroupName = "__testGroup";
        private const string _testGroupName2 = "__testGroup2";

        [TearDown]
        public void RemoveAddedGroups()
        {
            DoRemoveAddedGroups();
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
            AutoRemoveGroup(groupData.Id);
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
            AutoRemoveGroup(((NamedObject)groups[0]).Id);
            AutoRemoveGroup(((NamedObject)groups[1]).Id);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void RemoveGroupCmdlet(bool globalGroup)
        {
            var response = DoAddGroup(_testGroupName, globalGroup, false);
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(RemoveODSGroupCommand)),
                    "-Id",
                    response.Data.Id.ToString()
                })
            };
            Shell.Execute(commands); //throws eception on error, no assert necessary
        }

        [Test]
        public void RemoveGroupCmdletViaPipelineId()
        {
            var group1 = DoAddGroup(_testGroupName, false, false).Data;
            var group2 = DoAddGroup(_testGroupName2, false, false).Data;
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
            var group = DoAddGroup(_testGroupName, globalGroup, true).Data;
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


        [TestCase(false, GroupCmdletsTests._testGroupName, true, true)] // name of first group, partial name of second one
        [TestCase(false, "up2", false, true)] // part of second group name only
        [TestCase(false, "Group", true, true)] // partial name
        [TestCase(true, GroupCmdletsTests._testGroupName, true, false)] // with "exact" the second isn't matched (see first case)
        [TestCase(true, GroupCmdletsTests._testGroupName2, false, true)]
        [TestCase(true, "up2", false, false)] // with "excat" this should return a group
        public void GetGroupCmdletQuery(bool exact, string query, bool expectFirst, bool expectSecond)
        {
            var firstGroup = DoAddGroup(_testGroupName, false, true).Data;
            var secondGroup = DoAddGroup(_testGroupName2, false, true).Data;
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSGroupCommand)),
                    "-Scope",
                    ODSGroupCommandBase.GroupScope.Private.ToString(),
                    "-Name",
                    SingleQuote(query),
                    exact ? "-Exact" : ""
                })
            };
            var groups = Shell.Execute(commands);
            Assert.AreEqual(expectFirst, groups.Contains(firstGroup));
            Assert.AreEqual(expectSecond, groups.Contains(secondGroup));
        }

    }
}

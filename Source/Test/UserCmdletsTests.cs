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

﻿using NUnit.Framework;
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
    class UserCmdletsTests : TestBase
    {
        // TODO: better cleanup for added users!
        private List<long> _addedUsers = new List<long>();

        // TODO: using an arbitrary user is a workaround, because we can't delete and
        // therefore not add a user with known properties
        private UserObject GetAbritraryUser()
        {
            var listRequest = UserRequestFactory.CreateQueryUserRequest(0, 1, "", "", "", "");
            var list = RequestHandler.ExecuteAndUnpack<List<UserObject>>(listRequest);
            return list[0];
        }

        private string ArbitrarySubstring(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            if (input.Length > 5)
            {
                return input.Substring(2, input.Length - 5);
            }
            if (input.Length > 2)
            {
                return input.Substring(1, input.Length - 2);
            }
            if (input.Length == 2)
            {
                return input.Substring(0, 1);
            }
            return input;
        }

        private UserObject AddRandomUser()
        {
            var data = GetRandomUserData();
            var request = UserRequestFactory.CreateAddUserRequest(data);
            var user = RequestHandler.ExecuteAndUnpack<UserObject>(request);
            _addedUsers.Add(user.Id);
            return user;
        }

        [TearDown]
        public void RemoveAddedUsers()
        {
            foreach (var curId in _addedUsers)
            {
                var request = UserRequestFactory.CreateDeleteUserRequest(curId);
                RequestHandler.ExecuteSuccessfully<DataspaceResponse>(request);
            }
            _addedUsers.Clear();
        }

        [Test]
        public void GetUserCmdletAll()
        {
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSUserCommand))                    
                })
            };
            var user = GetAbritraryUser();
            var results = Shell.Execute(commands);
            Assert.True(results.Contains(user));
        }

        [Test]
        public void GetUserCmdletById()
        {
            var user = GetAbritraryUser();
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSUserCommand)),
                    "-Id",
                    user.Id.ToString()
                })
            };
            var results = Shell.Execute(commands);
            Assert.AreEqual(results.Count, 1);
            Assert.True(user.EqualsCompletely(results[0] as UserObject));
        }

        [Test]
        public void GetUserCmdletExactName()
        {
            var user = GetAbritraryUser();
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSUserCommand)),
                    "-UserName",
                    SingleQuote(user.UserName),
                    "-Exact"
                })
            };
            var results = Shell.Execute(commands);
            Assert.AreEqual(1, results.Count);
            Assert.True(user.EqualsCompletely(results[0] as UserObject));
        }

        [Test]
        public void GetUserCmdletQuery()
        {
            // TODO: we need a different user and check that it's not in the results
            var user = GetAbritraryUser();
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSUserCommand)),
                    "-UserName",
                    SingleQuote(ArbitrarySubstring(user.UserName)),
                    "-FamilyName",
                    SingleQuote(ArbitrarySubstring(user.FamilyName)),
                    "-GivenName",
                    SingleQuote(ArbitrarySubstring(user.GivenName)),
                    "-EMail",
                    SingleQuote(ArbitrarySubstring(user.EMail))
                })
            };
            var results = Shell.Execute(commands);
            Assert.Greater(results.Count, 0);
            Assert.True(results.Contains(user));
        }

        //TODO: if user management is better, add tests for start, limit, other cases (e.g. pipelining) for add, set, remove

        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void AddUserCmdlet()
        {
            var addable = GetRandomUserData();
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(AddODSUserCommand)),
                    "-UserName",
                    SingleQuote(addable.UserName),
                    "-FamilyName",
                    SingleQuote(addable.FamilyName),
                    "-GivenName",
                    SingleQuote(addable.GivenName),
                    "-EMail",
                    SingleQuote(addable.EMail),
                    "-Role",
                    SingleQuote(addable.Role),
                    "-Language",
                    SingleQuote(addable.Language),
                    "-MaxSpace",
                    addable.MaxSpace.ToString()
                })
            };
            var results = Shell.Execute(commands);
            Assert.AreEqual(1, results.Count);
            var addedUser = results[0] as UserObject;
            Assert.NotNull(addedUser);
            Assert.Greater(addedUser.Id, 0);
            _addedUsers.Add(addedUser.Id);
            Assert.False(addedUser.Locked);
            Assert.True(addable.EqualsCompletely(addedUser));
        }

        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void SetUserCmdlet()
        {
            var user = AddRandomUser();
            var updates = GetRandomUserData();
            updates.UserName = user.UserName;
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(SetODSUserCommand)),
                    "-Id",
                    user.Id.ToString(),
                    "-FamilyName",
                    SingleQuote(updates.FamilyName),
                    "-GivenName",
                    SingleQuote(updates.GivenName),
                    "-EMail",
                    SingleQuote(updates.EMail),
                    "-Role",
                    SingleQuote(updates.Role),
                    "-Language",
                    SingleQuote(updates.Language),
                    "-MaxSpace",
                    updates.MaxSpace.ToString()
                })
            };
            var results = Shell.Execute(commands);
            Assert.AreEqual(1, results.Count);
            var updated = results[0] as UserObject;
            Assert.NotNull(updated);
            Assert.AreEqual(user.Id, updated.Id);
            Assert.True(updates.EqualsCompletely(updated));
        }

        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void RemoveUserCmdlet()
        {
            var user = AddRandomUser();
            string[] commands = new string[] {
                SimpleConnectCommand(DefaultLoginData),
                String.Join(" ", new string[] {
                    CmdletName(typeof(RemoveODSUserCommand)),
                    "-Id",
                    user.Id.ToString()
                }),
                String.Join(" ", new string[] {
                    CmdletName(typeof(GetODSUserCommand)),
                    "-Id",
                    user.Id.ToString()
                })
            };
            var results = Shell.Execute(commands);
            Assert.AreEqual(1, results.Count);
            var deleted = results[0] as UserObject;
            Assert.NotNull(deleted);
            user.Locked = true;
            Assert.True(deleted.EqualsCompletely(user));
        }
    }
}

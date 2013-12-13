// ODSCmdlets - Cmdlets for Powershell and Pash for Open Data Space Management
// Copyright (C) 2013  <name of author>
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
using System;
using NUnit.Framework;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace Test
{
    [TestFixture]
    public class ConnectionTests : TestBase
    {
        [Test]
        public void TestConnect()
        {
            LoginData login = DefaultLoginData;
            string command = String.Join(" ", new string[] {
                "Connect-ODS",
                "-Host",
                SingleQuote(login.Host),
                "-Username",
                SingleQuote(login.Username),
                "-Password",
                SingleQuote(login.Password)
            });
            Collection<object> results = Shell.Execute(command);
            Assert.IsNull(results);
            //make sure the ODS_SESSION object was created in the shell
            var session = Shell.GetVariableValue("ODS_SESSION");
            Assert.IsNotNull(session);
            //make sure it contains a non-empty SessionId => connecting worked
            //Assert.IsNotEmpty(session.SessionId);
        }

        [Test]
        public void TestConnectFail()
        {

        }
    }
}


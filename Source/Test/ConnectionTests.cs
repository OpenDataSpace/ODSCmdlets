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
using OpenDataSpace.Commands;

namespace Test
{
    [TestFixture]
    public class ConnectionTests : TestBase
    {
        private string SimpleConnectCommand(LoginData login)
        {
            return String.Join(" ", new string[] {
                CmdletName(typeof(ConnectODSCommand)),
                "-Host",
                SingleQuote(login.URL),
                "-Username",
                SingleQuote(login.UserName),
                "-Password",
                SingleQuote(login.Password)
            });
        }

        [Test]
        public void ConnectODSSimpleAuth()
        {
            Shell.Execute(SimpleConnectCommand(DefaultLoginData));
            //make sure the session information object was created in the shell
            var session = Shell.GetVariableValue(ODSCommandBase.SessionInfoVariableName) as SessionInformation;
            Assert.IsNotNull(session, "Session Information cannot be found in PS environment.");
            Assert.True(session.IsValid(), "Invalid Session Information");
        }

        [Test]
        public void ConnectODSCredAuth()
        {
            LoginData login = DefaultLoginData;
            string[] commands = 
            {
                // password to secure string
                String.Format("$securepw = ConvertTo-SecureString {0} -asplaintext -force",
                    SingleQuote(login.Password)),
                String.Format("$username = {0}", SingleQuote(login.UserName)),
                // PSCredential object
                "$cred = New-Object System.Management.Automation.PSCredential($username,$securepw)",
                // ods connect command
                String.Format("{0} -Host {1} -Credential $cred", 
                    CmdletName(typeof(ConnectODSCommand)), SingleQuote(login.URL))
            };
            Shell.Execute(commands);
            //make sure the session information object was created in the shell
            var session = Shell.GetVariableValue(ODSCommandBase.SessionInfoVariableName) as SessionInformation;
            Assert.IsNotNull(session, "Session Information cannot be found in PS environment.");
            Assert.True(session.IsValid(), "Invalid Session Information");
        }

        [Test]
        public void ConnectODSWrongAuth()
        {
            LoginData login = new LoginData(DefaultLoginData);
            login.Password = login.Password + "foobar"; //arbitrary pw modification
            try
            {
                Shell.Execute(SimpleConnectCommand(login));
                Assert.True(false, "Login seemed to work with wrong password!");
            }
            catch (CmdletInvocationException exception)
            {
                var realException = exception.InnerException as RequestFailedException;
                Assert.IsNotNull(realException, "Wrong exception thrown for failed login");
                Assert.True(realException.Message.Contains("Error Code: 2"),
                    String.Format("Wrong error for failed login: {0}", realException.Message));
            }
        }

        [Test]
        public void DisconnectODS()
        {
            string[] commands = {
                SimpleConnectCommand(DefaultLoginData),
                CmdletName(typeof(DisconnectODSCommand))
            };
            var res = Shell.Execute(commands);
            var sessionInfo = Shell.GetVariableValue(ODSCommandBase.SessionInfoVariableName);
            Assert.IsNull(sessionInfo, "Session info was not deleted");
            Assert.Greater(res.Count, 0, "No return value");
            Assert.AreEqual(true, res[0], "Logout unsuccessful");
        }

        [Test]
        public void DisconnectWorksAlways()
        {
            // disconnect without connecting doesn't result in an exception
            var res = Shell.Execute(CmdletName(typeof(DisconnectODSCommand)));
            Assert.Greater(res.Count, 0, "No return value");
            Assert.AreEqual(false, res[0]);
        }
    }
}


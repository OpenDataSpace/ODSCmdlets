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


using OpenDataSpace.Commands;
using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Security;

namespace Test
{
    public class LoginData
    {
        public string URL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginData(string host, string username, string password)
        {
            URL = host;
            UserName = username;
            Password = password;
        }

        public LoginData(LoginData login)
        {
            URL = login.URL;
            UserName = login.UserName;
            Password = login.Password;
        }
    }

    public class TestBase
    {
        private const string _defaultLoginDataFileName = "test_login.txt";
        private LoginData _defaultLoginData;
        private TestShellInterface _shell;
        private RequestHandler _requestHandler;
        private Random _random;

        public Random Random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }

        public LoginData DefaultLoginData
        {
            get
            {
                if (_defaultLoginData == null)
                {
                    Uri codebaseUri = new Uri(typeof(TestBase).Assembly.CodeBase);
                    string outDir = Path.GetDirectoryName(codebaseUri.LocalPath);
                    _defaultLoginData = ReadLoginData(
                        Path.Combine(outDir, _defaultLoginDataFileName));
                }
                return _defaultLoginData;
            }
        }

        public RequestHandler RequestHandler
        {
            get
            {
                if (_requestHandler == null)
                {
                    LoginData login = DefaultLoginData;
                    var requestHandler = new RequestHandler(login.UserName, login.Password, login.URL);
                    requestHandler.Login();
                    _requestHandler = requestHandler;
                }
                return _requestHandler;
            }
        }

        public TestShellInterface Shell
        {
            get
            {
                if (_shell == null)
                {
                    _shell = new TestShellInterface();
                }
                return _shell;
            }
        }

        public TestBase()
        {
            // Should avoid problems with SSL and tests systems without valid certificate
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

        // General helper stuff

        internal string SimpleConnectCommand(LoginData login)
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

        internal string CmdletName(Type type)
        {
            var attribute = System.Attribute.GetCustomAttribute(type, typeof(CmdletAttribute)) 
                as CmdletAttribute;
            return String.Format("{0}-{1}", attribute.VerbName, attribute.NounName);
        }

        internal string SingleQuote(string escapable)
        {
            return String.Format("'{0}'", escapable);
        }

        internal UpdatableUserObject GetRandomUserData()
        {
            var name = "testguy_" + Random.Next();
            return new UpdatableUserObject()
            {
                UserName = name,
                FamilyName = "family_" + Random.Next(),
                GivenName = "given_" + Random.Next(),
                EMail = name + "@testing.com",
                Language = "en",
                Role = "DataSpaceDefaultUser",
                MaxSpace = Random.Next(10000)
            };
        }

        // TODO: using an arbitrary user is a workaround, because we can't delete and
        // therefore not add a user with known properties
        internal UserObject GetAbritraryUser()
        {
            var listRequest = UserRequestFactory.CreateQueryUserRequest(0, 1, "", "", "", "");
            var list = RequestHandler.ExecuteAndUnpack<List<UserObject>>(listRequest);
            return list[0];
        }

        // Private stuff

        private LoginData ReadLoginData(string filename)
        {
            string[] data = File.ReadAllLines(filename);
            if (data.Length < 3)
            {
                throw new Exception(
                    String.Format("Login data in file '{0}' is incomplete",
                                  filename)
                );
            }
            return new LoginData(data[0], data[1], data[2]);
        }

    }
}


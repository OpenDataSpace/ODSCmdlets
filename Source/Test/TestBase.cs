using OpenDataSpace.Commands;
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

        internal RequestHandler RequestHandler
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


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
using System.Management.Automation;
using System.Security;

namespace OpenDataSpace.Commands
{
    public class ODSCommandBase : PSCmdlet
    {
        private RequestHandler _requestHandler;

        public const string SessionInfoVariableName = "_ODS_SESSION";

        private RequestHandler RequestHandler
        {
            get
            {
                if (_requestHandler == null)
                {
                    // No manual login, try to login with information in session state
                    var cInfo = SessionState.PSVariable.GetValue(SessionInfoVariableName, null) as SessionInformation;
                    if (cInfo == null || !cInfo.IsValid())
                    {
                        throw new ConnectionFailedException("No session information provided", "NoSessionInfo");
                    }
                    _requestHandler = new RequestHandler(cInfo.SessionId, cInfo.URL);
                }
                return _requestHandler;
            }
        }


        public ODSCommandBase()
        {
        }

        public void Connect(string username, string password, string url)
        {
            Connect(username, RequestHandler.ToSecureString(password), url);
        }         

        public void Connect(string username, SecureString password, string url)
        {
            _requestHandler = new RequestHandler(username, password, url);
            string sId = _requestHandler.Login();
            var cInfo = new SessionInformation(sId, url, username);
            SessionState.PSVariable.Set(SessionInfoVariableName, cInfo);
        }

        public void Disconnect()
        {
            SessionState.PSVariable.Remove(SessionInfoVariableName);
        }

    }
}


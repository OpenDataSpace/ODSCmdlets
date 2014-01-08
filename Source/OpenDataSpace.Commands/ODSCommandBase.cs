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

namespace OpenDataSpace.Commands
{
    public class ODSCommandBase : PSCmdlet
    {
        private const string _sessionIdVarName = "_ODS_SESSION";
        private RequestHandler _requestHandler;

        private RequestHandler RequestHandler
        {
            get
            {
                if (_requestHandler == null)
                {
                    // No manual login, try to login with information in session state
                    var cInfo = SessionState.PSVariable.GetValue(_sessionIdVarName, null) as ConnectionInformation;
                    if (cInfo == null || !cInfo.IsValid())
                    {
                        throw new InvalidOperationException("No connection information provided");
                    }
                    _requestHandler = new RequestHandler(cInfo.SessionId, cInfo.Hostname);
                }
                return _requestHandler;
            }
        }


        public ODSCommandBase()
        {
        }

        public bool Connect(string username, string password, string hostname)
        {
            _requestHandler = new RequestHandler(username, password, hostname);
            string sId = _requestHandler.Login();
            if (String.IsNullOrEmpty(sId))
            {
                return false; // Login failed
            }
            var cInfo = new ConnectionInformation(sId, hostname);
            SessionState.PSVariable.Set(_sessionIdVarName, cInfo);
            return true;
        }

    }
}


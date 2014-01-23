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

using System;
using System.Management.Automation;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommunications.Connect, ODSNouns.ODS, DefaultParameterSetName="CredAuth")]
    public class ConnectODSCommand : ODSCommandBase
    {
        [Alias("Host", "h")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "CredAuth")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "SimpleAuth")]
        public string URL { get; set; }

        [Alias("c")]
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "CredAuth")]
        public PSCredential Credential { get; set; }

        [Alias("User", "u")]
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "SimpleAuth")]
        public string UserName { get; set; }

        [Alias("p")]
        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "SimpleAuth")]
        public string Password { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                if (Credential != null) //cred auth
                {
                    Connect(NormalizeUserName(Credential.UserName), Credential.Password, URL);
                }
                else // simple auth
                {
                    Connect(UserName, Password, URL);
                }
            }
            catch (ReportableException e)
            {
                ThrowTerminatingError(e.ErrorRecord);
            }
        }

        private string NormalizeUserName(string username)
        {
            if (username.StartsWith(@"\"))
            {
                return username.Remove(0, 1);
            }
            return username;
        }
    }
}


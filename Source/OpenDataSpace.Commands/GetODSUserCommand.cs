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

using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommon.Get, ODSNouns.User, DefaultParameterSetName = "Query")]
    public class GetODSUserCommand : ODSCommandBase
    {
        private int _defaultLimit = 25;

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, 
                   ValueFromPipeline = true, ParameterSetName = "ById")]
        public long[] Id { get; set; }

        [Alias("Name")]
        [Parameter(Position = 0, Mandatory = false, ValueFromPipeline = true, 
                   ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public string UserName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public string FamilyName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public string GivenName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public string EMail { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "Query")]
        public SwitchParameter Exact { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "Query")]
        public int Start { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "Query")]
        public int? Limit { get; set; }

        protected override void ProcessRecord()
        {
            if (Id != null)
            {
                foreach (long curId in Id)
                {
                    try
                    {
                        var request = UserRequestFactory.CreateGetUserRequest(curId);
                        WriteObject(RequestHandler.ExecuteAndUnpack<UserObject>(request));
                    }
                    catch (ReportableException e)
                    {
                        WriteError(e.ErrorRecord);
                    }
                }
                return;
            }

            // otherwise we want to query users!
            try
            {
                int limit = Limit ?? _defaultLimit;
                var request = UserRequestFactory.CreateQueryUserRequest(Start, Limit ?? _defaultLimit,
                    UserName, FamilyName, GivenName, EMail);
                var users = RequestHandler.ExecuteAndUnpack<List<UserObject>>(request);
                foreach (var curUser in users)
                {
                    if (!Exact.IsPresent || CheckExactMatch(curUser))
                    {
                        WriteObject(curUser);
                    }
                }
            }
            catch (ReportableException e)
            {
                WriteError(e.ErrorRecord);
            }
        }

        private bool CheckExactMatch(UserObject user)
        {
            bool equal = true;
            if (!String.IsNullOrEmpty(UserName))
            {
                equal = equal && UserName.Equals(user.UserName);
            }
            if (!String.IsNullOrEmpty(FamilyName))
            {
                equal = equal && FamilyName.Equals(user.FamilyName);
            }
            if (!String.IsNullOrEmpty(GivenName))
            {
                equal = equal && GivenName.Equals(user.GivenName);
            }
            if (!String.IsNullOrEmpty(EMail))
            {
                equal = equal && EMail.Equals(user.EMail);
            }
            return equal;
        }
    }
}


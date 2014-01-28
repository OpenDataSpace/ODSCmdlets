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

﻿using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommon.Get, ODSNouns.GroupMember, DefaultParameterSetName = "GetAll")]
    public class GetODSGroupMemberCommand : ODSGroupCommandBase
    {

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string GroupName { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public GroupScope Scope { get; set; }

        [Parameter(Mandatory = false)]
        public int Start { get; set; }

        [Parameter(Mandatory = false)]
        public int? Limit { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var request = GroupMemberRequestFactory.CreateGetGroupMembersRequest(Start, Limit ?? 100,
                    GroupName, Scope.Equals(GroupScope.Global));
                var members = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
                foreach (var member in members)
                {
                    WriteObject(member);
                }
            }
            catch (ReportableException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
    }
}

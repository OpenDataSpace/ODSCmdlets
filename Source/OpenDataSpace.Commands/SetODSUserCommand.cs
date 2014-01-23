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
    [Cmdlet(VerbsCommon.Set, ODSNouns.User, SupportsShouldProcess = true)]
    public class SetODSUserCommand : ODSGroupCommandBase
    {

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public long Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string FamilyName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string GivenName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string EMail { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Language { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Role { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public long MaxSpace { get; set; }

        protected override void ProcessRecord()
        {
            var updates = new UpdatableUserObject()
            {
                UserName = "",
                FamilyName = FamilyName,
                GivenName = GivenName,
                EMail = EMail,
                Language = Language,
                Role = Role,
                MaxSpace = MaxSpace
            };
            try
            {
                var request = UserRequestFactory.CreateEditUserRequest(Id, updates);
                if (ShouldProcess("Edit user"))
                {
                    WriteObject(RequestHandler.ExecuteAndUnpack<UserObject>(request));
                }
            }
            catch (ReportableException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
    }
}

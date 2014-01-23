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
    [Cmdlet(VerbsCommon.Add, ODSNouns.Group, SupportsShouldProcess = true)]
    public class AddODSGroupCommand : ODSGroupCommandBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string[] Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public GroupScope Scope { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var curName in Name)
            {
                try
                {
                    var request = GroupRequestFactory.CreateAddGroupRequest(curName, Scope.Equals(GroupScope.Global));
                    // TODO: check use of ShouldProcess
                    if (ShouldProcess(curName))
                    {
                        var data = RequestHandler.ExecuteAndUnpack<NamedObject>(request);
                        WriteObject(data);
                    }
                }
                catch (ReportableException e)
                {
                    WriteError(e.ErrorRecord);
                }
            }

        }
    }
}

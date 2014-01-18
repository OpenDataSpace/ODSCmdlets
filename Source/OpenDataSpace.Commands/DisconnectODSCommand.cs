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
    [Cmdlet(VerbsCommunications.Disconnect, ODSNouns.ODS)]
    public class DisconnectODSCommand : ODSCommandBase
    {
        protected override void BeginProcessing()
        {
            WriteObject(Disconnect());
        }
    }
}


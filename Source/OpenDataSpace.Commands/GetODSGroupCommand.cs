using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommon.Get, ODSNouns.Group)]
    public class GetODSGroupCommand : ODSGroupCommandBase
    {
        //TODO: support getting specific groups (by name), with wildcard
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        public GroupScope Scope { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                var request = GroupRequestFactory.CreateGetGroupsRequest(Scope.Equals(GroupScope.Global));
                var groups = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
                foreach (var group in groups)
                {
                    WriteObject(group);
                }
            }
            catch (ReportableException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
    }
}

using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommon.Add, ODSNouns.GroupMember, SupportsShouldProcess = true)]
    public class AddODSGroupMemberCommand : ODSGroupCommandBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string GroupName { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public GroupScope Scope { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public long[] Id { get; set; }

        protected override void ProcessRecord()
        {
            foreach (var curId in Id)
            {
                try
                {
                    var request = GroupMemberRequestFactory.CreateAddGroupMemberRequest(GroupName, Scope.Equals(GroupScope.Global), curId);
                    if (ShouldProcess(curId.ToString()))
                    {
                        RequestHandler.ExecuteSuccessfully(request);
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

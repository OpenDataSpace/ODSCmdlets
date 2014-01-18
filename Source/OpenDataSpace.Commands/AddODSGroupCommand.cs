using OpenDataSpace.Commands.Objects;
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
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true,  ValueFromPipelineByPropertyName = true)]
        public string[] Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public GroupScope Scope { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                foreach (var curName in Name)
                {
                    var request = GroupRequestFactory.CreateAddGroupRequest(curName, Scope.Equals(GroupScope.Global));
                    // TODO: check use of ShouldProcess
                    if (ShouldProcess(String.Format("Add Group '{0}' to DataSpace", curName)))
                    {
                        var data = RequestHandler.ExecuteAndUnpack<NamedObject>(request);
                        WriteObject(data);
                    }
                }
            }
            catch (ReportableException e)
            {
                WriteError(e.ErrorRecord);
            }
        }
    }
}

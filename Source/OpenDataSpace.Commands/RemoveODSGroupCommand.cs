using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{

    [Cmdlet(VerbsCommon.Remove, ODSNouns.Group, SupportsShouldProcess = true)]
    public class RemoveODSGroupCommand : ODSGroupCommandBase
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public long[] Id { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                foreach (var curId in Id)
                {
                    var request = GroupRequestFactory.CreateDeleteGroupRequest(curId);
                    // TODO: check use of ShouldProcess regarding the passed message
                    if (ShouldProcess(String.Format("Remove Group {0} from DataSpace", curId)))
                    {
                        RequestHandler.SuccessfullyExecute<DataspaceResponse>(request);
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

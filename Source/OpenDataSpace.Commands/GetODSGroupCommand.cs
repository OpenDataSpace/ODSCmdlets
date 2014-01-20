using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    [Cmdlet(VerbsCommon.Get, ODSNouns.Group, DefaultParameterSetName = "GetAll")]
    public class GetODSGroupCommand : ODSGroupCommandBase
    {

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "GetAll")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public GroupScope Scope { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Query")]
        public string[] Name { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "Query")]
        public SwitchParameter Exact { get; set; }

        protected override void ProcessRecord()
        {
            // no name provided? empty string looks for everything
            if (Name == null || Name.Length == 0)
            {
                Name = new string[] { "" };
            }
            foreach (var curName in Name)
            {
                try
                {
                    var request = GroupRequestFactory.CreateGetGroupsRequest(curName, Scope.Equals(GroupScope.Global));
                    var groups = RequestHandler.ExecuteAndUnpack<List<NamedObject>>(request);
                    foreach (var group in groups)
                    {
                        if (!Exact.IsPresent || group.Name.Equals(curName))
                        {
                            WriteObject(group);
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
}

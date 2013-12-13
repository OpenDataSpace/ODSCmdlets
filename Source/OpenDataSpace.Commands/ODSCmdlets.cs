using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    [RunInstaller(true)]
    public class ODSCmdlets : PSSnapIn
    {

        public override string Description
        {
            get { return "Cmdlets to manage an OpenDataSpace"; }
        }

        public override string Name
        {
            get { return "ODSCmdlets"; }
        }

        public override string Vendor
        {
            get { return "OpenDataSpace"; }
        }
    }
}

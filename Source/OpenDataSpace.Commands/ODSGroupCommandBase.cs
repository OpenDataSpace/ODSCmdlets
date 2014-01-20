using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands
{
    public class ODSGroupCommandBase : ODSCommandBase
    {
        public enum GroupScope
        {
            Global,
            Private
        };
    }
}

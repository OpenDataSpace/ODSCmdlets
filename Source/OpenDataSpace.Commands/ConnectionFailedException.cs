using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    class ConnectionFailedException : ReportableException
    {
        public ConnectionFailedException(string message, string errorId)
            : this(message, errorId, null)
        {
        }

        public ConnectionFailedException(string message, string errorId, Exception innerException)
            : base(message, innerException, ErrorCategory.OpenError, errorId, null)
        {
        }
    }
}

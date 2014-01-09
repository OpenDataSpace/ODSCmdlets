using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace OpenDataSpace.Commands
{
    public class ReportableException : Exception
    {
        protected ErrorCategory _category;
        protected string _errorId;
        protected object _targetObject;

        public ErrorRecord ErrorRecord
        {
            get
            {
                return new ErrorRecord(this, _errorId, _category, _targetObject);
            }
        }

        public ReportableException(string message, Exception innerException, ErrorCategory category,
                                   string errorId, object targetObject)
            : base(message, innerException)
        {
            _category = category;
            _errorId = errorId;
            _targetObject = targetObject;
        }
    }
}

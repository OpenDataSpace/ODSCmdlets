using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class LoginResponse : DataspaceResponse
    {
        public string sessionId { get; set; }
    }
}

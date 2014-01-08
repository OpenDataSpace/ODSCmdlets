using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class LoginResponse
    {
        public string sessionId { get; set; }
        public bool success { get; set; }
        public string errorClass { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
    }
}

using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    class LoginResponse : DataspaceResponse
    {
        [DeserializeAs(Name = "sessionId", Attribute = true)]
        public string SessionId { get; set; }
    }
}

using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    public class DataspaceResponse
    {
        [DeserializeAs(Name="success", Attribute=true)]
        public bool Success { get; set; }

        [DeserializeAs(Name = "errorClass", Attribute = true)]
        public string ErrorClass { get; set; }

        [DeserializeAs(Name = "errorCode", Attribute = true)]
        public string ErrorCode { get; set; }

        [DeserializeAs(Name = "message", Attribute = true)]
        public string Message { get; set; }
    }
}

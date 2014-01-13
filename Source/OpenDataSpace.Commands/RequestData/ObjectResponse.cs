using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.RequestData
{
    class ObjectResponse : DataspaceResponse
    {
        [DeserializeAs(Name="data", Attribute=true)]
        public object Data { get; set; }
    }
}

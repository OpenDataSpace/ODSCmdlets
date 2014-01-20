using OpenDataSpace.Commands.Objects;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    class ObjectResponse<T> : DataspaceResponse where T : new()
    {
        [DeserializeAs(Name = "data", Attribute = true)]
        public T Data { get; set; }
    }
}

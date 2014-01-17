using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Objects
{
    class NamedObject : IEquatable<NamedObject>
    {
        [DeserializeAs(Name = "id", Attribute = true)]
        public long Id { get; set; }

        [DeserializeAs(Name = "name", Attribute = true)]
        public string Name { get; set; }

        public bool Equals(object other)
        {
            if (other is NamedObject)
            {
                return Equals((NamedObject)other);
            }
            return false;
        }

        public bool Equals(NamedObject other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

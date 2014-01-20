using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Objects
{
    public class UpdatableUserObject
    {
        public string UserName { get; set; }

        public string FamilyName { get; set; }

        public string GivenName { get; set; }

        [DeserializeAs(Name = "emailaddresses", Attribute = true)]
        public string EMail { get; set; }

        public string Language { get; set; }

        public string Role { get; set; }

        public long MaxSpace { get; set; }

        internal bool EqualsCompletely(UpdatableUserObject other)
        {
            return (
                String.Equals(UserName, other.UserName) &&
                String.Equals(FamilyName, other.FamilyName) &&
                String.Equals(GivenName, other.GivenName) &&
                String.Equals(EMail, other.EMail) &&
                String.Equals(Language, other.Language) &&
                String.Equals(Role, other.Role) &&
                MaxSpace.Equals(other.MaxSpace)
            );
        }
    }
}

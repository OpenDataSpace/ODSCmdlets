using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Objects
{
    public class UserObject : UpdatableUserObject
    {
        public long Id { get; set; }

        public bool Locked { get; set; }

        public string UsedSpacePercent { get; set; }

        public override bool Equals(object other)
        {
            if (other is UserObject)
            {
                return Equals((UserObject)other);
            }
            return false;
        }

        public bool Equals(UserObject other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        internal bool EqualsCompletely(UserObject other)
        {
            if (other == null)
            {
                return false;
            }
            return (
                base.EqualsCompletely(other) &&
                Id.Equals(other.Id) &&
                Locked.Equals(other.Locked) &&
                String.Equals(UsedSpacePercent, other.UsedSpacePercent)
            );
        }
    }
}

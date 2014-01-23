// ODSCmdlets - Cmdlets for Powershell and Pash for Open Data Space Management
// Copyright (C) GRAU DATA 2013-2014
//
// Author(s): Stefan Burnicki <stefan.burnicki@graudata.com>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

﻿using RestSharp.Deserializers;
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

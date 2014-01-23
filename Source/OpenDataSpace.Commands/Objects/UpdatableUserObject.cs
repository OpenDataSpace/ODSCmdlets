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

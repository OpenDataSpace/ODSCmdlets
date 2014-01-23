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

﻿using OpenDataSpace.Commands.Objects;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public static class UserRequestFactory
    {
        private static string[] _jsonUserObjectProperties;
        private static Dictionary<string, string> _jsonUpdatePropertyMap;
        private const string _defaultHandler = "dataspaceuser";
        private const string _defaultProvider = "dataspaceuser";
        private const string _defaultSource = "DataSpaceUser";
        private static Dictionary<string, string> _defaultSort = new Dictionary<string, string>() {
            { "property", "name"},
            { "direction", "ASC" }
        };
        private static Dictionary<string, string> _lockedFilter = new Dictionary<string, string>() {
            { "property", "locked"},
            { "value", "false" }
        };

        private static string[] JsonUserObjectProperties
        {
            get
            {
                if (_jsonUserObjectProperties == null)
                {
                    _jsonUserObjectProperties = JsonPropertyNames(typeof(UserObject)).Values.ToArray();
                }
                return _jsonUserObjectProperties;
            }
        }

        private static Dictionary<string, string> JsonUpdatePropertyMap
        {
            get
            {
                if (_jsonUpdatePropertyMap == null)
                {
                    _jsonUpdatePropertyMap = JsonPropertyNames(typeof(UpdatableUserObject));
                }
                return _jsonUpdatePropertyMap;
            }
        }

        private static Dictionary<string, string> JsonPropertyNames(Type type)
        {
            var names = new Dictionary<string, string>();
            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(DeserializeAsAttribute), true);
                var jsonName = property.Name.ToLower();
                if (attributes.Length > 0)
                {
                    // cast safe because we only looked for this type
                    var deserializeAttr = (DeserializeAsAttribute) attributes[0];
                    jsonName = deserializeAttr.Name;
                }
                names[property.Name] = jsonName;
            }
            return names;
        }

        private static ObjectRequest CreateUserObjectRequest(string requestName, Method method)
        {
            var request = new ObjectRequest(requestName, method);
            // TODO: think about making this dynamic by checking the DeserializeAs attributes from UserObject
            request.AskForProperties(JsonUserObjectProperties);
            request.AddParameter("handler", _defaultHandler);
            request.AddParameter("sort", _defaultSort);
            request.AddParameter("filter", _lockedFilter);
            return request;
        }

        private static Dictionary<string, object> BuildJsonUpdateDict(UpdatableUserObject userObject)
        {
            var updates = new Dictionary<string, object>();
            var type = userObject.GetType();

            foreach (var pair in JsonUpdatePropertyMap)
            {
                object value = type.GetProperty(pair.Key).GetValue(userObject, null);
                if (value != null)
                {
                    updates[pair.Value] = value;
                }
            }
            return updates;
        }

        public static ObjectRequest CreateAddUserRequest(UpdatableUserObject data)
        {
            var request = CreateUserObjectRequest("Add User", Method.POST);
            var updates = BuildJsonUpdateDict(data);
            request.Data = updates;
            return request;
        }

        public static ObjectRequest CreateEditUserRequest(long id, UpdatableUserObject data)
        {
            var request = CreateUserObjectRequest("Edit User", Method.PUT);
            var updates = BuildJsonUpdateDict(data);
            updates["id"] = id;
            request.Data = updates;
            request.ObjectId = id;
            return request;
        }

        public static ObjectRequest CreateDeleteUserRequest(long id)
        {
            var request = new ObjectRequest("Delete User", Method.DELETE);
            request.ObjectId = id;
            request.AddParameter("handler", _defaultHandler);
            return request;
        }

        public static ObjectRequest CreateGetUserRequest(long id)
        {
            var request = CreateUserObjectRequest("Get User", Method.GET);
            request.AddParameter("id", id);
            request.ObjectId = id;
            return request;
        }

        public static ObjectRequest CreateQueryUserRequest(int start, int limit, string username,
                                                        string familyname, string givenname, string mail)
        {
            var request = CreateUserObjectRequest("Query User", Method.GET);
            request.AddParameter("provider", _defaultProvider);
            request.AddParameter("source", _defaultSource);
            request.AddParameter("query", username ?? "");
            request.AddParameter("start", start);
            request.AddParameter("limit", limit);
            var parameters = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(familyname))
            {
                parameters["familyname"] = familyname;
            }
            if (!String.IsNullOrEmpty(givenname))
            {
                parameters["givenname"] = givenname;
            }
            if (!String.IsNullOrEmpty(mail))
            {
                parameters["mail"] = mail;
            }
            if (parameters.Count > 0)
            {
                request.AddParameter("parameters", parameters);
            }
            return request;
        }
    }
}

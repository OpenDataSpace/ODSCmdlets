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

﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands.Requests
{
    public static class GroupRequestFactory
    {
        private const string _handler = "dataspacegroup";
        private const string _provider = "dataspacedirectoryobjects";

        private static ObjectRequest CreateNamedObjectRequest(string requestName, Method method)
        {
            var request = new ObjectRequest(requestName, method);
            request.AskForProperties("name", "id");
            return request;
        }

        public static ObjectRequest CreateGetGroupsRequest(bool globalGroups)
        {
            return CreateGetGroupsRequest("", globalGroups);
        }

        public static ObjectRequest CreateGetGroupsRequest(string query, bool globalGroups)
        {
            var request = CreateNamedObjectRequest("Get Groups", Method.GET);
            request.AddParameter("provider", _provider);
            request.AddParameter("query", query);
            request.AddParameter("sort", new
            {
                property = "name",
                direction = "ASC"
            });
            request.AddParameter("parameters", new
            {
                resultset = globalGroups ? "globalgroups" : "privategroups"
            });
            return request;
        }

        public static ObjectRequest CreateAddGroupRequest(string groupName, bool globalGroup)
        {
            var request = CreateNamedObjectRequest("Add Group", Method.POST);
            request.Data = new
            {
                name = groupName,
                globalgroup = globalGroup
            };
            request.AddParameter("handler", _handler);
            return request;
        }

        public static ObjectRequest CreateDeleteGroupRequest(long id)
        {
            var request = new ObjectRequest("Delete Group", Method.DELETE);
            request.AddParameter("handler", _handler);
            request.Data = new
            {
                id = id
            };
            request.ObjectId = id;
            return request;
        }

    }
}

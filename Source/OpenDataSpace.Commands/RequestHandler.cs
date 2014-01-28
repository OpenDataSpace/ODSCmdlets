// ODSCmdlets - Cmdlets for Powershell and Pash for Open Data Space Management
// Copyright (C) GRAU DATA 2013-2014
//
// Author(s): Kevin Goedecke <kevin.goedecke@graudata.com>
//            Stefan Burnicki <stefan.burnicki@graudata.com>
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

﻿using OpenDataSpace.Commands.Requests;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace OpenDataSpace.Commands
{
    public class RequestHandler
    {
        private readonly IRestClient _client;
        private readonly string _username;
        private readonly SecureString _password;
        private string _sessionId;
        private DataspaceRequest.AuthMethod _authMethod;

        public RequestHandler(string username, string password, string url)
            : this(username, Utility.StringToSecureString(password), url, DataspaceRequest.AuthMethod.SessionId)
        {
        }

        public RequestHandler(string username, string password, string url, DataspaceRequest.AuthMethod authMethod)
            : this(username, Utility.StringToSecureString(password), url, authMethod)
        {
        }

        public RequestHandler(string username, SecureString password, string url)
            : this(username, password, url, DataspaceRequest.AuthMethod.SessionId)
        {
        }

        public RequestHandler(string username, SecureString password, string url, DataspaceRequest.AuthMethod authMethod)
        {
            _client = new RestClient(ValidateURL(url));
            _username = username;
            _password = password;
            _authMethod = authMethod;
        }

        public RequestHandler(string sessionId, string url)
            : this(sessionId, url, DataspaceRequest.AuthMethod.SessionId)
        {
        }

        public RequestHandler(string sessionId, string url, DataspaceRequest.AuthMethod authMethod)
        {
            _client = new RestClient(ValidateURL(url));
            _sessionId = sessionId;
            _authMethod = authMethod;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            IRestResponse<T> response;

            // Explicitly check whether to execute as GET- or POST-like request
            // RestSharp treats DELETE by default as a GET-like request, however we
            // need it as a POST-like request
            var method = Enum.GetName(typeof(Method), request.Method);
            switch (request.Method)
            {
                case Method.POST:
                case Method.PUT:
                case Method.PATCH:
                case Method.DELETE:
                    response = _client.ExecuteAsPost<T>(request, method);
                    break;
                default:
                    response = _client.ExecuteAsGet<T>(request, method);
                    break;
            }

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                string message = String.Format("Error retrieving response: {0}. {1}",
                    response.ResponseStatus.ToString(), response.ErrorMessage);
                throw new RequestFailedException(message, "RequestFailed", response.ErrorException);
            }
            // if we identify with a cookie, update the sessionId
            if (_authMethod == DataspaceRequest.AuthMethod.Cookie)
            {
                foreach (var cookie in response.Cookies)
                {
                    if (cookie.Name == DataspaceRequest.SessionIdCookieName)
                    {
                        _sessionId = cookie.Value;
                        break;
                    }
                }
            }
            return response.Data;
        }

        public bool Execute(DataspaceRequest request)
        {
            var response = Execute<DataspaceResponse>(request);
            return response.Success;
        }

        public T Execute<T>(DataspaceRequest request) where T : DataspaceResponse, new()
        {
            var response = Execute<T>(request.CreateRestRequest(_authMethod, _sessionId));
            if (response == null)
            {
                string message = String.Format("{0} request failed. Maybe the URL is incorrect?",
                    request.RequestName);
                throw new RequestFailedException(message,
                    "ResponseDataIsNull");
            }
            return response;
        }

        public void ExecuteSuccessfully(DataspaceRequest request)
        {
            ExecuteSuccessfully<DataspaceResponse>(request);
        }

        public T ExecuteSuccessfully<T>(DataspaceRequest request) where T : DataspaceResponse, new()
        {
            var response = Execute<T>(request);
            if (!response.Success)
            {
                string message = String.Format("{0} failed: {1}. Error Code: {2}",
                    request.RequestName, response.Message, response.ErrorCode);
                throw new RequestFailedException(message, "DataspaceRequestNotSuccessfull");
            }
            return response;
        }

        public T ExecuteAndUnpack<T>(ObjectRequest request) where T : new()
        {
            var response = ExecuteSuccessfully<ObjectResponse<T>>(request);
            return response.Data;
        }

        public string Login()
        {
            if (String.IsNullOrEmpty(_username) || _password == null)
            {
                // TODO: throw exception
            }
            var request = new LoginRequest(_username, _password);
            var response = ExecuteSuccessfully<LoginResponse>(request);
            if (_authMethod == DataspaceRequest.AuthMethod.SessionId)
            {
                _sessionId = response.SessionId;
            }
            // otherwise it's Cookie auth and set automatically!
            return _sessionId;
        }

        public bool Logout()
        {
            var request = new LogoutRequest();
            var response = Execute<DataspaceResponse>(request);
            return response.Success;
        }

        private string ValidateURL(string url)
        {
            // TODO: check first if protocol is already part of url
            return string.Format("https://{0}", url);
        }
    }
}

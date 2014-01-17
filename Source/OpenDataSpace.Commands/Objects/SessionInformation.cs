using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands
{
    class SessionInformation
    {
        public string URL { get; set; }
        public string SessionId { get; set; }
        public string UserName { get; set; }

        public SessionInformation()
        {
        }

        public SessionInformation(string sessionId, string url, string username)
        {
            URL = url;
            SessionId = sessionId;
            UserName = username;
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(URL) &&
                   !String.IsNullOrEmpty(SessionId) &&
                   !String.IsNullOrEmpty(UserName);
        }
    }
}

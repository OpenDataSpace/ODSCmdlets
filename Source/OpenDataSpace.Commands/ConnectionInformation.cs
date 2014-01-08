using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands
{
    class ConnectionInformation
    {
        public string Hostname { get; set; }
        public string SessionId { get; set; }

        public ConnectionInformation()
        {
        }

        public ConnectionInformation(string sessionId, string hostname)
        {
            Hostname = hostname;
            SessionId = sessionId;
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Hostname) && !String.IsNullOrEmpty(SessionId);
        }
    }
}

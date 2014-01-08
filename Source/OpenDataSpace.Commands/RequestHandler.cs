using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDataSpace.Commands
{
    class RequestHandler
    {
        private readonly IRestClient client;

        const string baseUrl = "";

        readonly string username;
        readonly string password;
        readonly string sessionId;

        public RequestHandler(string username, string password, string hostname)
        {
            client = new RestClient(baseUrl);
            this.username = username;
            this.password = password;
        }

        public RequestHandler(string sessionId, string hostname)
        {
            client = new RestClient(baseUrl);
            this.sessionId = sessionId;
        }

        public RequestHandler(IRestClient restClient)
        {   
            this.client = restClient;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = baseUrl;
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<T>(request);

                if (response.ErrorException != null)
                {
                    const string message = "Error retrieving response.  Check inner details for more info.";
                    var twilioException = new ApplicationException(message, response.ErrorException);
                    throw twilioException;
                }
                return response.Data;
        }
    }
}

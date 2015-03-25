using System;
using System.Collections.Generic;
using RestSharp;

namespace Toxiproxy.Net
{
    public class ToxicClient : ToxiproxyBaseClient
    {
        private readonly IRestClient _client;
        public ToxicClient(IRestClient client)
        {
            this._client = client;
        }

        public IDictionary<string, Proxy> All()
        {
            var request = GetDefaultRequestWithErrorParsingBehaviour("/toxics", Method.GET);
            var response = this._client.Execute<Dictionary<string, Proxy>>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Data == null)
            {
                return new Dictionary<string, Proxy>();
            } 
            
            return response.Data;
        }
    }
}
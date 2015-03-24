using System;
using System.Collections.Generic;
using RestSharp;

namespace Toxiproxy.Net
{
    public class Toxics
    {
        private readonly RestClient _client;
        public Toxics(RestClient client)
        {
            this._client = client;
        }

        public IDictionary<string, Proxy> All()
        {
            var request = new RestRequest("/toxics", Method.GET);
            var response = this._client.Execute<Dictionary<string, Proxy>>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Data == null)
            {
                return new Dictionary<string, Proxy>();
            } 
             
            Console.WriteLine(response.Content);
            return response.Data;
        }
    }
}
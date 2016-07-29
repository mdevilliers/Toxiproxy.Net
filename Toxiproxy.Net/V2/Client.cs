using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toxiproxy.Net.V2
{
    public class Client : ToxiproxyBaseClient
    {
        // TODO - Duplicated with the other Client class
        private readonly IRestClient _client;

        // TODO - Duplicated with the other Client class
        public Client(IRestClient client)
        {
            _client = client;
        }

        public void Reset()
        {
            // {/reset} - POST
            throw new NotImplementedException();
        }

        // TODO - Duplicated with the other Client class
        public IDictionary<string, Proxy> All()
        {
            // {/proxies} - GET

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies", Method.GET);
            var response = _client.Execute<Dictionary<string, Proxy>>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            if (response.Data == null)
            {
                return new Dictionary<string, Proxy>();
            }

            foreach (var proxy in response.Data.Values)
            {
                proxy.Client = this;
            }

            return response.Data;
        }

        // TODO - Duplicated with the other Client class
        public Proxy Add(Proxy proxy)
        {
            // {/proxies} - POST

            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(proxy);

            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            response.Data.Client = this;
            return response.Data;
        }

        // TODO - Duplicated with the other Client class
        public Proxy FindProxy(string proxyName)
        {
            // "/proxies/{proxy}" - GET

            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.GET);
            request.AddUrlSegment("name", proxyName);

            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            response.Data.Client = this;
            return response.Data;
        }

        // TODO - Duplicated with the other Client class
        public Proxy Update(Proxy proxy)
        {
            // "/proxies/{proxy}" - POST
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddUrlSegment("name", proxy.Name);
            request.AddJsonBody(proxy);

            var response = _client.Execute<Proxy>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            response.Data.Client = this;

            return response.Data;
        }

        // TODO - Duplicated with the other Client class
        public void Delete(string proxyName)
        {
            // "/proxies/{proxy}" - DELETE
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            var request = GetDefaultRequestWithErrorParsingBehaviour("/proxies/{name}", Method.DELETE);
            request.AddUrlSegment("name", proxyName);

            var response = _client.Execute(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
        }

        public IEnumerable<ToxicCollection> Toxicities(string proxyName)
        {
            // "/proxies/{proxy}/toxics" - GET
            throw new NotImplementedException();
        }

        public ToxicBase Toxics(string proxyName)
        {
            // "/proxies/{proxy}/toxics" - GET
            throw new NotImplementedException();
        }

        public void AddToxic(string proxyName)
        {
            // "/proxies/{proxy}/toxics" - POST
            throw new NotImplementedException();
        }

        public ToxicBase GetToxic(string proxyName, string toxicName)
        {
            // "/proxies/{proxy}/toxics/{toxic}" - GET
            throw new NotImplementedException();
        }

        public void UpdateToxic(string proxyName, string toxicName)
        {
            // "/proxies/{proxy}/toxics/{toxic}" - POST
            throw new NotImplementedException();
        }

        public void DeleteToxic(string proxyName, string toxicName)
        {
            // "/proxies/{proxy}/toxics/{toxic}" - DELETE
            throw new NotImplementedException();
        }
    }
}

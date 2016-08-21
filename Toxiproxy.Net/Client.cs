using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Toxiproxy.Net.Toxics;

namespace Toxiproxy.Net
{
    /// <summary>
    /// The client to send the requests to the ToxiProxy server
    /// </summary>
    public class Client
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonConverter[] _deserializeConverter = new[] { new JsonToxicsConverter() };

        public Client(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IDictionary<string, Proxy> All()
        {
            using (var httpClient = _clientFactory.Create())
            {
                var response = httpClient.GetAsync("/proxies").Result;
                CheckIsSuccessStatusCode(response);

                var result = response.Content.ReadAsAsync<Dictionary<string, Proxy>>().Result;

                foreach (var proxy in result.Values)
                {
                    proxy.Client = this;
                }

                return result;
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            using (var httpClient = _clientFactory.Create())
            {
                var response = httpClient.PostAsync("/reset", null).Result;

                CheckIsSuccessStatusCode(response);
            }
        }

        #region Proxy
        /// <summary>
        /// Adds the specified proxy to the ToxiProxy server.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">proxy</exception>
        public Proxy Add(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            using (var httpClient = _clientFactory.Create())
            {
                var response = httpClient.PostAsJsonAsync("/proxies", proxy).Result;
                CheckIsSuccessStatusCode(response);

                var newProxy = response.Content.ReadAsAsync<Proxy>().Result;

                newProxy.Client = this;

                return newProxy;
            }
        }

        /// <summary>
        /// Updates the specified proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">proxy</exception>
        public Proxy Update(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            using (var httpClient = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}", proxy.Name);
                var response = httpClient.PostAsJsonAsync(url, proxy).Result;

                CheckIsSuccessStatusCode(response);

                var newProxy = response.Content.ReadAsAsync<Proxy>().Result;

                newProxy.Client = this;

                return newProxy;
            }
        }

        /// <summary>
        /// Finds the proxy by name.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">proxyName</exception>
        public Proxy FindProxy(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            using (var httpClient = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}", proxyName);
                var response = httpClient.GetAsync(url).Result;

                CheckIsSuccessStatusCode(response);

                var proxy = response.Content.ReadAsAsync<Proxy>().Result;

                proxy.Client = this;

                return proxy;
            }
        }

        /// <summary>
        /// Deletes the specified proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <exception cref="ArgumentNullException">proxy</exception>
        public void Delete(Proxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }
            Delete(proxy.Name);
        }

        /// <summary>
        /// Deletes the specified proxy name.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <exception cref="ArgumentNullException">proxyName</exception>
        public void Delete(string proxyName)
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            using (var httpClient = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}", proxyName);
                var response = httpClient.DeleteAsync(url).Result;

                CheckIsSuccessStatusCode(response);
            }
        }
        #endregion

        #region Toxic
        /// <summary>
        /// Finds a toxic by proxy name and toxic name.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="toxicName">Name of the toxic.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// proxy
        /// or
        /// toxic name
        /// </exception>
        internal ToxicBase FindToxicByProxyNameAndToxicName(Proxy proxy, string toxicName)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            if (string.IsNullOrEmpty(toxicName))
            {
                throw new ArgumentNullException("toxic name");
            }

            using (var httpClient = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}/toxics/{1}", proxy.Name, toxicName);
                var response = httpClient.GetAsync(url).Result;
                CheckIsSuccessStatusCode(response);

                var responseContent = response.Content.ReadAsStringAsync().Result;
                var toxic = JsonConvert.DeserializeObject<ToxicBase>(responseContent, _deserializeConverter);

                toxic.Client = this;
                toxic.ParentProxy = proxy;

                return toxic;
            }
        }

        /// <summary>
        /// Adds the toxic to the specific proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxy">The proxy.</param>
        /// <param name="toxic">The toxic.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// proxy
        /// or
        /// toxic
        /// </exception>
        internal T AddToxicToProxy<T>(Proxy proxy, T toxic) where T : ToxicBase
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            if (toxic == null)
            {
                throw new ArgumentNullException("toxic");
            }

            using (var client = _clientFactory.Create())
            {
                var url = string.Format("proxies/{0}/toxics", proxy.Name);
                var objectSerialized = JsonConvert.SerializeObject(toxic);
                var response = client.PostAsync(url, new StringContent(objectSerialized, Encoding.UTF8, "application/json")).Result;

                CheckIsSuccessStatusCode(response);

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(content);

                toxic.Client = this;
                toxic.ParentProxy = proxy;

                return result;
            }
        }

        /// <summary>
        /// Finds the name of the toxics in the proxy with the specified name.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <returns></returns>
        internal IEnumerable<ToxicBase> FindAllToxicsByProxyName(string proxyName)
        {
            using (var client = _clientFactory.Create())
            {
                var url = string.Format("proxies/{0}/toxics", proxyName);
                var response = client.GetAsync(url).Result;

                CheckIsSuccessStatusCode(response);
                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<ToxicBase>>(content, _deserializeConverter);
                return result;
            }
        }

        /// <summary>
        /// Removes the toxic from a proxy.
        /// </summary>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <param name="toxicName">Name of the toxic.</param>
        internal void RemoveToxicFromProxy(string proxyName, string toxicName)
        {
            using (var client = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}/toxics/{1}", proxyName, toxicName);
                var response = client.DeleteAsync(url).Result;

                CheckIsSuccessStatusCode(response);
            }
        }

        /// <summary>
        /// Updates the toxic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyName">Name of the proxy.</param>
        /// <param name="existingToxicName">Name of the existing toxic.</param>
        /// <param name="toxic">The toxic.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// proxyName
        /// or
        /// toxic
        /// </exception>
        internal T UpdateToxic<T>(string proxyName, string existingToxicName, T toxic) where T : ToxicBase
        {
            if (string.IsNullOrEmpty(proxyName))
            {
                throw new ArgumentNullException("proxyName");
            }

            if (toxic == null)
            {
                throw new ArgumentNullException("toxic");
            }

            using (var client = _clientFactory.Create())
            {
                var url = string.Format("/proxies/{0}/toxics/{1}", proxyName, existingToxicName);
                var objectSerialized = JsonConvert.SerializeObject(toxic);
                var response = client.PostAsync(url, new StringContent(objectSerialized, Encoding.UTF8, "application/json")).Result;

                CheckIsSuccessStatusCode(response);

                var content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(content, _deserializeConverter);
                return result;
            }
        }

        /// <summary>
        /// Checks the response status code and throw exceptions in case of failure status code.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="ToxiProxiException">
        /// Not found
        /// or
        /// duplicated entity
        /// or
        /// An error occurred: " + error.title
        /// </exception>
        #endregion

        private void CheckIsSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new ToxiProxiException("Not found");

                    case HttpStatusCode.Conflict:
                        throw new ToxiProxiException("duplicated entity");

                    default:
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        var error = JsonConvert.DeserializeObject<ToxiProxiErrorMessage>(errorContent);
                        throw new ToxiProxiException("An error occurred: " + error.title);
                }
            }
        }
    }
}
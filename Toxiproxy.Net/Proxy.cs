using System.Collections.Generic;
using Toxiproxy.Net.Toxics;

namespace Toxiproxy.Net
{
    public class Proxy
    {
        internal Client Client { get; set; }

        public string Name { get; set; }
        public string Listen { get; set; }
        public string Upstream { get; set; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Deletes this proxy.
        /// </summary>
        public void Delete()
        {
            Client.Delete(this);
        }

        /// <summary>
        /// Updates this proxy.
        /// </summary>
        /// <returns></returns>
        public Proxy Update()
            => Client.Update(this);

        /// <summary>
        /// Adds the specified toxic to this proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toxic">The toxic.</param>
        /// <returns></returns>
        public T Add<T>(T toxic) where T : ToxicBase
            => Client.AddToxicToProxy<T>(this, toxic);

        /// <summary>
        /// Gets all the toxics.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ToxicBase> GetAllToxics()
            => Client.FindAllToxicsByProxyName(Name);

        /// <summary>
        /// Gets a toxic by name in this proxy.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ToxicBase GetToxicByName(string name)
            => Client.FindToxicByProxyNameAndToxicName(this, name);

        /// <summary>
        /// Removes the toxic.
        /// </summary>
        /// <param name="toxicName">Name of the toxic.</param>
        public void RemoveToxic(string toxicName)
        {
            Client.RemoveToxicFromProxy(Name, toxicName);
        }

        /// <summary>
        /// Updates the toxic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toxicName">Name of the toxic.</param>
        /// <param name="toxic">The toxic.</param>
        public void UpdateToxic<T>(string toxicName, T toxic) where T : ToxicBase
        {
            Client.UpdateToxic(Name, toxicName, toxic);
        }
    }
}
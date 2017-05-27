using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common.Modules.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common.Repositories.UserModules
{
    /// <summary>
    /// Store user -> modules relations in json file
    /// </summary>
    public class FileUserModulesRepository : IUserModulesRepository
    {
        public string FilePath { get; }

        public FileUserModulesRepository(string filePath)
        {
            FilePath = filePath;

            if (!File.Exists(FilePath))
            {
                using (JsonTextWriter w = new JsonTextWriter(new StreamWriter(File.Create(FilePath))))
                {
                    JObject t = new JObject();
                    t.WriteTo(w);
                }
            }
        }

        /// <inheritdoc />
        public void AddModule(string userId, string clientId, ModuleIdentity module)
        {
            JObject t;
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                t = (JObject) JToken.ReadFrom(r);
                t.TryGetValue(userId, out var clients);
                if (clients != null)
                {
                    ((JObject) clients).TryGetValue(clientId, out var identities);
                    if (identities != null)
                        ((JArray)identities).Add(module.ToString());
                    else
                        ((JObject)clients).Add(clientId, new JArray(module.ToString()));
                }
                else
                {
                    t.Add(userId, new JObject(
                        new JProperty(
                            clientId,
                            new JArray(module.ToString()))));
                }
            }
            using (JsonWriter w = new JsonTextWriter(new StreamWriter(File.OpenWrite(FilePath))))
            {
                t.WriteTo(w);
            }
        }

        /// <inheritdoc />
        public void RemoveModule(string userId, string clientId, ModuleIdentity module)
        {
            JObject t;
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                t = (JObject)JToken.ReadFrom(r);
                t.TryGetValue(userId, out var clients);
                JToken identities = null;
                ((JObject) clients)?.TryGetValue(clientId, out identities);
                var jT = ((JArray)identities)?.First(x => Extensions.Value<string>(x) == module.ToString());
                ((JArray)identities)?.Remove(jT);
            }
            using (JsonWriter w = new JsonTextWriter(new StreamWriter(File.OpenWrite(FilePath))))
            {
                t.WriteTo(w);
            }
        }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> GetModules(string userId, string clientId)
        {
            var res = new List<ModuleIdentity>();
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                var t = (JObject)JToken.ReadFrom(r);
                t.TryGetValue(userId, out var clients);
                JToken identities = null;
                ((JObject) clients)?.TryGetValue(clientId, out identities);
                if (identities != null)
                {
                    foreach (var moduleIdentity in ((JArray)identities).Select(x => ModuleIdentity.Parse(x.Value<string>())))
                        res.Add(moduleIdentity);
                }
            }
            return res;
        }

        public IEnumerable<(string userId, string clientType, ModuleIdentity identity)> GetAll()
        {
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                var t = (JObject)JToken.ReadFrom(r);
                foreach (var x in t.Properties())
                {
                    var userId = x.Name;
                    foreach (var val in ((JObject)x.Value).Properties())
                    {
                        var clientType = val.Name;
                        var identities = ((JArray) val.Value).Values<string>();
                        foreach (var identity in identities)
                        {
                            yield return (userId, clientType, ModuleIdentity.Parse(identity));
                        }
                    }
                }
            }
        }
    }
}

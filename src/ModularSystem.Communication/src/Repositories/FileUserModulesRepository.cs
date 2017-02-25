using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Data.Mappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Communication.Repositories
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
        public void AddModule(string userId, ModuleIdentity module)
        {
            JObject t;
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                t = (JObject) JToken.ReadFrom(r);
                JToken identities;
                t.TryGetValue(userId, out identities);
                if (identities != null)
                    ((JArray)identities).Add(module.ToString());
                else
                    t.Add(userId, new JArray(module.ToString()));
            }
            using (JsonWriter w = new JsonTextWriter(new StreamWriter(File.OpenWrite(FilePath))))
            {
                t.WriteTo(w);
            }
        }

        /// <inheritdoc />
        public void RemoveModule(string userId, ModuleIdentity module)
        {
            JObject t;
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                t = (JObject)JToken.ReadFrom(r);
                JToken identities;
                t.TryGetValue(userId, out identities);
                var jT = ((JArray) identities)?.First(x => x.Value<string>() == module.ToString());
                ((JArray) identities)?.Remove(jT);
            }
            using (JsonWriter w = new JsonTextWriter(new StreamWriter(File.OpenWrite(FilePath))))
            {
                t.WriteTo(w);
            }
        }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> GetModules(string userId)
        {
            var res = new List<ModuleIdentity>();
            using (JsonReader r = new JsonTextReader(File.OpenText(FilePath)))
            {
                var t = (JObject)JToken.ReadFrom(r);
                JToken identities;
                t.TryGetValue(userId, out identities);
                if (identities != null)
                {
                    foreach (var moduleIdentity in ((JArray) identities).Select(x => ModuleIdentity.Parse(x.Value<string>())))
                        res.Add(moduleIdentity);
                }
            }
            return res;
        }
    }
}

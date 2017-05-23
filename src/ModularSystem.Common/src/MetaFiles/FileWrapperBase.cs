using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common.MetaFiles
{
    public class FileWrapperBase
    {
        protected readonly JObject File;

        public FileWrapperBase()
        {
            File = new JObject();
        }

        public FileWrapperBase(JObject file)
        {
            File = file;
        }

        public T GetValue<T>(string key)
        {
            return File[key].Value<T>();
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            return ContainsKey(key)
                ? GetValue<T>(key)
                : defaultValue;
        }

        public T[] GetValues<T>(string key)
        {
            return File[key].Values<T>().ToArray();
        }

        public T[] GetValues<T>(string key, T[] defaultValue)
        {
            return ContainsKey(key)
                ? GetValues<T>(key)
                : defaultValue;
        }

        public void SetValue<T>(string key, T value)
        {
            File.Add(key, new JValue(value));
        }

        public void SetValues<T>(string key, IEnumerable<T> value)
        {
            File.Add(key, new JArray(value));
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, JToken>)File).ContainsKey(key);
        }

        public void Write(Stream s)
        {
            using (var w = new StreamWriter(s))
            using (var jw = new JsonTextWriter(w))
            {
                File.WriteTo(jw);
            }
        }

        public void Write(string path)
        {
            using (var f = System.IO.File.OpenWrite(path))
            {
                Write(f);
            }
        }
    }
}
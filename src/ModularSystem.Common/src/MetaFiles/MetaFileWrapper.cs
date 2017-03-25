using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common.MetaFiles
{
    public class MetaFileWrapper
    {
        public const string FileName = "meta.json";

        private readonly JObject _file;

        public MetaFileWrapper()
        {
            _file = new JObject();
        }

        public MetaFileWrapper(Stream s)
        {
            _file = JObject.Load(new JsonTextReader(new StreamReader(s)));
        }

        public MetaFileWrapper(string file)
        {
            _file = JObject.Load(new JsonTextReader(new StreamReader(File.OpenRead(file))));
        }

        /// <summary>
        /// Find meta file in dir, and use it
        /// </summary>
        public MetaFileWrapper(string path, bool recurrently = false)
        {
            var t = Directory.GetFiles(path, FileName, recurrently ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).First();
            _file = JObject.Load(new JsonTextReader(new StreamReader(File.OpenRead(t))));
        }

        #region Properties

        public const string DependenciesKey = "dependencies";
        public const string TypeKey = "type";

        public string[] Dependencies
        {
            get { return GetValues<string>(DependenciesKey); }
            set { SetValues(DependenciesKey, value); }
        }

        public string Type
        {
            get { return GetValue<string>(TypeKey); }
            set { SetValue(TypeKey, value); }
        }

        public T GetValue<T>(string key)
        {
            return _file[key].Value<T>();
        }

        public T[] GetValues<T>(string key)
        {
            return _file[key].Values<T>().ToArray();
        }

        public void SetValue<T>(string key, T value)
        {
            _file.Add(key, new JValue(value));
        }

        public void SetValues<T>(string key, IEnumerable<T> value)
        {
            _file.Add(key, new JArray(value));
        }
        #endregion


        public void Write(Stream s)
        {
            using (var w = new StreamWriter(s))
            using (var jw = new JsonTextWriter(w))
            {
                _file.WriteTo(jw);
            }
        }

        public void Write(string path)
        {
            using (var f = File.OpenWrite(path))
            {
                Write(f);
            }
        }

        /// <summary>
        /// Create meta file and write to it
        /// </summary>
        public void CreateAndWrite(string path)
        {
            var t = Path.Combine(path, FileName);
            Write(t);
        }
    }
}

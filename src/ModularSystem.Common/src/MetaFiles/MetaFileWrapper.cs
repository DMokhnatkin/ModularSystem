using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common.MetaFiles
{
    public class MetaFileWrapper
    {
        public const string FileName = "meta.json";

        private readonly JToken _file;

        public MetaFileWrapper()
        {
            _file = new JObject();
        }

        public MetaFileWrapper(Stream s)
        {
            _file = JToken.ReadFrom(new JsonTextReader(new StreamReader(s)));
        }

        public MetaFileWrapper(string file)
        {
            _file = JToken.ReadFrom(new JsonTextReader(new StreamReader(File.OpenRead(file))));
        }

        /// <summary>
        /// Find meta file in dir, and use it
        /// </summary>
        public MetaFileWrapper(string path, bool recurrently = false)
        {
            var t = Directory.GetFiles(path, FileName, recurrently ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).First();
            _file = JToken.ReadFrom(new JsonTextReader(new StreamReader(File.OpenRead(t))));
        }

        public const string DependenciesKey = "dependencies";
        public const string TypeKey = "type";

        public string[] Dependencies
        {
            get { return _file[DependenciesKey]?.Values<string>().ToArray(); }
            set { _file[DependenciesKey] = new JArray(value); }
        }

        public string Type
        {
            get { return _file[TypeKey]?.Value<string>(); }
            set { _file[TypeKey] = value; }
        }

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

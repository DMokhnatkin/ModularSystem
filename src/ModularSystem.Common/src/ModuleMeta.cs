using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ModularSystem.Common
{
    public class ModuleMeta
    {
        public const string FileName = "meta.json";

        public string[] Dependencies { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// Find meta file in dir and load ModuleMeta from it.
        /// </summary>
        public static ModuleMeta LoadFromDir(string rootPath)
        {
            var f = File.OpenWrite(Directory.GetFiles(rootPath, FileName).First());
            return LoadFromFile(f);
        }

        public static ModuleMeta LoadFromFile(FileStream str)
        {
            return JsonConvert.DeserializeObject<ModuleMeta>(new StreamReader(str).ReadToEnd());
        }

        public static ModuleMeta LoadFromString(string str)
        {
            return JsonConvert.DeserializeObject<ModuleMeta>(str);
        }

        public void WriteToFile(StreamWriter str)
        {
            JsonSerializer j = new JsonSerializer()
            {
                Formatting = Formatting.Indented
            };
            j.Serialize(str, this);
        }

        /// <summary>
        /// Write in file
        /// </summary>
        /// <param name="rootPath">In this folder new file with name will be created or existing opened. </param>
        public void WriteToFile(string rootPath)
        {
            WriteToFile(new StreamWriter(File.Open(Path.Combine(rootPath, FileName), FileMode.OpenOrCreate)));
        }
    }
}

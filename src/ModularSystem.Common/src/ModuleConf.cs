using System.IO;
using Newtonsoft.Json;

namespace ModularSystem.Common
{
    public class ModuleConf
    {
        public string ModuleIdentity { get; set; }
        public string[] Dependencies { get; set; }

        public static ModuleConf LoadFromFile(FileStream str)
        {
            return JsonConvert.DeserializeObject<ModuleConf>(new StreamReader(str).ReadToEnd());
        }

        public static ModuleConf LoadFromString(string str)
        {
            return JsonConvert.DeserializeObject<ModuleConf>(str);
        }

        public void WriteToFile(StreamWriter str)
        {
            JsonSerializer j = new JsonSerializer()
            {
                Formatting = Formatting.Indented
            };
            j.Serialize(str, this);
        }
    }
}

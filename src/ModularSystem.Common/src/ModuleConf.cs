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

        public void WriteToFile(FileStream str)
        {
            new StreamWriter(str).Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}

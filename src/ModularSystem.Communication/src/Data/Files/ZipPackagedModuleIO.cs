using System.IO;
using System.IO.Compression;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;
using ModularSystem.Communication.Data.Mappers;
using Newtonsoft.Json;

namespace ModularSystem.Communication.Data.Files
{
    public static class ZipPackagedModuleIo
    {
        private const string ConfigFileName = "conf.json";

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public static ZipPackagedModule InitializeForZip(string path)
        {
            var res = new ZipPackagedModule();
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                using (JsonReader sr = new JsonTextReader(new StreamReader(z.GetEntry(ConfigFileName).Open())))
                {
                    var t = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    res.ModuleInfo = t.Deserialize<ModuleInfoDto>(sr).Unwrap();
                }
            }
            res.Path = path;
            return res;
        }
    }
}

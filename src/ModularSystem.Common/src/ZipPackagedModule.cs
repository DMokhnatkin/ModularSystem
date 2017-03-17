using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Transfer.Dto;
using ModularSystem.Common.Transfer.Mappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IPathStoredModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> Dependencies { get; }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public void InitializeFromZip(string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var s = new StreamReader(z.GetEntry(ModuleSettings.ConfFileName).Open()).ReadToEnd();
                JObject j = JObject.Parse(s);
                ModuleIdentity = j.GetValue("")
                using (JsonReader sr = new JsonTextReader())
                {
                    var o = new JObject()
                    var t = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    ModuleInfo = t.Deserialize<ModuleInfoDto>(sr).Unwrap();
                }
            }
            Path = path;
        }

        /// <summary>
        /// Pack all files in directory into zip and initialize ZipPackagedModule instance for it
        /// </summary>
        public static ZipPackagedModule Pack(string filesPath, string zipDestinationPath)
        {
            ZipFile.CreateFromDirectory(filesPath, zipDestinationPath);
            ZipPackagedModule m = new ZipPackagedModule();
            m.InitializeFromZip(zipDestinationPath);
            return m;
        }
    }
}

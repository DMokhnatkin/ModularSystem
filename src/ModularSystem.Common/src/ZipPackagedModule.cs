using System.IO;
using System.IO.Compression;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Transfer.Dto;
using ModularSystem.Common.Transfer.Mappers;
using Newtonsoft.Json;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IPathModule
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <inheritdoc />
        public string Path { get; set; }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public void InitializeFromPath(string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                using (JsonReader sr = new JsonTextReader(new StreamReader(z.GetEntry(ModuleSettings.ConfFileName).Open())))
                {
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
        public static ZipPackagedModule PackFiles(string filesPath, string zipDestinationPath)
        {
            ZipFile.CreateFromDirectory(filesPath, zipDestinationPath);
            ZipPackagedModule m = new ZipPackagedModule();
            m.InitializeFromPath(zipDestinationPath);
            return m;
        }
    }
}

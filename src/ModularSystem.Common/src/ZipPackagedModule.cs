using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IPathStoredModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public void InitializeFromZip(string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var s = new StreamReader(z.GetEntry(ModuleSettings.ConfFileName).Open());
                var t = ModuleConf.LoadFromString(s.ReadToEnd());
                ModuleInfo = new ModuleInfo(ModuleIdentity.Parse(t.ModuleIdentity), t.Dependencies.Select(ModuleIdentity.Parse).ToArray());
            }
            Path = path;
        }

        /// <summary>
        /// Pack all files in directory into zip and initialize ZipPackagedModule instance for it
        /// </summary>
        public static ZipPackagedModule PackFolder(string filesPath, string zipDestinationPath)
        {
            ZipFile.CreateFromDirectory(filesPath, zipDestinationPath);
            ZipPackagedModule m = new ZipPackagedModule();
            m.InitializeFromZip(zipDestinationPath);
            return m;
        }
    }
}

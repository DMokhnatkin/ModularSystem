using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IModule, IPathStoredModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

        internal ZipPackagedModule()
        { }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public static ZipPackagedModule InitializeFromZip(string path)
        {
            ModuleIdentity mi = ModuleIdentity.Parse(System.IO.Path.GetDirectoryName(path));

            ZipPackagedModule r = new ZipPackagedModule();
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var s = new StreamReader(z.GetEntry(ModuleSettings.ConfFileName).Open());
                var t = ModuleMeta.LoadFromString(s.ReadToEnd());
                r.ModuleInfo = new ModuleInfo(mi, t.Dependencies.Select(ModuleIdentity.Parse).ToArray());
            }
            r.Path = path;
            return r;
        }

        /// <summary>
        /// Pack all files in directory into zip and initialize ZipPackagedModule instance for it
        /// </summary>
        public static ZipPackagedModule PackFolder(string filesPath, string zipDestinationPath)
        {
            ZipFile.CreateFromDirectory(filesPath, zipDestinationPath);
            return InitializeFromZip(zipDestinationPath);
        }

        /// <summary>
        /// Unpack zip packaged module to destination directory.
        /// </summary>
        public void UnpackToFolder(string destFolder)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(Path)))
            {
                z.ExtractToDirectory(destFolder);
            }
        }
    }
}

using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IModule, IPathStoredModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; internal set; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies { get; internal set; }

        internal ZipPackagedModule()
        { }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public ZipPackagedModule(string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var t = new MetaFileWrapper(z.GetEntry(MetaFileWrapper.DefaultFileName).Open());
                ModuleIdentity = ModuleIdentity.Parse(t.Identity);
                Dependencies = t.Dependencies.Select(ModuleIdentity.Parse).ToArray();
            }
            Path = path;
        }

        /// <summary>
        /// Open meta files from archive. 
        /// </summary>
        public MetaFileWrapper ExtractMetaFile()
        {
            using (var z = new ZipArchive(File.OpenRead(Path)))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <summary>
        /// Update meta file in archive.
        /// </summary>
        public void UpdateMetaFile(MetaFileWrapper metaFile)
        {
            using (var z = new ZipArchive(File.OpenRead(Path)))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }
    }
}

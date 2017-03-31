using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class FilePackedModule : IModule, IPathStoredModule, IPackedModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; internal set; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies { get; internal set; }

        internal FilePackedModule()
        { }

        /// <summary>
        /// Initialize FilePackedModule from zip archive
        /// </summary>
        public FilePackedModule(string path)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var t = new MetaFileWrapper(z.GetEntry(MetaFileWrapper.DefaultFileName).Open());
                ModuleIdentity = ModuleIdentity.Parse(t.Identity);
                Dependencies = t.Dependencies.Select(ModuleIdentity.Parse).ToArray();
            }
            Path = path;
        }

        public Stream OpenStream()
        {
            return File.OpenRead(Path);
        }

        /// <inheritdoc />
        public MetaFileWrapper ExtractMetaFile()
        {
            using (var str = OpenStream())
            using (var z = new ZipArchive(str))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <inheritdoc />
        public void UpdateMetaFile(MetaFileWrapper metaFile)
        {
            using (var str = OpenStream())
            using (var z = new ZipArchive(str))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }
    }
}

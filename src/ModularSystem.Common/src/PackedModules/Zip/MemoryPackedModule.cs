using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.PackedModules.Zip
{
    public class MemoryPackedModule : IModule, IPackedModule
    {
        public byte[] Data { get; }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies { get; }

        /// <summary>
        /// Initialize FilePackedModule from byte array
        /// </summary>
        public MemoryPackedModule(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (ZipArchive z = new ZipArchive(ms))
            {
                var t = new MetaFileWrapper(z.GetEntry(MetaFileWrapper.DefaultFileName).Open());
                ModuleIdentity = ModuleIdentity.Parse(t.Identity);
                Dependencies = t.Dependencies.Select(ModuleIdentity.Parse).ToArray();
            }
            Data = data;
        }

        /// <inheritdoc />
        public Stream OpenStream()
        {
            return new MemoryStream(Data);
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

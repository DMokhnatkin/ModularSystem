using System.IO;
using System.IO.Compression;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.PackedModules.Zip
{
    /// <summary>
    /// Base class for all modules which are packed in zip archive.
    /// </summary>
    public abstract class ZipPackedModule : IPackedModule, IZipPacked
    {
        #region IPackedModule inherited

        /// <inheritdoc />
        public abstract Stream OpenWriteStream();

        /// <inheritdoc />
        public abstract Stream OpenReadStream();

        /// <inheritdoc />
        public abstract Stream OpenEditStream();

        /// <inheritdoc />
        public void CopyTo(Stream stream)
        {
            using (var s = OpenReadStream())
            {
                s.CopyTo(stream);
            }
        }

        /// <inheritdoc />
        public byte[] ExtractBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleIdentity ModuleIdentity { get; }

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleIdentity[] Dependencies { get; }

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleType ModuleType { get; }

        /// <inheritdoc />
        public MetaFileWrapper ExtractMetaFile()
        {
            using (var z = OpenReadZipArchive())
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <inheritdoc />
        public void UpdateMetaFile(MetaFileWrapper metaFile)
        {
            using (var z = OpenEditZipArchive())
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }

        #endregion IPackedModule inherited

        #region IZipPacked inherited

        /// <inheritdoc />
        public virtual ZipArchive OpenReadZipArchive()
        {
            return new ZipArchive(OpenReadStream(), ZipArchiveMode.Read);
        }

        /// <inheritdoc />
        public virtual ZipArchive OpenCreateZipArchive()
        {
            return new ZipArchive(OpenWriteStream(), ZipArchiveMode.Create);
        }

        /// <inheritdoc />
        public virtual ZipArchive OpenEditZipArchive()
        {
            return new ZipArchive(OpenEditStream(), ZipArchiveMode.Update);
        }

        #endregion IZipPacked inherited
    }
}

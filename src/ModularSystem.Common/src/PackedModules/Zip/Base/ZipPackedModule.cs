using System.IO;
using System.IO.Compression;

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

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleIdentity ModuleIdentity { get; }

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleIdentity[] Dependencies { get; }

        // ToRemove
        /// <inheritdoc />
        public abstract ModuleType ModuleType { get; }

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

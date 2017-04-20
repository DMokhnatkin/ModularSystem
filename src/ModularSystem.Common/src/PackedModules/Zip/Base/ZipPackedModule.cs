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

        /// <inheritdoc />
        public abstract ModuleIdentity ModuleIdentity { get; }

        /// <inheritdoc />
        public abstract ModuleIdentity[] Dependencies { get; }

        #endregion IPackedModule inherited

        #region IZipPacked inherited

        /// <inheritdoc />
        public virtual ZipArchive OpeReadZipArchive()
        {
            using (var readStream = OpenReadStream())
            {
                return new ZipArchive(readStream, ZipArchiveMode.Read);
            }
        }

        /// <inheritdoc />
        public virtual ZipArchive OpenCreateZipArchive()
        {
            using (var writeStream = OpenWriteStream())
            {
                return new ZipArchive(writeStream, ZipArchiveMode.Create);
            }
        }

        /// <inheritdoc />
        public virtual ZipArchive OpenEditZipArchive()
        {
            using (var editStream = OpenEditStream())
            {
                return new ZipArchive(editStream, ZipArchiveMode.Update);
            }
        }

        #endregion IZipPacked inherited
    }
}

using System.IO;
using System.IO.Compression;

namespace ModularSystem.Common.PackedModules.Zip
{
    /// <summary>
    /// Base class for all batches using zip archive.
    /// </summary>
    public abstract class ZipBatchedModules : IBatchedModules, IZipPacked
    {
        #region IBatchedModules inherited

        /// <inheritdoc />
        public abstract Stream OpenWriteStream();

        /// <inheritdoc />
        public abstract Stream OpenReadStream();

        /// <inheritdoc />
        public abstract Stream OpenEditStream();

        #endregion

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

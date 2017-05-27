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
        #endregion

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

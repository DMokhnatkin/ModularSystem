using System.IO.Compression;

namespace ModularSystem.Common.PackedModules.Zip
{
    public interface IZipPacked
    {
        /// <summary>
        /// Open packed data as zip archive. Only read is permitted.
        /// </summary>
        ZipArchive OpeReadZipArchive();

        /// <summary>
        /// Open packed data as zip archive. Only write is permitted.
        /// </summary>
        ZipArchive OpenCreateZipArchive();

        /// <summary>
        /// Open packed data as zip archive. Both read and write are permitted.
        /// </summary>
        ZipArchive OpenEditZipArchive();
    }
}

using System;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class PackHelperV2
    {
        #region PackModules

        /// <summary>
        /// Check if files in directory are valid module files.
        /// </summary>
        /// <param name="sourceDirectoryPath"></param>
        /// <param name="metaFileName"></param>
        private static void EnsureModuleIsValid(string sourceDirectoryPath, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            MetaFileWrapper meta = MetaFileWrapper.FindInDirectory(sourceDirectoryPath, metaFileName);
            if (!meta.ContainsKey(MetaFileWrapper.TypeKey))
            {
                throw new ArgumentException($"Invalid meta file: '{MetaFileWrapper.TypeKey}' key was not found in file");
            }
            if (!meta.ContainsKey(MetaFileWrapper.IdentityKey))
            {
                throw new ArgumentException($"Invalid meta file: '{MetaFileWrapper.IdentityKey}' key was not found in file");
            }
        }

        // Pack directory to byte array.
        private static byte[] PackDirectoryToByteArray(string sourceDirectoryPath)
        {
            // TODO: can be done without temp archive on disk. (default api doen't provide easy way).
            // using (var t = new ZipArchive(s, ZipArchiveMode.Create)) { ...CreateEntitiesManually... }
            var tmp = Path.GetTempFileName();
            File.Delete(tmp);
            ZipFile.CreateFromDirectory(sourceDirectoryPath, tmp);

            return File.ReadAllBytes(tmp);
        }

        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="module">Result</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static void PackModule(string sourceDirectoryPath, out MemoryPackedModuleV2 module, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            EnsureModuleIsValid(sourceDirectoryPath, metaFileName);

            // In this place you can do some extra actions depend on module type
            // **

            module = new MemoryPackedModuleV2(PackDirectoryToByteArray(sourceDirectoryPath));
        }

        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="destFilePath">Path to file which will be created</param>
        /// <param name="module">Result</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static void PackModule(string sourceDirectoryPath, string destFilePath, out FilePackedModuleV2 module, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            EnsureModuleIsValid(sourceDirectoryPath, metaFileName);

            // In this place you can do some extra actions depend on module type
            // **

            File.WriteAllBytes(destFilePath, PackDirectoryToByteArray(sourceDirectoryPath));
            module = new FilePackedModuleV2(destFilePath);
        }
        #endregion

        #region MetaFiles

        /// <summary>
        /// Open meta files from packed module. This is valid only for zip packed modules.
        /// </summary>
        public static MetaFileWrapper ExtractMetaFile(IPackedModuleV2 module)
        {
            using (var str = module.OpenReadStream())
            using (var z = new ZipArchive(str))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <summary>
        /// Update meta file in packed module. This is valid only for zip packed modules.
        /// </summary>
        public static void UpdateMetaFile(IPackedModuleV2 module, MetaFileWrapper metaFile)
        {
            using (var str = module.OpenEditStream())
            using (var z = new ZipArchive(str, ZipArchiveMode.Update))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }
        #endregion
    }
}


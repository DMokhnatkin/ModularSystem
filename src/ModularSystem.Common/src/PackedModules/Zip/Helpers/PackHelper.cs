using System;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class PackHelper
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
        /// <param name="moduleInfo">Result</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static void PackModule(string sourceDirectoryPath, out MemoryPackedModuleInfo moduleInfo, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            EnsureModuleIsValid(sourceDirectoryPath, metaFileName);

            // In this place you can do some extra actions depend on module type
            // **

            moduleInfo = new MemoryPackedModuleInfo(PackDirectoryToByteArray(sourceDirectoryPath));
        }

        /// <summary>
        /// Alias <see cref="PackModule(string, out MemoryPackedModuleInfo, string)"/>
        /// </summary>
        public static MemoryPackedModuleInfo PackModuleToMemory(string sourceDirectoryPath, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            MemoryPackedModuleInfo res;
            PackModule(sourceDirectoryPath, out res, metaFileName);
            return res;
        }

        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="destFilePath">Path to file which will be created</param>
        /// <param name="moduleInfo">Result</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static void PackModule(string sourceDirectoryPath, string destFilePath, out FilePackedModuleInfo moduleInfo, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            EnsureModuleIsValid(sourceDirectoryPath, metaFileName);

            // In this place you can do some extra actions depend on module type
            // **

            File.WriteAllBytes(destFilePath, PackDirectoryToByteArray(sourceDirectoryPath));
            moduleInfo = new FilePackedModuleInfo(destFilePath);
        }

        /// <summary>
        /// Alias <see cref="PackModule(string, string, out FilePackedModuleInfo, string)"/>
        /// </summary>
        public static FilePackedModuleInfo PackModuleToFile(string sourceDirectoryPath, string destFilePath, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            FilePackedModuleInfo res;
            PackModule(sourceDirectoryPath, destFilePath, out res);
            return res;
        }
        #endregion

        #region Unpack

        /// <summary>
        /// Unpack module to directory
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="destination">Directory path where to unpack</param>
        public static void UnpackToDirectory(this ZipPackedModuleInfo moduleInfo, string destination)
        {
            using (var z = moduleInfo.OpenReadZipArchive())
            {
                z.ExtractToDirectory(destination);
            }
        }
        #endregion

        #region MetaFiles

        /// <summary>
        /// Open meta files from packed module. This is valid only for zip packed modules.
        /// </summary>
        public static MetaFileWrapper ExtractMetaFile(this ZipPackedModuleInfo moduleInfo)
        {
            using (var z = moduleInfo.OpenReadZipArchive())
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <summary>
        /// Update meta file in packed module. This is valid only for zip packed modules.
        /// </summary>
        public static void UpdateMetaFile(this ZipPackedModuleInfo moduleInfo, MetaFileWrapper metaFile)
        {
            using (var z = moduleInfo.OpenEditZipArchive())
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }
        #endregion
    }
}


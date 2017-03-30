using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common
{
    public static class ZipPackHelper
    {
        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="destDirectoryPath">Path to directory where packaged module file (zip archive by default) will be created</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static ZipPackagedModule PackModule(string sourceDirectoryPath, string destDirectoryPath, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            MetaFileWrapper meta = new MetaFileWrapper(sourceDirectoryPath, metaFileName);
            if (!meta.ContainsKey(MetaFileWrapper.TypeKey))
            {
                throw new ArgumentException($"Invalid meta file: '{MetaFileWrapper.TypeKey}' key was not found in file");
            }
            if (!meta.ContainsKey(MetaFileWrapper.IdentityKey))
            {
                throw new ArgumentException($"Invalid meta file: '{MetaFileWrapper.IdentityKey}' key was not found in file");
            }

            // In this place you can do some extra actions depend on module type

            var destFilePath = Path.Combine(destDirectoryPath, $"{meta.Identity}.zip");
            if (File.Exists(destFilePath))
                File.Delete(destFilePath);
            return ZipPackagedModule.PackFolder(sourceDirectoryPath, destFilePath);
        }

        /// <summary>
        /// Pack (batch) list of modules in one zip archive.
        /// </summary>
        /// <param name="destPath">Path of destination zip archive</param>
        /// <param name="packagedModules">List of modules to pack</param>
        public static void BatchModules(string destPath, IEnumerable<ZipPackagedModule> packagedModules)
        {
            using (var fs = File.OpenWrite(destPath))
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var zipPackagedModule in packagedModules)
                {
                    zip.CreateEntryFromFile(zipPackagedModule.Path, Path.GetFileName(zipPackagedModule.Path));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class PackHelper
    {
        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static MemoryPackedModule PackModuleToMemory(string sourceDirectoryPath, string metaFileName = MetaFileWrapper.DefaultFileName)
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

            // In this place you can do some extra actions depend on module type
            // **

            // TODO: can be done without temp archive on disk. (default api doen't provide easy way).
            // using (var t = new ZipArchive(s, ZipArchiveMode.Create)) { ...CreateEntitiesManually... }
            var tmp = Path.GetTempFileName();
            File.Delete(tmp);
            ZipFile.CreateFromDirectory(sourceDirectoryPath, tmp);

            return new MemoryPackedModule(File.ReadAllBytes(tmp));
        }

        /// <summary>
        /// Pack one module
        /// </summary>
        /// <param name="sourceDirectoryPath">Directory with module files</param>
        /// <param name="destDirectoryPath">Path to directory where packaged module file (zip archive by default) will be created</param>
        /// <param name="metaFileName">Name of module meta file</param>
        public static FilePackedModule PackModuleToFile(string sourceDirectoryPath, string destDirectoryPath, string metaFileName = MetaFileWrapper.DefaultFileName)
        {
            var memoryModule = PackModuleToMemory(sourceDirectoryPath);

            var destFilePath = Path.Combine(destDirectoryPath, $"{memoryModule.ModuleIdentity}.zip");

            using (var s = memoryModule.OpenStream())
            using (var fs = File.Create(destFilePath))
            {
                s.CopyTo(fs);
            }

            return new FilePackedModule(destFilePath);
        }

        /// <summary>
        /// Unpack zip packaged module to destination directory.
        /// </summary>
        public static void UnpackModule(this IPackedModule module, string destDirectoryPath)
        {
            using (var t = module.OpenStream())
            using (ZipArchive z = new ZipArchive(t))
            {
                z.ExtractToDirectory(destDirectoryPath);
            }
        }

        /// <summary>
        /// Pack (batch) list of modules in one zip archive.
        /// </summary>
        /// <param name="destPath">Path of destination zip archive</param>
        /// <param name="packagedModules">List of modules to pack</param>
        public static FileBatchedModules BatchModules(string destPath, IEnumerable<FilePackedModule> packagedModules)
        {
            using (var fs = File.OpenWrite(destPath))
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var zipPackagedModule in packagedModules)
                {
                    zip.CreateEntryFromFile(zipPackagedModule.Path, Path.GetFileName(zipPackagedModule.Path));
                }
            }

            return new FileBatchedModules(destPath);
        }

        /// <summary>
        /// Pack (batch) list of modules.
        /// </summary>
        public static MemoryBatchedModules BatchMemoryModules(IEnumerable<IPackedModule> packagedModules)
        {
            using (var fs = new MemoryStream())
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                foreach (var zipPackagedModule in packagedModules)
                {
                    if (zipPackagedModule is MemoryPackedModule)
                    {
                        MemoryPackedModule memoryPacked = (MemoryPackedModule) zipPackagedModule;
                        var entry = zip.CreateEntry(zipPackagedModule.ModuleIdentity.ToString());
                        using (var z = entry.Open())
                        {
                            z.Write(memoryPacked.Data, 0, memoryPacked.Data.Length);
                        }
                    }
                    if (zipPackagedModule is FilePackedModule)
                    {
                        FilePackedModule filePacked = (FilePackedModule)zipPackagedModule;
                        zip.CreateEntryFromFile(filePacked.Path, Path.GetFileName(filePacked.Path));
                    }
                }
                return new MemoryBatchedModules(fs.ToArray());
            }
        }

        /// <summary>
        /// Unpack (unbatch) collection of module.
        /// Inner modules will not be unpacked i.e. directoryPath will contains collection of zip (by default) archives as result.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="directoryPath">This directory will contains result (collection of zip archives i.e. packed modules)</param>
        public static FilePackedModule[] UnbatchModules(this IBatchedModules batch, string directoryPath)
        {
            List<FilePackedModule> res = new List<FilePackedModule>();
            using (var fs = batch.OpenStream())
            using (var bs = new ZipArchive(fs))
            {
                foreach (var zipArchiveEntry in bs.Entries)
                {
                        var dest = Path.Combine(directoryPath, zipArchiveEntry.Name);
                        zipArchiveEntry.ExtractToFile(dest);
                        res.Add(new FilePackedModule(dest));
                }
            }
            return res.ToArray();
        }

        /// <summary>
        /// Unpack (unbatch) collection of module to memory.
        /// Inner modules will not be unpacked.
        /// </summary>
        public static MemoryPackedModule[] UnbatchModulesToMemory(this IBatchedModules batch)
        {
            List<MemoryPackedModule> res = new List<MemoryPackedModule>();
            using (var fs = batch.OpenStream())
            using (var bs = new ZipArchive(fs))
            {
                foreach (var zipArchiveEntry in bs.Entries)
                {
                    using (var t = zipArchiveEntry.Open())
                    using (var br = new BinaryReader(t))
                    {
                        res.Add(new MemoryPackedModule(br.ReadBytes((int)t.Length)));
                    }
                }
            }
            return res.ToArray();
        }

        /// <summary>
        /// Unpack (unbatch) collection of module (which are batched in one file).
        /// Inner modules will be unpacked i.e. for each inner module will be created directory in directoryPath and module data will be extracted in this directories
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="directoryPath">This directory will contains result (collection of directories. In which directory files of some are existing)</param>
        public static void UnbatchAndUnpackModules(this IBatchedModules batch, string directoryPath)
        {
            using (var fs = batch.OpenStream())
            using (var bs = new ZipArchive(fs))
            {
                foreach (var zipArchiveEntry in bs.Entries)
                {
                    // Create directory for module
                    var moduleDir = Path.Combine(directoryPath, zipArchiveEntry.Name);
                    if (Directory.Exists(moduleDir))
                        Directory.Delete(moduleDir);
                    Directory.CreateDirectory(moduleDir);

                    using (var entryStream = zipArchiveEntry.Open())
                    using (var t = new BinaryReader(entryStream))
                    {
                        var r = new MemoryPackedModule(t.ReadBytes((int)entryStream.Length));
                        r.UnpackModule(moduleDir);
                    }
                }
            }
        }
    }
}


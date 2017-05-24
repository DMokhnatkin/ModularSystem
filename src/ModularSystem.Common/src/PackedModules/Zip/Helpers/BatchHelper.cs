using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class BatchHelper
    {
        #region Batch
        // Get list of modules and batch them in one zip archive, then return byte array of this archive.
        private static byte[] BatchModulesToByteArray(IEnumerable<IPackedModule> packedModules)
        {
            using (var fs = new MemoryStream())
            {
                using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    foreach (var zipPackedModule in packedModules)
                    {
                        var entry = zip.CreateEntry(Guid.NewGuid().ToString());
                        using (var entryStream = entry.Open())
                        using (var moduleStream = zipPackedModule.OpenReadStream())
                        {
                            moduleStream.CopyTo(entryStream);
                        }
                    }
                }
                return fs.ToArray();
            }
        }

        /// <summary>
        /// Pack (batch) list of modules in one zip archive.
        /// </summary>
        /// <param name="destPath">Path of destination zip archive</param>
        /// <param name="packedModules">List of modules to pack</param>
        /// <param name="batch">Result</param>
        public static void BatchModules(IEnumerable<IPackedModule> packedModules, string destPath, out FileBatchedModules batch)
        {
            File.WriteAllBytes(destPath, BatchModulesToByteArray(packedModules));
            batch = new FileBatchedModules(destPath);
        }

        /// <summary>
        /// Alias <see cref="BatchModules(IEnumerable{IPackedModule}, string, out FileBatchedModules)"/>
        /// </summary>
        public static FileBatchedModules BatchModulesToFile(IEnumerable<IPackedModule> packedModules, string destPath)
        {
            FileBatchedModules res;
            BatchModules(packedModules, destPath, out res);
            return res;
        }

        /// <summary>
        /// Pack (batch) list of modules in one zip archive.
        /// </summary>
        /// <param name="packedModules">List of modules to pack</param>
        /// <param name="batch">Result</param>
        public static void BatchModules(IEnumerable<IPackedModule> packedModules, out MemoryBatchedModules batch)
        {
            batch = new MemoryBatchedModules(BatchModulesToByteArray(packedModules));
        }

        /// <summary>
        /// Alias <see cref="BatchModules(IEnumerable{IPackedModule}, out MemoryBatchedModules)"/>
        /// </summary>
        public static MemoryBatchedModules BatchModulesToMemory(IEnumerable<IPackedModule> packedModules)
        {
            MemoryBatchedModules res;
            BatchModules(packedModules, out res);
            return res;
        }

        #endregion

        #region Unbatch

        // Unbatch each inner module and return array of byte arrays.
        private static byte[][] UnbatchModulesToByteArray(ZipBatchedModules batch)
        {
            List<byte[]> res = new List<byte[]>();
            using (var bs = batch.OpenReadZipArchive())
            {
                foreach (var zipArchiveEntry in bs.Entries)
                {
                    using (var t = zipArchiveEntry.Open())
                    using (var ms = new MemoryStream())
                    {
                        t.CopyTo(ms);
                        res.Add(ms.ToArray());
                    }
                }
            }
            return res.ToArray();
        }

        /// <summary>
        /// Unpack (unbatch) collection of module.
        /// Inner modules will not be unpacked i.e. directoryPath will contains collection of zip (by default) archives as result.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="directoryPath">This directory will contains result (collection of zip archives i.e. packed modules)</param>
        public static void UnbatchModules(this IBatchedModules batch, string directoryPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unpack (unbatch) collection of module to memory.
        /// Inner modules will not be unpacked.
        /// </summary>
        public static void UnbatchModules(this ZipBatchedModules batch, out MemoryPackedModule[] result)
        {
            result = UnbatchModulesToByteArray(batch)
                .Select(x => new MemoryPackedModule(x))
                .ToArray();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Extract data as byte array
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static byte[] ExtractData(this IBatchedModules batch)
        {
            using (var s = batch.OpenReadStream())
            using (var br = new BinaryReader(s))
            {
                return br.ReadBytes((int) s.Length);
            }
        }
        #endregion
    }
}

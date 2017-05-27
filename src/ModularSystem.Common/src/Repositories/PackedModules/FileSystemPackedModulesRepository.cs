using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.Repositories.PackedModules
{
    public class FileSystemPackedModulesRepository : IPackedModulesRepository
    {
        private string StoredPath { get; }

        public FileSystemPackedModulesRepository(string storedPath)
        {
            StoredPath = storedPath;
        }

        /// <inheritdoc />
        public IPackedModule AddModule(ModuleIdentity identity, IPackedModule packed)
        {
            var path = Path.Combine(StoredPath, $"{identity}.zip");
            if (File.Exists(path))
                throw new ArgumentException($"Module {identity} is already in repository");

            // Copy packed data
            using (var writeStream = File.Create(path))
            {
                packed.CopyTo(writeStream);
            }

            return new FilePackedModule(path);
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} wasn't registered");
            Directory.Delete(Path.Combine(StoredPath, $"{moduleIdentity}.zip"), true);
        }

        /// <inheritdoc />
        public IEnumerable<ModuleIdentity> GetIdentities()
        {
            foreach (var f in Directory.GetFiles(StoredPath))
            {
                Regex reg = new Regex("^(.+).zip$");
                yield return ModuleIdentity.Parse(reg.Match(f).Groups[1].Value);
            }
        }

        /// <inheritdoc />
        public IPackedModule GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return new FilePackedModule(Path.Combine(StoredPath, $"{moduleIdentity}.zip"));
        }

        /// <inheritdoc />
        public bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return File.Exists(Path.Combine(StoredPath, $"{moduleIdentity}.zip"));
        }
    }
}

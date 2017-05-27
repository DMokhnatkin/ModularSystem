using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModularSystem.Common.Modules;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.Repositories
{
    public class FileSystemModulesRepository : IModulesRepository<IPackedModuleInfo>
    {
        public string BasePath { get; }

        public FileSystemModulesRepository(string basePath)
        {
            BasePath = basePath;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
        }

        /// <inheritdoc />
        public IEnumerator<IPackedModuleInfo> GetEnumerator()
        {
            foreach (var f in Directory.GetFiles(BasePath))
            {
                yield return new FilePackedModuleInfo(f);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddModule(IPackedModuleInfo moduleInfo)
        {
            if (IsModuleRegistered(moduleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {moduleInfo.ModuleIdentity} is already registered");
            using (var f = File.OpenWrite(Path.Combine(BasePath, $"{moduleInfo.ModuleIdentity}.zip")))
            using (var ms = moduleInfo.OpenReadStream())
            using (var msr = new BinaryReader(ms))
            {
                var data = msr.ReadBytes((int) ms.Length);
                f.Write(data, 0, data.Length);
            }
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} wasn't registered");
            Directory.Delete(Path.Combine(BasePath, $"{moduleIdentity}.zip"), true);
        }

        /// <inheritdoc />
        public IPackedModuleInfo GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return new FilePackedModuleInfo(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }

        /// <inheritdoc />
        public bool ContainsModule(ModuleIdentity moduleIdentity)
        {
            return IsModuleRegistered(moduleIdentity);
        }

        private bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return File.Exists(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }
    }
}

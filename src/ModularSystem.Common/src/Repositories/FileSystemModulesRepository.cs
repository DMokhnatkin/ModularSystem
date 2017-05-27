using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.Repositories
{
    // ToRemove
    public class FileSystemModulesRepository : IModulesRepository<IPackedModule>
    {
        public string BasePath { get; }

        public FileSystemModulesRepository(string basePath)
        {
            BasePath = basePath;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
        }

        /// <inheritdoc />
        public IEnumerator<IPackedModule> GetEnumerator()
        {
            foreach (var f in Directory.GetFiles(BasePath))
            {
                yield return new FilePackedModule(f);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddModule(IPackedModule module)
        {
            if (IsModuleRegistered(module.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleIdentity} is already registered");
            using (var f = File.OpenWrite(Path.Combine(BasePath, $"{module.ModuleIdentity}.zip")))
            using (var ms = module.OpenReadStream())
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
        public IPackedModule GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return new FilePackedModule(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }

        private bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return File.Exists(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }
    }
}

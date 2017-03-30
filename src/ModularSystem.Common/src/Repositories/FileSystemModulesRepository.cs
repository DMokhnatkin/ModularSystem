using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ModularSystem.Common.Repositories
{
    public class FileSystemModulesRepository : IModulesRepository<ZipPackagedModule>
    {
        public string BasePath { get; }

        public FileSystemModulesRepository(string basePath)
        {
            BasePath = basePath;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
        }

        /// <inheritdoc />
        public IEnumerator<ZipPackagedModule> GetEnumerator()
        {
            foreach (var f in Directory.GetFiles(BasePath))
            {
                yield return new ZipPackagedModule(f);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddModule(ZipPackagedModule module)
        {
            if (IsModuleRegistered(module.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleIdentity} is already registered");
            File.Copy(module.Path, Path.Combine(BasePath, $"{module.ModuleIdentity}.zip"));
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} wasn't registered");
            Directory.Delete(Path.Combine(BasePath, $"{moduleIdentity}.zip"), true);
        }

        /// <inheritdoc />
        public ZipPackagedModule GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return new ZipPackagedModule(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }

        private bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return File.Exists(Path.Combine(BasePath, $"{moduleIdentity}.zip"));
        }
    }
}

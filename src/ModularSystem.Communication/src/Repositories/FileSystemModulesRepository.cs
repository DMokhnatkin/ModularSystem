using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModularSystem.Common;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Data.Files;

namespace ModularSystem.Communication.Repositories
{
    public class FileSystemModulesRepository : IModulesRepository
    {
        public string BasePath { get; }

        public FileSystemModulesRepository(string basePath)
        {
            BasePath = basePath;

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
        }

        /// <inheritdoc />
        public IEnumerator<IPathModule> GetEnumerator()
        {
            foreach (var directory in Directory.GetFiles(BasePath))
            {
                yield return ZipPackagedModuleIo.InitializeForZip(directory);
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddModule(IPathModule packagedModule)
        {
            if (IsModuleRegistered(packagedModule.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {packagedModule.ModuleInfo.ModuleIdentity} is already registered");
            File.Copy(packagedModule.Path, Path.Combine(BasePath, packagedModule.ModuleInfo.ModuleIdentity.ToString()));
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} wasn't registered");
            Directory.Delete(Path.Combine(BasePath, moduleIdentity.ToString()), true);
        }

        /// <inheritdoc />
        public IPathModule GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return ZipPackagedModuleIo.InitializeForZip(Path.Combine(BasePath, moduleIdentity.ToString()));
        }

        private bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return File.Exists(Path.Combine(BasePath, moduleIdentity.ToString()));
        }
    }
}

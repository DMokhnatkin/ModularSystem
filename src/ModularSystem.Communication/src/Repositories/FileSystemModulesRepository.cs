using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ModularSystem.Common;
using ModularSystem.Common.Repositories;
using ModularSystem.Communication.Data.Files;
using ModularSystem.Communication.Data.Mappers;

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
        public IEnumerator<IModule> GetEnumerator()
        {
            foreach (var directory in Directory.GetDirectories(BasePath))
            {
                yield return ModuleDtoFileSystem.ReadFromDirectory(directory).Unwrap().Result;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void AddModule(IModule module)
        {
            if (IsModuleRegistered(module.ModuleInfo.ModuleIdentity))
                throw new ArgumentException($"Module {module.ModuleInfo.ModuleIdentity} is already registered");
            module.Wrap().Result.WriteToDirectory(Path.Combine(BasePath, module.ModuleInfo.ModuleIdentity.ToString()));
        }

        /// <inheritdoc />
        public void RemoveModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                throw new ArgumentException($"Module {moduleIdentity} wasn't registered");
            Directory.Delete(Path.Combine(BasePath, moduleIdentity.ToString()), true);
        }

        /// <inheritdoc />
        public IModule GetModule(ModuleIdentity moduleIdentity)
        {
            if (!IsModuleRegistered(moduleIdentity))
                return null;
            return ModuleDtoFileSystem.ReadFromDirectory(Path.Combine(BasePath, moduleIdentity.ToString())).Unwrap().Result;
        }

        private bool IsModuleRegistered(ModuleIdentity moduleIdentity)
        {
            return Directory.Exists(Path.Combine(BasePath, moduleIdentity.ToString()));
        }
    }
}

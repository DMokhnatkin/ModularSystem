using System;
using System.Collections.Generic;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules.Client;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Common.Repositories.PackedModules;

namespace ModularSystem.Common.BLL
{
    public class ClientModulesManager
    {
        // Persistent storage for packed client modules.
        private readonly IPackedModulesRepository _modulesRepository;

        // Cache over repository.
        private Dictionary<ModuleIdentity, ServerSideClientModule> _modules;

        public ClientModulesManager(IPackedModulesRepository modulesRepository)
        {
            _modulesRepository = modulesRepository;
            InitializeFromRepository();
        }

        /// <summary>
        /// Load cache (_modules) from repository
        /// </summary>
        private void InitializeFromRepository()
        {
            _modules = new Dictionary<ModuleIdentity, ServerSideClientModule>();
            foreach (var moduleIdentity in _modulesRepository.GetIdentities())
            {
                var packedModule = _modulesRepository.GetModule(moduleIdentity);
                var meta = packedModule.ExtractMetaFile();
                var newModule = CreateFromMeta(meta, packedModule);
                _modules.Add(newModule.ModuleIdentity, newModule);
            }
        }

        private ServerSideClientModule CreateFromMeta(MetaFileWrapper meta, IPackedModule packed)
        {
            return new ServerSideClientModule(
                ModuleIdentity.Parse(meta.Identity),
                meta.Dependencies.Select(ModuleIdentity.Parse).ToArray(),
                meta.ClientTypes,
                packed);
        }

        public ServerSideClientModule InstallModule(ZipPackedModule module)
        {
            var moduleMeta = module.ExtractMetaFile();

            var newPackedModule = _modulesRepository.AddModule(ModuleIdentity.Parse(moduleMeta.Identity), module);
            // Parse module info
            var newModule = CreateFromMeta(moduleMeta, newPackedModule);
            _modules[ModuleIdentity.Parse(moduleMeta.Identity)] = newModule;
            return newModule;
        }

        public ModuleIdentity[] GetListOfInstalledModules()
        {
            return _modules.Keys.ToArray();
        }

        public ServerSideClientModule[] GetInstalledModules(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            var res = new List<ServerSideClientModule>();
            foreach (var moduleIdentity in moduleIdentities)
            {
                var module = _modules[moduleIdentity];
                if (module == null)
                    throw new ArgumentException($"Client module {moduleIdentity} wasn't installed");
                res.Add(module);
            }
            return res.ToArray();
        }
    }
}

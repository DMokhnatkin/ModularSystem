using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common.Modules.Client;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.BLL
{
    public class ClientModulesManager
    {
        public string ClientModulesStorePath { get; }

        private readonly Dictionary<ModuleIdentity, ServerSideClientModule> _clientModules = new Dictionary<ModuleIdentity, ServerSideClientModule>();

        public ClientModulesManager(string clientModulesStorePath)
        {
            ClientModulesStorePath = clientModulesStorePath;

            FileSystemHelpers.ClearOrCreateDir(ClientModulesStorePath);
        }

        public ServerSideClientModule InstallModule(ZipPackedModule module)
        {
            var moduleMeta = module.ExtractMetaFile();

            // Copy packed data
            var modulePath = Path.Combine(ClientModulesStorePath, $"{moduleMeta.Identity}.zip");
            using (var moduleStream = module.OpenReadStream())
            using (var writeStream = File.Create(modulePath))
            {
                moduleStream.CopyTo(writeStream);
            }
            // Parse module info
            var packed = new FilePackedModule(modulePath);
            var newModule = new ServerSideClientModule(
                ModuleIdentity.Parse(moduleMeta.Identity),
                moduleMeta.Dependencies.Select(ModuleIdentity.Parse).ToArray(),
                moduleMeta.ClientTypes,
                packed);
            _clientModules.Add(newModule.ModuleIdentity, newModule);
            return newModule;
        }

        public ModuleIdentity[] GetListOfInstalledModules()
        {
            return _clientModules.Values.Select(x => x.ModuleIdentity).ToArray();
        }

        public ServerSideClientModule[] GetInstalledModules(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            var res = new List<ServerSideClientModule>();
            foreach (var moduleIdentity in moduleIdentities)
            {
                if (!_clientModules.ContainsKey(moduleIdentity))
                    throw new ArgumentException($"Client module {moduleIdentity} wasn't installed");
                res.Add(_clientModules[moduleIdentity]);
            }
            return res.ToArray();
        }
    }
}

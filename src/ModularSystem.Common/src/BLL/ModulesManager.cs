using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModularSystem.Common.Modules.Client;
using ModularSystem.Common.Modules.Server;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.BLL
{
    public class ModulesManager
    {
        public string ClientModulesStorePath { get; }
        public string ServerModulesStorePath { get; }

        private readonly Dictionary<ModuleIdentity, ServerSideClientModule> _clientModules = new Dictionary<ModuleIdentity, ServerSideClientModule>();
        private readonly Dictionary<ModuleIdentity, ServerModule> _serverModules = new Dictionary<ModuleIdentity, ServerModule>();

        public ModulesManager(string clientModulesStorePath, string serverModulesStorePath)
        {
            ClientModulesStorePath = clientModulesStorePath;
            ServerModulesStorePath = serverModulesStorePath;

            FileSystemHelpers.ClearOrCreateDir(ClientModulesStorePath);
            FileSystemHelpers.ClearOrCreateDir(ServerModulesStorePath);
        }

        public void InstallBatch(ZipBatchedModules batch, bool startServerModules = true)
        {
            MemoryPackedModule[] innerModules;
            batch.UnbatchModules(out innerModules);
            foreach (var memoryPackedModule in innerModules)
            {
                switch (memoryPackedModule.ModuleType)
                {
                    case ModuleType.Client:
                        var clientModule = InstallClientModule(memoryPackedModule);
                        _clientModules.Add(clientModule.ModuleIdentity, clientModule);
                        break;
                    case ModuleType.Server:
                        var serverModule = InstallServerModule(memoryPackedModule);
                        _serverModules.Add(serverModule.ModuleIdentity, serverModule);
                        if (startServerModules)
                            serverModule.Start();
                        break;
                }
            }
        }

        public ServerSideClientModule InstallClientModule(ZipPackedModule module)
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
            return new ServerSideClientModule(
                ModuleIdentity.Parse(moduleMeta.Identity), 
                moduleMeta.Dependencies.Select(ModuleIdentity.Parse).ToArray(), 
                moduleMeta.ClientTypes,
                packed);
        }

        public ServerModule InstallServerModule(ZipPackedModule module)
        {
            var moduleMeta = module.ExtractMetaFile();

            var modulePath = Path.Combine(ServerModulesStorePath, $"{moduleMeta.Identity}");
            module.UnpackToDirectory(modulePath);

            return new ServerModule(
                ModuleIdentity.Parse(moduleMeta.Identity), 
                moduleMeta.Dependencies.Select(ModuleIdentity.Parse).ToArray(),
                modulePath);
        }

        public ModuleIdentity[] GetInstalledModules()
        {
            return _serverModules.Values.Select(x => x.ModuleIdentity)
                .Concat(_clientModules.Values.Select(x => x.ModuleIdentity)).ToArray();
        }
    }
}

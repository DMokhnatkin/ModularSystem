using System.Linq;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.BLL
{
    public class ModulesManager
    {
        private readonly ClientModulesManager _clientModulesManager;
        private readonly ServerModulesManager _serverModulesManager;
        private readonly UserModulesManager _userModulesManager;

        public ModulesManager(ClientModulesManager clientModulesManager, ServerModulesManager serverModulesManager, UserModulesManager userModulesManager)
        {
            _clientModulesManager = clientModulesManager;
            _serverModulesManager = serverModulesManager;
            _userModulesManager = userModulesManager;
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
                        _clientModulesManager.InstallModule(memoryPackedModule);
                        break;
                    case ModuleType.Server:
                        var serverModule = _serverModulesManager.InstallModule(memoryPackedModule);
                        if (startServerModules)
                            serverModule.Start();
                        break;
                }
            }
        }

        public IBatchedModules ResolveClientModules(string userId, string clientType)
        {
            var clientModules = _userModulesManager.GetClientModulesForUser(userId, clientType);
            return BatchHelper.BatchModulesToMemory(clientModules.Select(x => x.Packed));
        }

        public ModuleIdentity[] GetInstalledList()
        {
            return _serverModulesManager.GetInstalledModules()
                .Concat(_clientModulesManager.GetListOfInstalledModules()).ToArray();
        }
    }
}

using System.Collections.Generic;
using ModularSystem.Common.Modules.Client;
using ModularSystem.Common.PackedModules;

namespace ModularSystem.Common.BLL
{
    public class UserModulesManager
    {
        private readonly Dictionary<(string, string), List<ServerSideClientModule>> _userModules = new Dictionary<(string, string), List<ServerSideClientModule>>();

        public void RegisterModulesForUser(string userId, string clientType, IEnumerable<ServerSideClientModule> modules)
        {
            if (!_userModules.ContainsKey((userId, clientType)))
                _userModules.Add((userId, clientType), new List<ServerSideClientModule>());
            foreach (var serverSideClientModule in modules)
            {
                _userModules[(userId, clientType)].Add(serverSideClientModule);
            }
        }

        public ServerSideClientModule[] GetClientModulesForUser(string userId, string clientType)
        {
            return _userModules[(userId, clientType)].ToArray();
        }
    }
}

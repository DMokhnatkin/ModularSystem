using System;
using System.Collections.Generic;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules.Client;

namespace ModularSystem.Common.Repositories.UserModules
{
    public class MemoryUserModulesRepository : IUserModulesRepository
    {
        private readonly Dictionary<(string userId , string clientType), HashSet<ModuleIdentity>> _userModules = new Dictionary<(string, string), HashSet<ModuleIdentity>>();

        public void AddModule(string userId, string clientId, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey((userId, clientId)))
                _userModules[(userId, clientId)] = new HashSet<ModuleIdentity>();
            _userModules[(userId, clientId)].Add(module);
        }

        public void RemoveModule(string userId, string clientId, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey((userId, clientId)) || !_userModules[(userId, clientId)].Contains(module))
                throw new ModuleMissedException(module, $"{module} isn't in requirments of user {userId}");
            _userModules[(userId, clientId)].Remove(module);
        }

        public IEnumerable<ModuleIdentity> GetModules(string userId, string clientId)
        {
            if (_userModules.ContainsKey((userId, clientId)))
                return _userModules[(userId, clientId)];
            return null;
        }

        public IEnumerable<(string userId, string clientType, ModuleIdentity identity)> GetAll()
        {
            foreach (var t in _userModules)
            {
                foreach (var x in t.Value)
                {
                    yield return (t.Key.userId, t.Key.clientType, x);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules.Client;

namespace ModularSystem.Common.Repositories.UserModules
{
    public class MemoryUserModulesRepository : IUserModulesRepository
    {
        private readonly Dictionary<(string userId , string clientType), HashSet<ModuleIdentity>> _userModules = new Dictionary<(string, string), HashSet<ModuleIdentity>>();

        public void AddModule(string userId, string clientType, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey((userId, clientType)))
                _userModules[(userId, clientType)] = new HashSet<ModuleIdentity>();
            _userModules[(userId, clientType)].Add(module);
        }

        public void RemoveModule(string userId, string clientType, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey((userId, clientType)) || !_userModules[(userId, clientType)].Contains(module))
                throw new ModuleMissedException(module, $"{module} isn't in requirments of user {userId}");
            _userModules[(userId, clientType)].Remove(module);
        }

        public IEnumerable<ModuleIdentity> GetModules(string userId, string clientType)
        {
            if (_userModules.ContainsKey((userId, clientType)))
                return _userModules[(userId, clientType)];
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

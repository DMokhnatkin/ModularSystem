using System.Collections.Generic;
using ModularSystem.Common.Exceptions;

namespace ModularSystem.Common.Repositories
{
    public class MemoryUserModulesRepository : IUserModulesRepository
    {
        private readonly Dictionary<(string, string), HashSet<ModuleIdentity>> _userModules = new Dictionary<(string, string), HashSet<ModuleIdentity>>();

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
    }
}

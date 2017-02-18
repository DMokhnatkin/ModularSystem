using System.Collections.Generic;
using ModularSystem.Common.Exceptions;

namespace ModularSystem.Common.Repositories
{
    public class MemoryUserModulesRepository : IUserModulesRepository
    {
        private readonly Dictionary<string, HashSet<ModuleIdentity>> _userModules = new Dictionary<string, HashSet<ModuleIdentity>>();

        public void AddModule(string userId, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey(userId))
                _userModules[userId] = new HashSet<ModuleIdentity>();
            _userModules[userId].Add(module);
        }

        public void RemoveModule(string userId, ModuleIdentity module)
        {
            if (!_userModules.ContainsKey(userId) || !_userModules[userId].Contains(module))
                throw new ModuleMissedException(module, $"{module} isn't in requirments of user {userId}");
            _userModules[userId].Remove(module);
        }

        public IEnumerable<ModuleIdentity> GetModules(string userId)
        {
            if (_userModules.ContainsKey(userId))
                return _userModules[userId];
            return null;
        }
    }
}

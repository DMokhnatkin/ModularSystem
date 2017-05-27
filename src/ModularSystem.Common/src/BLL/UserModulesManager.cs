using System.Collections.Generic;
using ModularSystem.Common.Repositories.UserModules;

namespace ModularSystem.Common.BLL
{
    public class UserModulesManager
    {
        // Persistent storage for required list of modules for users.
        private readonly IUserModulesRepository _userModulesRepository;

        // Cache over repository
        private Dictionary<(string, string), List<ModuleIdentity>> _userModules;

        public UserModulesManager(IUserModulesRepository userModulesRepository)
        {
            _userModulesRepository = userModulesRepository;

            InitializeFromRepository();
        }

        private void InitializeFromRepository()
        {
            _userModules = new Dictionary<(string, string), List<ModuleIdentity>>();
            foreach (var t in _userModulesRepository.GetAll())
            {
                if (!_userModules.ContainsKey((t.userId, t.clientType)))
                    _userModules.Add((t.userId, t.clientType), new List<ModuleIdentity>());
                _userModules[(t.userId, t.clientType)].Add(t.identity);
            }
        }

        public void RegisterModulesForUser(string userId, string clientType, IEnumerable<ModuleIdentity> modules)
        {
            if (!_userModules.ContainsKey((userId, clientType)))
            {
                _userModules.Add((userId, clientType), new List<ModuleIdentity>());
            }
            foreach (var moduleIdentity in modules)
            {
                _userModulesRepository.AddModule(userId, clientType, moduleIdentity);
                _userModules[(userId, clientType)].Add(moduleIdentity);
            }
        }

        public ModuleIdentity[] GetClientModulesForUser(string userId, string clientType)
        {
            return _userModules[(userId, clientType)].ToArray();
        }
    }
}

using System.Collections.Generic;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Repositories
{
    public interface IUserModulesRepository
    {
        void AddModule(string userId, string clientId, ModuleIdentity module);
        void RemoveModule(string userId, string clientId, ModuleIdentity module);
        IEnumerable<ModuleIdentity> GetModules(string userId, string clientId);
        bool Contains(string userId, string clientId, ModuleIdentity module);
    }
}
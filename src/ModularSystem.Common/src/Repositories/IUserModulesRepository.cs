using System.Collections.Generic;

namespace ModularSystem.Common.Repositories
{
    public interface IUserModulesRepository
    {
        void AddModule(string userId, ModuleIdentity module);
        void RemoveModule(string userId, ModuleIdentity module);
        IEnumerable<ModuleIdentity> GetModules(string userId);
    }
}
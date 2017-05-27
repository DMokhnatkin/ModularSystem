using System.Collections.Generic;

namespace ModularSystem.Common.Repositories.UserModules
{
    public interface IUserModulesRepository
    {
        void AddModule(string userId, string clientType, ModuleIdentity module);
        void RemoveModule(string userId, string clientType, ModuleIdentity module);
        IEnumerable<ModuleIdentity> GetModules(string userId, string clientType);

        /// <summary>
        /// Get all from repository
        /// </summary>
        IEnumerable<(string userId, string clientType, ModuleIdentity identity)> GetAll();
    }
}
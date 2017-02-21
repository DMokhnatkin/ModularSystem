using System.Collections.Generic;
using System.Linq;
using System.Net;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Сonfigurator.Proxies;

namespace ModularSystem.Сonfigurator.BLL
{
    public class HttpModules
    {
        private readonly ModulesProxy _proxy;

        public HttpModules(ModulesProxy proxy)
        {
            _proxy = proxy;
        }

        public bool InstallModule(IModule module)
        {
            return _proxy.InstallModuleAsync(module.Wrap().Result).Result.StatusCode == HttpStatusCode.OK;
        }

        public bool RemoveModule(ModuleIdentity module)
        {
            return _proxy.RemoveModuleAsync(module.Wrap()).Result.StatusCode == HttpStatusCode.OK;
        }

        public IEnumerable<ModuleIdentity> GetListOfModules()
        {
            return _proxy.GetModulesListAsync().Result.Select(x => x.Unwrap());
        }
    }
}

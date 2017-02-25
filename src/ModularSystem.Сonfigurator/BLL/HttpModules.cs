using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public HttpResponseMessage InstallModulePackage(FileStream package)
        {
            return _proxy.InstallModulePackageAsync(package).Result;
        }

        public HttpResponseMessage RemoveModule(ModuleIdentity module)
        {
            return _proxy.RemoveModuleAsync(module.Wrap()).Result;
        }

        public IEnumerable<ModuleIdentity> GetListOfModules()
        {
            return _proxy.GetModulesListAsync().Result.Select(x => x.Unwrap());
        }

        public HttpResponseMessage AddUserModules(IEnumerable<ModuleIdentity> moduleIdentities)
        {
            return _proxy.AddUserModules(moduleIdentities.Select(x => x.Wrap()).ToArray()).Result;
        }

        public IEnumerable<ModuleIdentity> GetUserModules(string userId)
        {
            return _proxy.GetUserModules(userId).Result.Select(x => x.Unwrap());
        }
    }
}

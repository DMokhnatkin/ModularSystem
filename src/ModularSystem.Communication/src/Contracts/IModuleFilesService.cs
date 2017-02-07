using System.ServiceModel;
using System.Threading.Tasks;
using ModularSystem.Communication.Data;

namespace ModularSystem.Communication.Contracts
{
    [ServiceContract]
    public interface IModuleFilesService
    {
        [OperationContract]
        Task<IDownloadModulesResponse> DownloadModules(IDownloadModulesRequest request);
    }
}

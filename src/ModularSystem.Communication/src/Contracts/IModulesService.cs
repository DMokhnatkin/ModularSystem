using System.ServiceModel;
using System.Threading.Tasks;
using ModularSystem.Communication.Data;

namespace ModularSystem.Communication.Contracts
{
    [ServiceContract]
    public interface IModulesService
    {
        [OperationContract]
        Task<ResolveResponse> ResolveAsync(ResolveRequest req);
    }
}

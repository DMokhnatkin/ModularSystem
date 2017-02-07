using System.Collections.Generic;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public interface IResolveRequest
    {
        IEnumerable<ModuleIdentity> ModuleIdentities { get; }
    }
}
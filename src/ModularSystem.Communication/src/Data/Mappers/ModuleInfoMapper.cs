using System.Linq;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Communication.Data.Mappers
{
    public static class ModuleInfoMapper
    {
        public static ModuleInfoDto Wrap(this ModuleInfo obj)
        {
            return new ModuleInfoDto
            {
                Dependencies = obj.Dependencies.Select(x => ModuleIdentityMapper.Wrap(x)).ToArray(),
                ModuleIdentity = obj.ModuleIdentity.Wrap()
            };
        }

        public static ModuleInfo Unwrap(this ModuleInfoDto dto)
        {
            return new ModuleInfo(dto.ModuleIdentity.Unwrap(), dto.Dependencies.Select(x => x.Unwrap()).ToArray());
        }
    }
}

using System.Linq;
using ModularSystem.Common.Transfer.Dto;

namespace ModularSystem.Common.Transfer.Mappers
{
    public static class ModuleInfoMapper
    {
        public static ModuleInfoDto Wrap(this ModuleInfo obj)
        {
            return new ModuleInfoDto
            {
                Dependencies = obj.Dependencies.Select(x => x.ToString()).ToArray(),
                ModuleIdentity = obj.ModuleIdentity.ToString()
            };
        }

        public static ModuleInfo Unwrap(this ModuleInfoDto dto)
        {
            return new ModuleInfo(ModuleIdentity.Parse(dto.ModuleIdentity), dto.Dependencies.Select(ModuleIdentity.Parse).ToArray());
        }
    }
}

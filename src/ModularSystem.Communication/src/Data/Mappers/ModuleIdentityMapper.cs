using System;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Communication.Data.Mappers
{
    public static class ModuleIdentityMapper
    {
        public static ModuleIdentityDto Wrap(this ModuleIdentity moduleIdentity)
        {
            return new ModuleIdentityDto
            {
                ModuleType = (byte) moduleIdentity.ModuleType,
                Name = moduleIdentity.Name,
                Version = moduleIdentity.Version.ToString()
            };
        }

        public static ModuleIdentity Unwrap(this ModuleIdentityDto dto)
        {
            return new ModuleIdentity(dto.Name, Version.Parse(dto.Version), (ModuleType)dto.ModuleType);
        }
    }
}

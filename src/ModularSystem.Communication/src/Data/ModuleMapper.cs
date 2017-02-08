using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public static class ModuleMapper
    {
        public static ModuleDto Wrap(this IModule module)
        {
            return new ModuleDto
            {
                ModuleInfo = module.ModuleInfo,
                Data = module.Data
            };
        }
    }
}

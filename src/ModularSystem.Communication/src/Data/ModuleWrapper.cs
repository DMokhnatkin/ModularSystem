using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public static class ModuleWrapper
    {
        public static ModuleDto Wrap(this IModule module)
        {
            return new ModuleDto
            {
                ModuleInfo = module.ModuleInfo
            };
        }
    }
}

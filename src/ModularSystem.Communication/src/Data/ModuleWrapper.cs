using System.Linq;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public static class ModuleWrapper
    {
        public const string NameVersionSeparator = @"\0";

        public static ModuleDto Wrap(this IModule module)
        {
            return new ModuleDto
            {
                ModuleIdentity = new ModuleIdentity(module.ModuleInfo.Name, module.ModuleInfo.Version),
                Dependencies = module.ModuleInfo.Dependencies.Select(x => $"{x.Name}{NameVersionSeparator}{x.Version}").ToArray()
            };
        }
    }
}

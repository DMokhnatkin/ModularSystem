using System.IO;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Communication.Data.Mappers
{
    public static class ModuleMapper
    {
        public static async Task<ModuleDto> Wrap(this IModule module)
        {
            ModuleDto res = new ModuleDto
            {
                ModuleInfo = module.ModuleInfo.Wrap()
            };
            // PERFOMANCE: now we read stream every time when wrap object. Maybe store byte[] as cache?
            using (MemoryStream writer = new MemoryStream())
            {
                await module.Data.CopyToAsync(writer);
                res.Data = writer.ToArray();
            }
            return res;
        }

        public static async Task<IModule> Unwrap(this ModuleDto module)
        {
            Module res = new Module()
            {
                ModuleInfo = module.ModuleInfo.Unwrap(),
                Data = new MemoryStream(module.Data)
            };
            return res;
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Common.Modules;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Communication.Data.Mappers
{
    public static class ModuleMapper
    {
        public static async Task<ModuleDto> Wrap(this IPackagedModule packagedModule)
        {
            ModuleDto res = new ModuleDto
            {
                ModuleInfo = packagedModule.ModuleInfo.Wrap()
            };
            // PERFOMANCE: now we read stream every time when wrap object. Maybe store byte[] as cache?
            using (MemoryStream writer = new MemoryStream())
            {
                await packagedModule.Data.CopyToAsync(writer);
                res.Data = writer.ToArray();
            }
            return res;
        }

        public static async Task<IPackagedModule> Unwrap(this ModuleDto module)
        {
            ZipPackagedModule res = new ZipPackagedModule()
            {
                ModuleInfo = module.ModuleInfo.Unwrap(),
                Data = new MemoryStream(module.Data)
            };
            return res;
        }
    }
}

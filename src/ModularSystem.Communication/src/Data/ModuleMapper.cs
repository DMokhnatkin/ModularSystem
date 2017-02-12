using System.IO;
using System.Threading.Tasks;
using ModularSystem.Common;

namespace ModularSystem.Communication.Data
{
    public static class ModuleMapper
    {
        public static async Task<ModuleDto> Wrap(this IModule module)
        {
            ModuleDto res = new ModuleDto
            {
                ModuleInfo = module.ModuleInfo
            };
            // PERFOMANCE: now we read stream every time when wrap object. Maybe store byte[] as cache?
            using (MemoryStream writer = new MemoryStream())
            {
                await module.Data.CopyToAsync(writer);
                res.Data = writer.ToArray();
            }
            return res;
        }
    }
}

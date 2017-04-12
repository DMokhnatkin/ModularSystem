using System.IO;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.PackedModules.IO
{
    public static class PackedModuleIO
    {
        public static FilePackedModuleV2 WriteToFile(this IPackedModuleV2 module, string filePath)
        {
            using (var moduleStream = module.OpenReadStream())
            using (var fileStream = File.OpenWrite(filePath))
            {
                moduleStream.CopyTo(fileStream);
            }
            return new FilePackedModuleV2(filePath);
        }

        public static void ReadFromFile(string filePath, out MemoryPackedModuleV2 module)
        {
            module = new MemoryPackedModuleV2(File.ReadAllBytes(filePath));
        }

        public static void ReadFromFile(string filePath, out FilePackedModuleV2 module)
        {
            module = new FilePackedModuleV2(filePath);
        }
    }
}

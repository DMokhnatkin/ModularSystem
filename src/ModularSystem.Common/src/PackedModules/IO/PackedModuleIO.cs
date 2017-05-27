using System.IO;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.PackedModules.IO
{
    public static class PackedModuleIO
    {
        public static FilePackedModuleInfo WriteToFile(this IPacked module, string filePath)
        {
            using (var moduleStream = module.OpenReadStream())
            using (var fileStream = File.OpenWrite(filePath))
            {
                moduleStream.CopyTo(fileStream);
            }
            return new FilePackedModuleInfo(filePath);
        }

        public static void ReadFromFile(string filePath, out MemoryPackedModuleInfo moduleInfo)
        {
            moduleInfo = new MemoryPackedModuleInfo(File.ReadAllBytes(filePath));
        }

        public static void ReadFromFile(string filePath, out FilePackedModuleInfo moduleInfo)
        {
            moduleInfo = new FilePackedModuleInfo(filePath);
        }
    }
}

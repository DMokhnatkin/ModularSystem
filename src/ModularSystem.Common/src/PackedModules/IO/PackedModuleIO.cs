using System.IO;
using ModularSystem.Common.PackedModules.Zip;

namespace ModularSystem.Common.PackedModules.IO
{
    public static class PackedModuleIO
    {
        public static FilePackedModule WriteToFile(this IPacked module, string filePath)
        {
            using (var moduleStream = module.OpenReadStream())
            using (var fileStream = File.OpenWrite(filePath))
            {
                moduleStream.CopyTo(fileStream);
            }
            return new FilePackedModule(filePath);
        }

        public static void ReadFromFile(string filePath, out MemoryPackedModule module)
        {
            module = new MemoryPackedModule(File.ReadAllBytes(filePath));
        }

        public static void ReadFromFile(string filePath, out FilePackedModule module)
        {
            module = new FilePackedModule(filePath);
        }
    }
}

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class IOHelpers
    {
        public static string GenerateFileName(this ZipPackedModuleInfo moduleInfo)
        {
            var meta =PackHelper.ExtractMetaFile(moduleInfo);
            return $"{meta.Identity}.zip";
        }
    }
}

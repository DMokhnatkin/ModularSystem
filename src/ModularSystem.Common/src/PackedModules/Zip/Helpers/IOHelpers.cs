namespace ModularSystem.Common.PackedModules.Zip
{
    public static class IOHelpers
    {
        public static string GenerateFileName(this ZipPackedModule module)
        {
            var meta =PackHelper.ExtractMetaFile(module);
            return $"{meta.Identity}.zip";
        }
    }
}

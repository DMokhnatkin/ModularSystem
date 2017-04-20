namespace ModularSystem.Common.PackedModules.Zip
{
    public static class IOHelpers
    {
        public static string GenerateFileName(this IZipPackedModule module)
        {
            var meta =PackHelper.ExtractMetaFile(module);
            return $"{meta.Identity}.zip";
        }
    }
}

namespace ModularSystem.Common.PackedModules.Zip
{
    public static class IOHelpers
    {
        public static string GenerateFileName(this IPackedModuleV2 module)
        {
            var meta =PackHelperV2.ExtractMetaFile(module);
            return $"{meta.Identity}.zip";
        }
    }
}

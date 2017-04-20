using System.IO;

namespace ModularSystem.Tests.Common.HelpersForTests
{
    public static class TestingIOHelpers
    {
        public static void DeleteFiles(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}

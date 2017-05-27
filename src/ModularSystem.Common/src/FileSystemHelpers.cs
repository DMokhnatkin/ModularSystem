using System.IO;

namespace ModularSystem.Common
{
    public static class FileSystemHelpers
    {
        /// <summary>
        /// Clear directory (if exists) or create (if not exists)
        /// </summary>
        public static void ClearOrCreateDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}

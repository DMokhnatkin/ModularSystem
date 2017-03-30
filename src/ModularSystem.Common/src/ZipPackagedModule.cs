using System.IO;
using System.IO.Compression;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common
{
    public class ZipPackagedModule : IModule, IPathStoredModule
    {
        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; internal set; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies { get; internal set; }

        internal ZipPackagedModule()
        { }

        /// <summary>
        /// Open meta files from archive. 
        /// </summary>
        public MetaFileWrapper ExtractMetaFile()
        {
            using (var z = new ZipArchive(File.OpenRead(Path)))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                return new MetaFileWrapper(metaFileStream);
            }
        }

        /// <summary>
        /// Update meta file in archive.
        /// </summary>
        public void UpdateMetaFile(MetaFileWrapper metaFile)
        {
            using (var z = new ZipArchive(File.OpenRead(Path)))
            using (var metaFileStream = z.GetEntry(MetaFileWrapper.DefaultFileName).Open())
            {
                metaFile.Write(metaFileStream);
            }
        }

        /// <summary>
        /// Initialize ZipPackagedModule from zip archive
        /// </summary>
        public static ZipPackagedModule InitializeFromZip(string path)
        {
            ZipPackagedModule r = new ZipPackagedModule();
            using (ZipArchive z = new ZipArchive(File.OpenRead(path)))
            {
                var t = new MetaFileWrapper(z.GetEntry(MetaFileWrapper.DefaultFileName).Open());
                r.ModuleIdentity = ModuleIdentity.Parse(t.Identity);
                r.Dependencies = t.Dependencies.Select(ModuleIdentity.Parse).ToArray();
            }
            r.Path = path;
            return r;
        }

        /// <summary>
        /// Pack all files in directory into zip and initialize ZipPackagedModule instance for it
        /// </summary>
        public static ZipPackagedModule PackFolder(string filesPath, string zipDestinationPath)
        {
            ZipFile.CreateFromDirectory(filesPath, zipDestinationPath);
            return InitializeFromZip(zipDestinationPath);
        }

        /// <summary>
        /// Unpack zip packaged module to destination directory.
        /// </summary>
        public void UnpackToFolder(string destFolder)
        {
            using (ZipArchive z = new ZipArchive(File.OpenRead(Path)))
            {
                z.ExtractToDirectory(destFolder);
            }
        }
    }
}

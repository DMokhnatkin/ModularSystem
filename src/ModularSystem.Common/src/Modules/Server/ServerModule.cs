using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using ModularSystem.Common.MetaFiles;

namespace ModularSystem.Common.Modules.Server
{
    public class ServerModule : IServerModule
    {
        private readonly MetaFileWrapper _metaFile;

        public ModuleIdentity ModuleIdentity { get; }
        public ModuleIdentity[] Dependencies { get; }
        public ModuleType ModuleType => ModuleType.Server;

        public string Path { get; set; }

        public static IConfigurationRoot Configuration { get; set; }

        public ServerModule(ModuleIdentity moduleIdentity, ModuleIdentity[] depeIdentities, string path)
        {
            ModuleIdentity = moduleIdentity;
            Dependencies = depeIdentities;
            Path = path;
            _metaFile = MetaFileWrapper.FindInDirectory(Path);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public void Start()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Path;
            info.FileName = Configuration["Deploy:PythonPath"];
            info.Arguments = $"{_metaFile.StartScript}";
            //info.UseShellExecute = false;
            using (Process process = Process.Start(info))
            { }
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}

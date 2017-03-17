using System;
using System.IO;
using System.IO.Compression;
using CommandLine;
using ModularSystem.Common;
using Newtonsoft.Json.Linq;

namespace PackPlugin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(PackWithConfigFile);
        }

        public static void PackWithConfigFile(Options o)
        {
            var conf = JObject.Parse(File.ReadAllText(o.ConfigFile));

            if (!Directory.Exists(o.OutputDir))
            {
                Directory.CreateDirectory(o.OutputDir);
            }
            else
            {
                // Clear dir
                Directory.Delete(o.OutputDir, true);
                Directory.CreateDirectory(o.OutputDir);
            }

            foreach (var module in conf["Modules"])
            {
                var modulePath = module.Value<string>();
                var tmpPath = Path.Combine(o.OutputDir, Path.GetRandomFileName());
                var t = ZipPackagedModule.PackFolder(modulePath, tmpPath);
                // Rename temp zip package to ModuleIdentity.ToString()
                File.Move(tmpPath, Path.Combine(o.OutputDir, $"{t.ModuleInfo.ModuleIdentity}.zip"));
            }

            var tmp = $"{Path.Combine(Path.GetTempPath(), conf["PackageName"].Value<string>())}.zip";
            ZipFile.CreateFromDirectory(o.OutputDir, tmp);
            Directory.Delete(o.OutputDir, true);
            Directory.CreateDirectory(o.OutputDir);
            File.Move(tmp, $"{Path.Combine(o.OutputDir, conf["PackageName"].Value<string>())}.zip");
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using CommandLine;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Сonfigurator.BLL;
using ModularSystem.Сonfigurator.InputOptions;
using ModularSystem.Сonfigurator.Proxies;

namespace ModularSystem.Сonfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);

            HttpModules modules = new HttpModules(new ModulesProxy("http://localhost:5005"));

            /*
            ModuleDto dto = new ModuleDto()
            {
                ModuleInfo = new ModuleInfoDto()
                {
                    ModuleIdentity = new ModuleIdentity("test", "1.0", ModuleType.Server).Wrap(),
                    Dependencies = new[] {new ModuleIdentity("test2", "1.0", ModuleType.Server).Wrap()}
                },
                Data = File.ReadAllBytes("t2.zip")
            };

            dto.WriteToDirectory("t3");*/

            while (true)
            {
                var s = ReadLine.Read(">");
                var command = s.Split(' ');

                Parser.Default
                    .ParseArguments<InstallOptions, RemoveOptions, ListOptions, ExitOptions>(command)
                    .WithParsed<InstallOptions>(
                        opts => Install(modules, opts))
                    .WithParsed<RemoveOptions>(
                        opts => Remove(modules, opts))
                    .WithParsed<ListOptions>(opts =>
                    {
                        var r = modules.GetListOfModules();
                        var moduleIdentities = r as ModuleIdentity[] ?? r.ToArray();
                        if (!moduleIdentities.Any())
                            Console.WriteLine("No modules are installed");
                        else
                        {
                            foreach (var moduleIdentity in moduleIdentities)
                            {
                                Console.WriteLine(" - " + moduleIdentity);
                            }
                        }
                    })
                    .WithParsed<ExitOptions>(opts => Environment.Exit(0));
            }
        }

        private static void Install(HttpModules modules, InstallOptions opts)
        {
            var r = modules.InstallModulePackage(File.OpenRead(opts.FilePath));
            Console.WriteLine(r.IsSuccessStatusCode ? "success" : $"error {r.StatusCode} : {r.Content.ReadAsStringAsync().Result}");
        }

        private static void Remove(HttpModules modules, RemoveOptions opts)
        {
            var r = modules.RemoveModule(ModuleIdentity.Parse($"{opts.Name} {opts.Version} {opts.Type}"));
            Console.WriteLine(r.IsSuccessStatusCode ? "success" : $"error {r.StatusCode} : {r.Content.ReadAsStringAsync().Result}");
        }
    }
}


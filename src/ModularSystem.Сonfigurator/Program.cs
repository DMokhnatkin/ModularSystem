using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using CommandLine;
using ModularSystem.Common;
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

            while (true)
            {
                var s = ReadLine.Read(">");
                var command = s.Split(' ');

                Parser.Default
                    .ParseArguments<InstallOptions, RemoveOptions, ListOptions, ExitOptions>(command)
                    .WithParsed<InstallOptions>(
                        opts => Install(modules, opts))
                    .WithParsed<RemoveOptions>(opts => { })
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
            Console.WriteLine(r.IsSuccessStatusCode ? "success" : $"error {r.StatusCode}");
        }
    }
}


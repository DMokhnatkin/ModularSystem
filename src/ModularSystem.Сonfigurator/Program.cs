using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
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
                    .ParseArguments<InstallOptions, RemoveOptions, ListOptions, ExitOptions, AddUserModulesOptions>(command)
                    .WithParsed<InstallOptions>(opts => Install(modules, opts))
                    .WithParsed<RemoveOptions>(opts => Remove(modules, opts))
                    .WithParsed<ListOptions>(opts =>
                    {
                        IEnumerable<ModuleIdentity> res = opts.UserId == null
                            ? modules.GetListOfModules()
                            : modules.GetUserModules(opts.UserId);
                        HandleEnumerableResult(res);
                    })
                    .WithParsed<AddUserModulesOptions>(opts => AddUserModules(modules, opts))
                    .WithParsed<ExitOptions>(opts => Environment.Exit(0));
            }
        }

        private static void Install(HttpModules modules, InstallOptions opts)
        {
            var r = modules.InstallModulePackage(File.OpenRead(opts.PackagePath));
            HandleResult(r);
        }

        private static void Remove(HttpModules modules, RemoveOptions opts)
        {
            var r = modules.RemoveModule(ModuleIdentity.Parse($"{opts.Name} {opts.Version} {opts.Type}"));
            HandleResult(r);
        }

        private static void AddUserModules(HttpModules modules, AddUserModulesOptions opts)
        {
            var moduleIdentities =
                opts.ModuleIdentities.Select(ModuleIdentity.Parse);
            var r = modules.AddUserModules(opts.UserId, moduleIdentities);
            HandleResult(r);
        }

        private static void HandleResult(HttpResponseMessage resp)
        {
            Console.WriteLine(resp.IsSuccessStatusCode ? "success" : $"error {resp.StatusCode} : {resp.Content.ReadAsStringAsync().Result}");
        }

        private static void HandleEnumerableResult<T>(IEnumerable<T> objs)
        {
            var enumerable = objs as T[] ?? objs.ToArray();
            if (!enumerable.Any())
                Console.WriteLine("Enumerable is empty");
            else
            {
                foreach (var obj in enumerable)
                {
                    Console.WriteLine(" * " + obj);
                }
            }
        }
    }
}


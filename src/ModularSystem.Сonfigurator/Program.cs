using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
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

            var proxy = new ModulesProxy("http://localhost:5005");
            HttpModules modules = new HttpModules(proxy);

            // Authenticate
            while (true)
            {
                var userName = ReadLine.Read("user name: ");
                var userPassword = new StringBuilder();
                Console.Write("user password: ");
                while (true)
                {
                    var c = Console.ReadKey(true);
                    if (c.Key == ConsoleKey.Enter)
                        break;
                    Console.Write('*');
                    userPassword.Append(c.KeyChar);
                }
                Console.WriteLine();
                if (proxy.GetTokenAsync("configurator", "g6wCBw", userName, userPassword.ToString(), "modules").Result)
                    break;
                Console.WriteLine("Authentication error");
            }

            while (true)
            {
                var s = ReadLine.Read(">");
                var command = s.Split(' ');

                Parser.Default
                    .ParseArguments<InstallOptions, RemoveOptions, ListOptions, ExitOptions, AddUserModulesOptions, RemoveUserModulesOptions>(command)
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
                    .WithParsed<RemoveUserModulesOptions>(opts => RemoveUserModules(modules, opts))
                    .WithParsed<ExitOptions>(opts => Environment.Exit(0));
            }
        }

        private static void Install(HttpModules modules, InstallOptions opts)
        {
            if (opts.PackagePath != null)
            {
                var r = modules.InstallModulePackage(File.OpenRead(opts.PackagePath));
                HandleResult(r);
            }
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
            var r = modules.AddUserModules(opts.UserId, opts.ClientId, moduleIdentities);
            HandleResult(r);
        }

        private static void RemoveUserModules(HttpModules modules, RemoveUserModulesOptions opts)
        {
            var moduleIdentities =
                opts.ModuleIdentities.Select(ModuleIdentity.Parse);
            var r = modules.RemoveUserModules(opts.UserId, opts.ClientId, moduleIdentities);
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


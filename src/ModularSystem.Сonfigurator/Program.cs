using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Сonfigurator.Proxies;

namespace ModularSystem.Сonfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            ModulesProxy proxy = new ModulesProxy("http://localhost:5005");

            Console.WriteLine("Type man to get help");

            do
            {
                var command = ReadLine.Read(">");
                switch (command)
                {
                    case "man":
                        PrintMan();
                        break;
                    case "install":
                        Console.WriteLine(InstallModule(proxy));
                        break;
                    case "list":
                        var r = GetListOfModules(proxy);
                        if (!r.Any())
                            Console.WriteLine("No modules are installed");
                        else
                        {
                            foreach (var moduleIdentity in r)
                            {
                                Console.WriteLine(" - " + moduleIdentity);
                            }
                        }
                        break;
                    case "exit":
                        return;
                }
            }
            while (true);
        }

        static HttpResponseMessage InstallModule(ModulesProxy proxy)
        {
            ModuleDto dto = new ModuleDto()
            {
                ModuleInfo = new ModuleInfoDto()
                {
                    ModuleIdentity = new ModuleIdentityDto()
                    {
                        ModuleType = 0,
                        Name = "test3",
                        Version = (new Version(1, 0)).ToString()
                    },
                    Dependencies = new ModuleIdentityDto[0]
                },
                Data = new byte[] { 1, 2, 3, 4, 5 }
            };
            return proxy.InstallModuleAsync(dto).Result;
        }

        static IEnumerable<ModuleIdentity> GetListOfModules(ModulesProxy proxy)
        {
            return proxy.GetModulesListAsync().Result.Select(x => x.Unwrap());
        }

        static void PrintMan()
        {
            Console.WriteLine("man for configurator");
            Console.WriteLine("* install : install module");
            Console.WriteLine("* list : get list of installed modules");
            Console.WriteLine("* exit : exit from configurator");
        } 
    }
}

using System;
using System.IO;
using System.Threading;
using ModularSystem.Common;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Data.Mappers;
using ModularSystem.Сonfigurator.Proxies;

namespace ModularSystem.Сonfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(2000);
            ModulesProxy proxy = new ModulesProxy();
            Module test = new Module()
            {
                Data = new MemoryStream(new byte[] {0, 0, 0, 1, 2, 3, 4, 56, 7}),
                ModuleInfo = new ModuleInfo(new ModuleIdentity("mytest", new Version(1, 0), ModuleType.Server), new ModuleIdentity[0])
            };
            var res = proxy.InstallModuleAsync(test.Wrap().Result).Result;
            Console.ReadLine();
        }
    }
}

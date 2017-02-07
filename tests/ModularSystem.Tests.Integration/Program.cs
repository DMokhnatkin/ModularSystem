
using System;
using System.Linq;
using System.ServiceModel;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Proxies;
using ModularSystem.Server.Services;

namespace ModularSystem.Tests.Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ModulesService), new Uri(@"http://localhost:80/Temporary_Listen_Addresses"));
            host.Open();
            ModulesServiceProxy proxy = new ModulesServiceProxy(new BasicHttpBinding(), new EndpointAddress(@"http://localhost:80/Temporary_Listen_Addresses"));
            var res = proxy.ResolveAsync(new ResolveRequest()).Result;
            if (res.ModuleIdentities.Count() != 0)
                throw new ArgumentException();
            host.Close();
        }
    }
}

using System;
using System.ServiceModel;
using ModularSystem.Server.Services;
using NUnit.Framework;

namespace ModularSystem.Tests.Server
{
    [TestFixture]
    public class CommunicationTest
    {
        [Test]
        public void StartResolveService()
        {
            try
            {
                ServiceHost host = new ServiceHost(typeof(ModulesService), new Uri(@"http://localhost:80/Temporary_Listen_Addresses"));
                host.Open();
                host.Close();
            }
            catch (Exception e)
            {
                Assert.Fail($"{e}");
            }
        }
    }
}

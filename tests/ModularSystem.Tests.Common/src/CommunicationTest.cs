using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularSystem.Server.Services;
using Moq;

namespace ModularSystem.Tests.Common
{
    [TestClass]
    public class CommunicationTest
    {
        [TestMethod]
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

using System;
using System.IO;
using ModularSystem.Common;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.Conf
{
    [TestFixture]
    public class ModuleConfTest
    {
        [Test]
        public void TestLoad()
        {
            var t = ModuleConf.LoadFromFile(File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/ConfFiles/conf1.json")));
            Assert.AreEqual(t.ModuleIdentity, "test-server-1.0");

            Assert.Contains("test-client-1.1", t.Dependencies);
            Assert.Contains("test2-server-1.5", t.Dependencies);
            Assert.Contains("test4-client-1.7", t.Dependencies);
        }

        [Test]
        public void TestWrite()
        {
            var t = new ModuleConf()
            {
                ModuleIdentity = "test-server-1.0",
                Dependencies = new [] { "test-client-1.1", "test2-server-1.5", "test4-client-1.7"}
            };
            var s = Path.GetTempFileName();
            using (var fs = File.CreateText(s))
            {
                t.WriteToFile(fs);
            }

            using (var fs = File.OpenRead(s))
            {
                var y = ModuleConf.LoadFromFile(File.OpenRead(s));
                Assert.AreEqual(y.ModuleIdentity, "test-server-1.0");
                Assert.Contains("test-client-1.1", y.Dependencies);
                Assert.Contains("test2-server-1.5", y.Dependencies);
                Assert.Contains("test4-client-1.7", y.Dependencies);
            }
        }
    }
}

using System;
using System.IO;
using ModularSystem.Common;
using ModularSystem.Common.MetaFiles;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.Conf
{
    [TestFixture]
    public class ModuleMetaTest
    {
        [Test]
        public void TestLoad()
        {
            var t = new MetaFileWrapper(File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/ModuleMetaFiles/meta.json")));

            Assert.AreEqual("my.test-1.0", t.Identity);
            Assert.AreEqual("client", t.Type);

            Assert.Contains("test.client.wpf-1.1", t.ClientDependencies);
            Assert.Contains("test4.client.wpf-1.7", t.ClientDependencies);

            Assert.Contains("test2.server-1.5", t.ServerDependencies);
            Assert.Contains("test3.server-2.5", t.ServerDependencies);
        }

        [Test]
        public void TestWrite()
        {
            var t = new MetaFileWrapper
            {
                Identity = "my.test-1.0",
                Type = "test",
                ClientDependencies = new [] { "test-client-1.1", "test4-client-1.7" },
                ServerDependencies = new [] { "test2-server-1.5", "test3-server-1.6" }
            };
            var s = Path.GetTempFileName();
            t.Write(s);

            using (var fs = File.OpenRead(s))
            {
                var y = new MetaFileWrapper(File.OpenRead(s));

                Assert.AreEqual("my.test-1.0", y.Identity);
                Assert.AreEqual("test", y.Type);

                Assert.Contains("test-client-1.1", y.ClientDependencies);
                Assert.Contains("test4-client-1.7", y.ClientDependencies);

                Assert.Contains("test2-server-1.5", y.ServerDependencies);
                Assert.Contains("test3-server-1.6", y.ServerDependencies);
            }
        }

        [Test]
        public void TestNullProps()
        {
            var t = new MetaFileWrapper();
            Assert.AreEqual(null, t.Identity);
            Assert.AreEqual(null, t.Type);
            Assert.AreEqual(null, t.ClientDependencies);
            Assert.AreEqual(null, t.ServerDependencies);
        }
    }
}

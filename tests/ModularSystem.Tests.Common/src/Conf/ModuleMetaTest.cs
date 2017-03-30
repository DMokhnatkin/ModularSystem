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

            Assert.Contains("test.client.wpf-1.1", t.Dependencies);
            Assert.Contains("test2.server-1.5", t.Dependencies);
            Assert.Contains("test4.client.wpf-1.7", t.Dependencies);
        }

        [Test]
        public void TestWrite()
        {
            var t = new MetaFileWrapper
            {
                Type = "test",
                Dependencies = new [] { "test-client-1.1", "test2-server-1.5", "test4-client-1.7"}
            };
            var s = Path.GetTempFileName();
            t.Write(s);

            using (var fs = File.OpenRead(s))
            {
                var y = new MetaFileWrapper(File.OpenRead(s));
                Assert.AreEqual(y.Type, "test");
                Assert.Contains("test-client-1.1", y.Dependencies);
                Assert.Contains("test2-server-1.5", y.Dependencies);
                Assert.Contains("test4-client-1.7", y.Dependencies);
            }
        }

        [Test]
        public void TestNullProps()
        {
            var t = new MetaFileWrapper();
            Assert.AreEqual(null, t.Dependencies);
            Assert.AreEqual(null, t.Type);
        }
    }
}

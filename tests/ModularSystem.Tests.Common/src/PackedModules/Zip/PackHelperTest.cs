using System;
using System.IO;
using ModularSystem.Common;
using ModularSystem.Common.PackedModules.Zip;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.PackedModules.Zip
{
    [TestFixture]
    public class PackHelperTest
    {
        private static readonly string TestModule1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/Modules/module1-1.3.0");

        [Test]
        public void PackModuleToMemoryTest()
        {
            var r = PackHelper.PackModuleToMemory(TestModule1Path);
            Assert.AreEqual(r.ModuleIdentity.Name, "module1");
            Assert.AreEqual(r.ModuleIdentity.Version, new Version(1, 3, 0));
            Assert.Contains(new ModuleIdentity("test.client.wpf", "1.1"), r.Dependencies);
            Assert.Contains(new ModuleIdentity("test2.server", "1.5"), r.Dependencies);
            Assert.Contains(new ModuleIdentity("test4.client.wpf", "1.7"), r.Dependencies);
        }

        [Test]
        public void PackModuleToFileTest()
        {
            var tmp = Path.GetTempPath();
            var r = PackHelper.PackModuleToFile(TestModule1Path, tmp);
            Assert.AreEqual(r.ModuleIdentity.Name, "module1");
            Assert.AreEqual(r.ModuleIdentity.Version, new Version(1, 3, 0));
            Assert.Contains(new ModuleIdentity("test.client.wpf", "1.1"), r.Dependencies);
            Assert.Contains(new ModuleIdentity("test2.server", "1.5"), r.Dependencies);
            Assert.Contains(new ModuleIdentity("test4.client.wpf", "1.7"), r.Dependencies);
        }

        [Test]
        public void UnpackModule()
        {
            throw new NotImplementedException();
        }
    }
}

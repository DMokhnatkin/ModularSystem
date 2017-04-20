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
            MemoryPackedModule mp;
            PackHelper.PackModule(TestModule1Path, out mp);
            Assert.AreEqual(mp.ModuleIdentity.Name, "module1");
            Assert.AreEqual(mp.ModuleIdentity.Version, new Version(1, 3, 0));
            Assert.Contains(new ModuleIdentity("test.client.wpf", "1.1"), mp.Dependencies);
            Assert.Contains(new ModuleIdentity("test2.server", "1.5"), mp.Dependencies);
            Assert.Contains(new ModuleIdentity("test4.client.wpf", "1.7"), mp.Dependencies);
        }

        [Test]
        public void PackModuleToFileTest()
        {
            MemoryPackedModule mp;
            PackHelper.PackModule(TestModule1Path, out mp);
            Assert.AreEqual(mp.ModuleIdentity.Name, "module1");
            Assert.AreEqual(mp.ModuleIdentity.Version, new Version(1, 3, 0));
            Assert.Contains(new ModuleIdentity("test.client.wpf", "1.1"), mp.Dependencies);
            Assert.Contains(new ModuleIdentity("test2.server", "1.5"), mp.Dependencies);
            Assert.Contains(new ModuleIdentity("test4.client.wpf", "1.7"), mp.Dependencies);
        }

        [Test]
        public void UnpackModule()
        {
            throw new NotImplementedException();
        }
    }
}

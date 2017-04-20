using System;
using System.IO;
using System.Linq;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Tests.Common.HelpersForTests;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.PackedModules.Zip
{
    [TestFixture]
    public class PackHelperTest
    {
        private static readonly string TestModule1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/Modules/module1-1.3.0");
        private static readonly string TestModule2Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/Modules/module2-1.1.2");

        [Test]
        public void PackModuleToMemoryTest()
        {
            MemoryPackedModule m;
            PackHelper.PackModule(TestModule1Path, out m);
            // Test if module packed succes (i don't know how)

            Assert.Throws<FileNotFoundException>(() => PackHelper.PackModule(TestModule2Path, out m));
        }

        [Test]
        public void PackModuleToFileTest()
        {
            TestingIOHelpers.DeleteFiles("test");
            FilePackedModule m;
            PackHelper.PackModule(TestModule1Path, "test", out m);
            // Test if module packed succes (i don't know how)

            TestingIOHelpers.DeleteFiles("test");
            Assert.Throws<FileNotFoundException>(() => PackHelper.PackModule(TestModule2Path, "test", out m));
        }

        [Test]
        public void ExtractMetaFileTest()
        {
            // MemoryPackedModule
            MemoryPackedModule m;
            PackHelper.PackModule(TestModule1Path, out m);

            var r = m.ExtractMetaFile();
            Assert.AreEqual(r.Identity, "module1-1.3.0");
            Assert.AreEqual(r.Type, "wpfclient");
            Assert.IsTrue(r.Dependencies.SequenceEqual(new[] {"test.client.wpf-1.1", "test2.server-1.5", "test4.client.wpf-1.7"}));

            // FilePackedModule
            TestingIOHelpers.DeleteFiles("test");
            FilePackedModule m2;
            PackHelper.PackModule(TestModule1Path, "test", out m2);

            var r2 = PackHelper.ExtractMetaFile(m2);
            Assert.AreEqual(r2.Identity, "module1-1.3.0");
            Assert.AreEqual(r2.Type, "wpfclient");
            Assert.IsTrue(r2.Dependencies.SequenceEqual(new[] { "test.client.wpf-1.1", "test2.server-1.5", "test4.client.wpf-1.7" }));
        }

        [Test]
        public void UpdateMetaFileTest()
        {
            var newMetaFile = new MetaFileWrapper
            {
                Identity = "test0",
                Dependencies = new[] {"test1", "test2"},
                Type = "test3"
            };

            // MemoryPackedModule
            MemoryPackedModule m;
            PackHelper.PackModule(TestModule1Path, out m);

            PackHelper.UpdateMetaFile(m, newMetaFile);
            var r = PackHelper.ExtractMetaFile(m);
            Assert.AreEqual(r.Identity, newMetaFile.Identity);
            Assert.AreEqual(r.Type, newMetaFile.Type);
            Assert.IsTrue(r.Dependencies.SequenceEqual(newMetaFile.Dependencies));

            // FilePackedModule
            TestingIOHelpers.DeleteFiles("test");
            FilePackedModule m2;
            PackHelper.PackModule(TestModule1Path, "test", out m2);

            PackHelper.UpdateMetaFile(m2, newMetaFile);
            var r2 = PackHelper.ExtractMetaFile(m2);
            Assert.AreEqual(r2.Identity, newMetaFile.Identity);
            Assert.AreEqual(r2.Type, newMetaFile.Type);
            Assert.IsTrue(r2.Dependencies.SequenceEqual(newMetaFile.Dependencies));
        }
    }
}

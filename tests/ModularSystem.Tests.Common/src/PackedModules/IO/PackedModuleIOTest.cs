using System.IO;
using System.Linq;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.IO;
using ModularSystem.Tests.Common.HelpersForTests;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.PackedModules.IO
{
    [TestFixture]
    public class PackedModuleIOTest
    {
        private readonly byte[] _sampleData = {1, 2, 3, 4, 5};

        [Test]
        public void WriteToFileTest()
        {
            // MemoryPackedModule
            TestingIOHelpers.DeleteFiles("test");
            new MemoryPackedModuleV2(_sampleData).WriteToFile("test");
            Assert.IsTrue(_sampleData.SequenceEqual(File.ReadAllBytes("test")));

            // FilePackedModule
            TestingIOHelpers.DeleteFiles("test", "test2");
            File.WriteAllBytes("test", _sampleData);
            new FilePackedModuleV2("test").WriteToFile("test2");
            Assert.IsTrue(_sampleData.SequenceEqual(File.ReadAllBytes("test2")));
        }

        [Test]
        public void ReadFromFile_MemoryPackedTest()
        {
            TestingIOHelpers.DeleteFiles("test");
            new MemoryPackedModuleV2(_sampleData).WriteToFile("test");

            MemoryPackedModuleV2 m;
            PackedModuleIO.ReadFromFile("test", out m);
            using (var resStream = m.OpenReadStream())
            using (var br = new BinaryReader(resStream))
            {
                Assert.IsTrue(_sampleData.SequenceEqual(br.ReadBytes((int)resStream.Length)));
            }
        }

        [Test]
        public void ReadFromFile_FilePackedTest()
        {
            TestingIOHelpers.DeleteFiles("test", "test2");
            File.WriteAllBytes("test", _sampleData);
            new FilePackedModuleV2("test").WriteToFile("test2");

            FilePackedModuleV2 m;
            PackedModuleIO.ReadFromFile("test2", out m);
            using (var resStream = m.OpenReadStream())
            using (var br = new BinaryReader(resStream))
            {
                Assert.IsTrue(_sampleData.SequenceEqual(br.ReadBytes((int)resStream.Length)));
            }
        }
    }
}

using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.Repositories;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.Repositories
{
    [TestFixture]
    public abstract class BaseIUserModulesRepositoryTest
    {
        protected IUserModulesRepository UserModulesRepository;

        protected ModuleIdentity[] SampleModules =
        {
            new ModuleIdentity("test", "1.0", ModuleType.Server),
            new ModuleIdentity("test2", "1.0", ModuleType.Server),
            new ModuleIdentity("test2", "1.1", ModuleType.Server),
            new ModuleIdentity("test2", "1.0", ModuleType.Client),
            new ModuleIdentity("test2", "1.1", ModuleType.Client),
        };

        [SetUp]
        protected abstract void InitializeTest();

        [Test]
        public void TestAddModule()
        {
            UserModulesRepository.AddModule("1", SampleModules[0]);
            UserModulesRepository.AddModule("1", SampleModules[1]);
            UserModulesRepository.AddModule("1", SampleModules[2]);
            UserModulesRepository.AddModule("1", SampleModules[3]);
            UserModulesRepository.AddModule("1", SampleModules[4]);
            Assert.IsTrue(UserModulesRepository.GetModules("1").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("1").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("1").Contains(SampleModules[2]));
            Assert.IsTrue(UserModulesRepository.GetModules("1").Contains(SampleModules[3]));
            Assert.IsTrue(UserModulesRepository.GetModules("1").Contains(SampleModules[4]));

            UserModulesRepository.AddModule("2", SampleModules[0]);
            UserModulesRepository.AddModule("2", SampleModules[1]);
            UserModulesRepository.AddModule("2", SampleModules[2]);
            Assert.IsTrue(UserModulesRepository.GetModules("2").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("2").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("2").Contains(SampleModules[2]));
        }

        [Test]
        public void TestRemoveModule()
        {
            UserModulesRepository.AddModule("1", new ModuleIdentity("test", "1.0", ModuleType.Server));
        }

        [Test]
        public void TestGetModules()
        {
            UserModulesRepository.AddModule("1", new ModuleIdentity("test", "1.0", ModuleType.Server));
        }
    }
}

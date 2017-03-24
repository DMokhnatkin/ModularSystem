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
            new ModuleIdentity("test.server", "1.0"),
            new ModuleIdentity("test2.server", "1.0"),
            new ModuleIdentity("test2.server", "1.1"),
            new ModuleIdentity("test2.client", "1.0"),
            new ModuleIdentity("test2.client", "1.1")
        };

        [SetUp]
        protected abstract void InitializeTest();

        [Test]
        public void TestAddModule()
        {
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[0]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[1]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[2]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[3]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[4]);
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[2]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[3]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[4]));

            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[0]);
            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[1]);
            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[2]);
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[2]));
        }

        [Test]
        public void TestRemoveModule()
        {
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[0]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[1]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[2]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[3]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[4]);

            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[0]));
            UserModulesRepository.RemoveModule("1", "wpfclient", SampleModules[0]);
            Assert.IsFalse(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[0]));

            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[1]));
            UserModulesRepository.RemoveModule("1", "wpfclient", SampleModules[1]);
            Assert.IsFalse(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[1]));

            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[2]));
            UserModulesRepository.RemoveModule("1", "wpfclient", SampleModules[2]);
            Assert.IsFalse(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[2]));
        }

        [Test]
        public void TestGetModules()
        {
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[0]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[1]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[2]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[3]);
            UserModulesRepository.AddModule("1", "wpfclient", SampleModules[4]);
            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[0]);
            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[1]);
            UserModulesRepository.AddModule("2", "wpfclient", SampleModules[2]);

            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[2]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[3]));
            Assert.IsTrue(UserModulesRepository.GetModules("1", "wpfclient").Contains(SampleModules[4]));
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[0]));
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[1]));
            Assert.IsTrue(UserModulesRepository.GetModules("2", "wpfclient").Contains(SampleModules[2]));
        }
    }
}

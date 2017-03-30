using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class UserModulesTest
    {
        private RegisteredModules _registeredModules;
        private ZipPackagedModule[] _samplePackagedModules;

        [SetUp]
        public void InitializeTest()
        {
            _registeredModules = new RegisteredModules(new MemoryModulesRepository<ZipPackagedModule>(), new MemoryUserModulesRepository()); // Mock should be used
            _samplePackagedModules = new ZipPackagedModule[5];
            _samplePackagedModules[0] =
                new ZipPackagedModule { ModuleIdentity = new ModuleIdentity("test.server", "1.0"), Dependencies = new ModuleIdentity[0] };
            _samplePackagedModules[1] =
                new ZipPackagedModule { ModuleIdentity = new ModuleIdentity("test.client", "1.0"), Dependencies = new[] { _samplePackagedModules[0].ModuleIdentity } };
            _samplePackagedModules[2] =
                new ZipPackagedModule { ModuleIdentity = new ModuleIdentity("test.server", "2.0"), Dependencies = new[] { _samplePackagedModules[0].ModuleIdentity } };
            _samplePackagedModules[3] =
                new ZipPackagedModule { ModuleIdentity = new ModuleIdentity("test.client", "2.0"), Dependencies = new[] { _samplePackagedModules[1].ModuleIdentity, _samplePackagedModules[2].ModuleIdentity } };
            _samplePackagedModules[4] =
                new ZipPackagedModule { ModuleIdentity = new ModuleIdentity("test2.client", "1.0"), Dependencies = new ModuleIdentity[0] };

            _registeredModules.RegisterModules(_samplePackagedModules);
        }

        [Test]
        public void TestAddModule()
        {
            // Add module 0
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[0].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[0].ModuleIdentity));

            // Add module 1
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[1].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[1].ModuleIdentity));

            // Module 3 can't be added (required module 2 is not added)
            Assert.Throws<ModuleMissedException>(() => _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[3].ModuleIdentity));

            // Add module 2
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[2].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[2].ModuleIdentity));

            // Now module 3 can be added
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[3].ModuleIdentity);
        }

        [Test]
        public void TestAddModules()
        {
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleIdentity).Reverse());

            // Check module 0
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[0].ModuleIdentity));

            // Check module 1
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[1].ModuleIdentity));

            // Check module 2
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[2].ModuleIdentity));

            // Check module 3
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[3].ModuleIdentity));
        }

        [Test]
        public void TestRemoveModule()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleIdentity));

            // Remove module 3
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[3].ModuleIdentity);

            // We can't remove module 0. There are some dependent modules
            Assert.Throws<ModuleIsRequiredException>(() => _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[0].ModuleIdentity));

            // Remove module 2
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[2].ModuleIdentity);

            // Remove module 1
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[1].ModuleIdentity);

            // Now we can remove module 0
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[0].ModuleIdentity);
        }

        [Test]
        public void TestRemoveModules()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleIdentity));

            // Remove all modules
            _registeredModules.RemoveModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleIdentity));

            Assert.IsTrue(!_registeredModules.GetModuleIdentities("1", "wpfclient").Any());
        }
    }
}

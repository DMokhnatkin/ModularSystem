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
                new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "1.0"), new ModuleIdentity[0]) };
            _samplePackagedModules[1] =
                new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Client, "1.0"), new[] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[2] =
                new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "2.0"), new[] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[3] =
                new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Client, "2.0"), new[] { _samplePackagedModules[1].ModuleInfo.ModuleIdentity, _samplePackagedModules[2].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[4] =
                new ZipPackagedModule {ModuleInfo = new ModuleInfo(new ModuleIdentity("test2", ModuleType.Client, "1.0"), new ModuleIdentity[0]) };

            _registeredModules.RegisterModules(_samplePackagedModules);
        }

        [Test]
        public void TestAddModule()
        {
            // Add module 0
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));

            // Add module 1
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[1].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));

            // Module 3 can't be added (required module 2 is not added)
            Assert.Throws<ModuleMissedException>(() => _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[3].ModuleInfo.ModuleIdentity));

            // Add module 2
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[2].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));

            // Now module 3 can be added
            _registeredModules.AddModule("1", "wpfclient", _samplePackagedModules[3].ModuleInfo.ModuleIdentity);
        }

        [Test]
        public void TestAddModules()
        {
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity).Reverse());

            // Check module 0
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));

            // Check module 1
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));

            // Check module 2
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));

            // Check module 3
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestRemoveModule()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity));

            // Remove module 3
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[3].ModuleInfo.ModuleIdentity);

            // We can't remove module 0. There are some dependent modules
            Assert.Throws<ModuleIsRequiredException>(() => _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[0].ModuleInfo.ModuleIdentity));

            // Remove module 2
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[2].ModuleInfo.ModuleIdentity);

            // Remove module 1
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[1].ModuleInfo.ModuleIdentity);

            // Now we can remove module 0
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackagedModules[0].ModuleInfo.ModuleIdentity);
        }

        [Test]
        public void TestRemoveModules()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity));

            // Remove all modules
            _registeredModules.RemoveModules("1", "wpfclient", _samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity));

            Assert.IsTrue(!_registeredModules.GetModuleIdentities("1", "wpfclient").Any());
        }
    }
}

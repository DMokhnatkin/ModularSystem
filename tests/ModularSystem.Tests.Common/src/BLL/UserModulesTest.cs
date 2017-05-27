using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Modules;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Common.Repositories;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class UserModulesTest
    {
        private RegisteredModules _registeredModules;
        private IPackedModuleInfo[] _samplePackedModulesInfo;

        [SetUp]
        public void InitializeTest()
        {
            _registeredModules = new RegisteredModules(new MemoryModulesRepository<IPackedModuleInfo>(), new MemoryUserModulesRepository()); // Mock should be used
            _samplePackedModulesInfo = new IPackedModuleInfo[5];
            _samplePackedModulesInfo[0] =
                TestHelpers.CreateMemoryPackedModule("type", new ModuleIdentity("test.server", "1.0"), new ModuleIdentity[0]);
            _samplePackedModulesInfo[1] =
                TestHelpers.CreateMemoryPackedModule("type", new ModuleIdentity("test.client", "1.0"), new[] { _samplePackedModulesInfo[0].ModuleIdentity });
            _samplePackedModulesInfo[2] =
                TestHelpers.CreateMemoryPackedModule("type", new ModuleIdentity("test.server", "2.0"), new[] {_samplePackedModulesInfo[0].ModuleIdentity });
            _samplePackedModulesInfo[3] =
                TestHelpers.CreateMemoryPackedModule("type", new ModuleIdentity("test.client", "2.0"), new[] { _samplePackedModulesInfo[1].ModuleIdentity, _samplePackedModulesInfo[2].ModuleIdentity });
            _samplePackedModulesInfo[4] =
                TestHelpers.CreateMemoryPackedModule("type", new ModuleIdentity("test2.client", "1.0"), new ModuleIdentity[0] );

            _registeredModules.RegisterModules(_samplePackedModulesInfo);
        }

        [Test]
        public void TestAddModule()
        {
            // Add module 0
            _registeredModules.AddModule("1", "wpfclient", _samplePackedModulesInfo[0].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[0].ModuleIdentity));

            // Add module 1
            _registeredModules.AddModule("1", "wpfclient", _samplePackedModulesInfo[1].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[1].ModuleIdentity));

            // Module 3 can't be added (required module 2 is not added)
            Assert.Throws<ModuleMissedException>(() => _registeredModules.AddModule("1", "wpfclient", _samplePackedModulesInfo[3].ModuleIdentity));

            // Add module 2
            _registeredModules.AddModule("1", "wpfclient", _samplePackedModulesInfo[2].ModuleIdentity);
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[2].ModuleIdentity));

            // Now module 3 can be added
            _registeredModules.AddModule("1", "wpfclient", _samplePackedModulesInfo[3].ModuleIdentity);
        }

        [Test]
        public void TestAddModules()
        {
            _registeredModules.AddModules("1", "wpfclient", _samplePackedModulesInfo.Select(x => x.ModuleIdentity).Reverse());

            // Check module 0
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[0].ModuleIdentity));

            // Check module 1
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[1].ModuleIdentity));

            // Check module 2
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[2].ModuleIdentity));

            // Check module 3
            Assert.IsTrue(_registeredModules.GetModuleIdentities("1", "wpfclient").Contains(_samplePackedModulesInfo[3].ModuleIdentity));
        }

        [Test]
        public void TestRemoveModule()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackedModulesInfo.Select(x => x.ModuleIdentity));

            // Remove module 3
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackedModulesInfo[3].ModuleIdentity);

            // We can't remove module 0. There are some dependent modules
            Assert.Throws<ModuleIsRequiredException>(() => _registeredModules.RemoveModule("1", "wpfclient", _samplePackedModulesInfo[0].ModuleIdentity));

            // Remove module 2
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackedModulesInfo[2].ModuleIdentity);

            // Remove module 1
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackedModulesInfo[1].ModuleIdentity);

            // Now we can remove module 0
            _registeredModules.RemoveModule("1", "wpfclient", _samplePackedModulesInfo[0].ModuleIdentity);
        }

        [Test]
        public void TestRemoveModules()
        {
            // Add all modules
            _registeredModules.AddModules("1", "wpfclient", _samplePackedModulesInfo.Select(x => x.ModuleIdentity));

            // Remove all modules
            _registeredModules.RemoveModules("1", "wpfclient", _samplePackedModulesInfo.Select(x => x.ModuleIdentity));

            Assert.IsTrue(!_registeredModules.GetModuleIdentities("1", "wpfclient").Any());
        }
    }
}

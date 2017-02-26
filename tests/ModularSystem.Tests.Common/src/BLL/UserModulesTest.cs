using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Exceptions;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class UserModulesTest
    {
        private Modules _modules;
        private IModule[] _sampleModules;

        [SetUp]
        public void InitializeTest()
        {
            _modules = new Modules(new MemoryModulesRepository(), new MemoryUserModulesRepository()); // Mock should be used
            _sampleModules = new IModule[5];
            _sampleModules[0] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "1.0", ModuleType.Server), new ModuleIdentity[0]));
            _sampleModules[1] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "1.0", ModuleType.Client), new[] { _sampleModules[0].ModuleInfo.ModuleIdentity }));
            _sampleModules[2] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Server), new[] { _sampleModules[0].ModuleInfo.ModuleIdentity }));
            _sampleModules[3] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Client), new[] { _sampleModules[1].ModuleInfo.ModuleIdentity, _sampleModules[2].ModuleInfo.ModuleIdentity }));
            _sampleModules[4] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test2", "1.0", ModuleType.Client), new ModuleIdentity[0]));

            _modules.RegisterModules(_sampleModules);
        }

        [Test]
        public void TestAddModule()
        {
            // Add module 0
            _modules.AddModule("1", _sampleModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[0].ModuleInfo.ModuleIdentity));

            // Add module 1
            _modules.AddModule("1", _sampleModules[1].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[1].ModuleInfo.ModuleIdentity));

            // Module 3 can't be added (required module 2 is not added)
            Assert.Throws<ModuleMissedException>(() => _modules.AddModule("1", _sampleModules[3].ModuleInfo.ModuleIdentity));

            // Add module 2
            _modules.AddModule("1", _sampleModules[2].ModuleInfo.ModuleIdentity);
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[2].ModuleInfo.ModuleIdentity));

            // Now module 3 can be added
            _modules.AddModule("1", _sampleModules[3].ModuleInfo.ModuleIdentity);
        }

        [Test]
        public void TestAddModules()
        {
            _modules.AddModules("1", _sampleModules.Select(x => x.ModuleInfo.ModuleIdentity).Reverse());

            // Check module 0
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[0].ModuleInfo.ModuleIdentity));

            // Check module 1
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[1].ModuleInfo.ModuleIdentity));

            // Check module 2
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[2].ModuleInfo.ModuleIdentity));

            // Check module 3
            Assert.IsTrue(_modules.GetModuleIdentities("1").Contains(_sampleModules[3].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestRemoveModule()
        {
            // Add all modules
            _modules.AddModules("1", _sampleModules.Select(x => x.ModuleInfo.ModuleIdentity));

            // Remove module 3
            _modules.RemoveModule("1", _sampleModules[3].ModuleInfo.ModuleIdentity);

            // We can't remove module 0. There are some dependent modules
            Assert.Throws<ModuleIsRequiredException>(() => _modules.RemoveModule("1", _sampleModules[0].ModuleInfo.ModuleIdentity));

            // Remove module 2
            _modules.RemoveModule("1", _sampleModules[2].ModuleInfo.ModuleIdentity);

            // Remove module 1
            _modules.RemoveModule("1", _sampleModules[1].ModuleInfo.ModuleIdentity);

            // Now we can remove module 0
            _modules.RemoveModule("1", _sampleModules[0].ModuleInfo.ModuleIdentity);
        }

        [Test]
        public void TestRemoveModules()
        {
            // Add all modules
            _modules.AddModules("1", _sampleModules.Select(x => x.ModuleInfo.ModuleIdentity));

            // Remove all modules
            _modules.RemoveModules("1", _sampleModules.Select(x => x.ModuleInfo.ModuleIdentity));

            Assert.IsTrue(!_modules.GetModuleIdentities("1").Any());
        }
    }
}

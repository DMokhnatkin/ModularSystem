using System;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Modules;
using ModularSystem.Common.PackedModules;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class ModulesTest
    {
        private RegisteredModules _registeredModules;
        private IPackedModuleInfo[] _samplePackedModulesInfo;

        [SetUp]
        public void InitializeTest()
        {
            _registeredModules = new RegisteredModules(new MemoryModulesRepository<IPackedModuleInfo>(), new MemoryUserModulesRepository());
            _samplePackedModulesInfo = new IPackedModuleInfo[5];
            _samplePackedModulesInfo[0] = TestHelpers.CreateMemoryPackedModule("test", new ModuleIdentity("test.server", "1.0"), new ModuleIdentity[0]);
            _samplePackedModulesInfo[1] = TestHelpers.CreateMemoryPackedModule("test", new ModuleIdentity("test.client", "1.0"), new[] { _samplePackedModulesInfo[0].ModuleIdentity });
            _samplePackedModulesInfo[2] = TestHelpers.CreateMemoryPackedModule("test", new ModuleIdentity("test.server", "2.0"), new[] { _samplePackedModulesInfo[0].ModuleIdentity });
            _samplePackedModulesInfo[3] = TestHelpers.CreateMemoryPackedModule("test", new ModuleIdentity("test.client", "2.0"), new[] { _samplePackedModulesInfo[1].ModuleIdentity, _samplePackedModulesInfo[2].ModuleIdentity });
            _samplePackedModulesInfo[4] = TestHelpers.CreateMemoryPackedModule("test", new ModuleIdentity("test2.client", "1.0"), new ModuleIdentity[0]);
        }

        [Test]
        public void CheckDependencies()
        {
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackedModulesInfo[1]).IsCheckSuccess);
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackedModulesInfo[3]).IsCheckSuccess);
        }

        [Test]
        public void TestRegisterModule()
        {
            _registeredModules.RegisterModule(_samplePackedModulesInfo[0]);
            Assert.AreEqual(_samplePackedModulesInfo[0], _registeredModules.GetModule(_samplePackedModulesInfo[0].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackedModulesInfo[0]));
            _registeredModules.RegisterModule(_samplePackedModulesInfo[1]);
            Assert.AreEqual(_samplePackedModulesInfo[1], _registeredModules.GetModule(_samplePackedModulesInfo[1].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackedModulesInfo[1]));
            _registeredModules.RegisterModule(_samplePackedModulesInfo[2]);
            _registeredModules.RegisterModule(_samplePackedModulesInfo[3]);
            _registeredModules.RegisterModule(_samplePackedModulesInfo[4]);
            Assert.AreEqual(_samplePackedModulesInfo[2], _registeredModules.GetModule(_samplePackedModulesInfo[2].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[3], _registeredModules.GetModule(_samplePackedModulesInfo[3].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[4], _registeredModules.GetModule(_samplePackedModulesInfo[4].ModuleIdentity));
        }

        [Test]
        public void TestRegisterModules()
        {
            _registeredModules.RegisterModules(new []
            {
                _samplePackedModulesInfo[3],
                _samplePackedModulesInfo[1],
                _samplePackedModulesInfo[4],
                _samplePackedModulesInfo[0],
                _samplePackedModulesInfo[2],
            });
            Assert.AreEqual(_samplePackedModulesInfo[0], _registeredModules.GetModule(_samplePackedModulesInfo[0].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[1], _registeredModules.GetModule(_samplePackedModulesInfo[1].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[2], _registeredModules.GetModule(_samplePackedModulesInfo[2].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[3], _registeredModules.GetModule(_samplePackedModulesInfo[3].ModuleIdentity));
            Assert.AreEqual(_samplePackedModulesInfo[4], _registeredModules.GetModule(_samplePackedModulesInfo[4].ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _registeredModules.RegisterModules(_samplePackedModulesInfo);

            _registeredModules.UnregisterModule(_samplePackedModulesInfo[4].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModulesInfo[3].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModulesInfo[2].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModulesInfo[1].ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[1].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[2].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[3].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[4].ModuleIdentity));

            _registeredModules.UnregisterModule(_samplePackedModulesInfo[0].ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[0].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.UnregisterModule(_samplePackedModulesInfo[0].ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModules()
        {
            _registeredModules.RegisterModules(_samplePackedModulesInfo);

            _registeredModules.UnregisterModules(_samplePackedModulesInfo.Select(x => x.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[0].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[1].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[2].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[3].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModulesInfo[4].ModuleIdentity));
        }
    }
}

using System;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
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
        private FilePackedModule[] _samplePackedModules;

        [SetUp]
        public void InitializeTest()
        {
            _registeredModules = new RegisteredModules(new MemoryModulesRepository<IPackedModule>(), new MemoryUserModulesRepository());
            _samplePackedModules = new FilePackedModule[5];
            _samplePackedModules[0] = new FilePackedModule { ModuleIdentity = new ModuleIdentity("test.server", "1.0"), Dependencies = new ModuleIdentity[0] };
            _samplePackedModules[1] = new FilePackedModule { ModuleIdentity = new ModuleIdentity("test.client", "1.0"), Dependencies = new[] { _samplePackedModules[0].ModuleIdentity } };
            _samplePackedModules[2] = new FilePackedModule { ModuleIdentity = new ModuleIdentity("test.server", "2.0"), Dependencies = new[] { _samplePackedModules[0].ModuleIdentity } };
            _samplePackedModules[3] = new FilePackedModule { ModuleIdentity = new ModuleIdentity("test.client", "2.0"), Dependencies = new[] { _samplePackedModules[1].ModuleIdentity, _samplePackedModules[2].ModuleIdentity } };
            _samplePackedModules[4] = new FilePackedModule { ModuleIdentity = new ModuleIdentity("test2.client", "1.0"), Dependencies = new ModuleIdentity[0]};
        }

        [Test]
        public void CheckDependencies()
        {
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackedModules[1]).IsCheckSuccess);
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackedModules[3]).IsCheckSuccess);
        }

        [Test]
        public void TestRegisterModule()
        {
            _registeredModules.RegisterModule(_samplePackedModules[0]);
            Assert.AreEqual(_samplePackedModules[0], _registeredModules.GetModule(_samplePackedModules[0].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackedModules[0]));
            _registeredModules.RegisterModule(_samplePackedModules[1]);
            Assert.AreEqual(_samplePackedModules[1], _registeredModules.GetModule(_samplePackedModules[1].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackedModules[1]));
            _registeredModules.RegisterModule(_samplePackedModules[2]);
            _registeredModules.RegisterModule(_samplePackedModules[3]);
            _registeredModules.RegisterModule(_samplePackedModules[4]);
            Assert.AreEqual(_samplePackedModules[2], _registeredModules.GetModule(_samplePackedModules[2].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[3], _registeredModules.GetModule(_samplePackedModules[3].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[4], _registeredModules.GetModule(_samplePackedModules[4].ModuleIdentity));
        }

        [Test]
        public void TestRegisterModules()
        {
            _registeredModules.RegisterModules(new []
            {
                _samplePackedModules[3],
                _samplePackedModules[1],
                _samplePackedModules[4],
                _samplePackedModules[0],
                _samplePackedModules[2],
            });
            Assert.AreEqual(_samplePackedModules[0], _registeredModules.GetModule(_samplePackedModules[0].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[1], _registeredModules.GetModule(_samplePackedModules[1].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[2], _registeredModules.GetModule(_samplePackedModules[2].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[3], _registeredModules.GetModule(_samplePackedModules[3].ModuleIdentity));
            Assert.AreEqual(_samplePackedModules[4], _registeredModules.GetModule(_samplePackedModules[4].ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _registeredModules.RegisterModules(_samplePackedModules);

            _registeredModules.UnregisterModule(_samplePackedModules[4].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModules[3].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModules[2].ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackedModules[1].ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[1].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[2].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[3].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[4].ModuleIdentity));

            _registeredModules.UnregisterModule(_samplePackedModules[0].ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[0].ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.UnregisterModule(_samplePackedModules[0].ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModules()
        {
            _registeredModules.RegisterModules(_samplePackedModules);

            _registeredModules.UnregisterModules(_samplePackedModules.Select(x => x.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[0].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[1].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[2].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[3].ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackedModules[4].ModuleIdentity));
        }
    }
}

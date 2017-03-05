using System;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class ModulesTest
    {
        private RegisteredModules _registeredModules;
        private ZipPackagedModule[] _samplePackagedModules;

        [SetUp]
        public void InitializeTest()
        {
            _registeredModules = new RegisteredModules(new MemoryModulesRepository<ZipPackagedModule>(), new MemoryUserModulesRepository());
            _samplePackagedModules = new ZipPackagedModule[5];
            _samplePackagedModules[0] = new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "1.0"), new ModuleIdentity[0]) };
            _samplePackagedModules[1] = new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Client, "1.0"), new[] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[2] = new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "2.0"), new[] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[3] = new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test", ModuleType.Client, "2.0"), new[] { _samplePackagedModules[1].ModuleInfo.ModuleIdentity, _samplePackagedModules[2].ModuleInfo.ModuleIdentity }) };
            _samplePackagedModules[4] = new ZipPackagedModule { ModuleInfo = new ModuleInfo(new ModuleIdentity("test2", ModuleType.Client, "1.0"), new ModuleIdentity[0])};
        }

        [Test]
        public void CheckDependencies()
        {
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackagedModules[1].ModuleInfo).IsCheckSuccess);
            Assert.IsFalse(_registeredModules.CheckDependencies(_samplePackagedModules[3].ModuleInfo).IsCheckSuccess);
        }

        [Test]
        public void TestRegisterModule()
        {
            _registeredModules.RegisterModule(_samplePackagedModules[0]);
            Assert.AreEqual(_samplePackagedModules[0], _registeredModules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackagedModules[0]));
            _registeredModules.RegisterModule(_samplePackagedModules[1]);
            Assert.AreEqual(_samplePackagedModules[1], _registeredModules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.RegisterModule(_samplePackagedModules[1]));
            _registeredModules.RegisterModule(_samplePackagedModules[2]);
            _registeredModules.RegisterModule(_samplePackagedModules[3]);
            _registeredModules.RegisterModule(_samplePackagedModules[4]);
            Assert.AreEqual(_samplePackagedModules[2], _registeredModules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[3], _registeredModules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[4], _registeredModules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestRegisterModules()
        {
            _registeredModules.RegisterModules(new []
            {
                _samplePackagedModules[3],
                _samplePackagedModules[1],
                _samplePackagedModules[4],
                _samplePackagedModules[0],
                _samplePackagedModules[2],
            });
            Assert.AreEqual(_samplePackagedModules[0], _registeredModules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[1], _registeredModules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[2], _registeredModules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[3], _registeredModules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[4], _registeredModules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _registeredModules.RegisterModules(_samplePackagedModules);

            _registeredModules.UnregisterModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity);
            _registeredModules.UnregisterModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));

            _registeredModules.UnregisterModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _registeredModules.UnregisterModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModules()
        {
            _registeredModules.RegisterModules(_samplePackagedModules);

            _registeredModules.UnregisterModules(_samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_registeredModules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }
    }
}

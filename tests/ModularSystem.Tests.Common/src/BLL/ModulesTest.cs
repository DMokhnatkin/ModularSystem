using System;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.BLL
{
    [TestFixture]
    public class ModulesTest
    {
        private Modules _modules;
        private IPathModule[] _samplePackagedModules;

        [SetUp]
        public void InitializeTest()
        {
            _modules = new Modules(new MemoryModulesRepository(), new MemoryUserModulesRepository());
            _samplePackagedModules = new IPathModule[5];
            _samplePackagedModules[0] =
                Mock.Of<IPathModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "1.0"), new ModuleIdentity[0]));
            _samplePackagedModules[1] =
                Mock.Of<IPathModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test",ModuleType.Client, "1.0"), new [] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }));
            _samplePackagedModules[2] =
                Mock.Of<IPathModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", ModuleType.Server, "2.0"), new [] { _samplePackagedModules[0].ModuleInfo.ModuleIdentity }));
            _samplePackagedModules[3] =
                Mock.Of<IPathModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", ModuleType.Client, "2.0"), new [] { _samplePackagedModules[1].ModuleInfo.ModuleIdentity, _samplePackagedModules[2].ModuleInfo.ModuleIdentity }));
            _samplePackagedModules[4] =
                Mock.Of<IPathModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test2", ModuleType.Client, "1.0"), new ModuleIdentity[0]));
        }

        [Test]
        public void CheckDependencies()
        {
            Assert.IsFalse(_modules.CheckDependencies(_samplePackagedModules[1].ModuleInfo).IsCheckSuccess);
            Assert.IsFalse(_modules.CheckDependencies(_samplePackagedModules[3].ModuleInfo).IsCheckSuccess);
        }

        [Test]
        public void TestRegisterModule()
        {
            _modules.RegisterModule(_samplePackagedModules[0]);
            Assert.AreEqual(_samplePackagedModules[0], _modules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.RegisterModule(_samplePackagedModules[0]));
            _modules.RegisterModule(_samplePackagedModules[1]);
            Assert.AreEqual(_samplePackagedModules[1], _modules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.RegisterModule(_samplePackagedModules[1]));
            _modules.RegisterModule(_samplePackagedModules[2]);
            _modules.RegisterModule(_samplePackagedModules[3]);
            _modules.RegisterModule(_samplePackagedModules[4]);
            Assert.AreEqual(_samplePackagedModules[2], _modules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[3], _modules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[4], _modules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestRegisterModules()
        {
            _modules.RegisterModules(new []
            {
                _samplePackagedModules[3],
                _samplePackagedModules[1],
                _samplePackagedModules[4],
                _samplePackagedModules[0],
                _samplePackagedModules[2],
            });
            Assert.AreEqual(_samplePackagedModules[0], _modules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[1], _modules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[2], _modules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[3], _modules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_samplePackagedModules[4], _modules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _modules.RegisterModules(_samplePackagedModules);

            _modules.UnregisterModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));

            _modules.UnregisterModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.UnregisterModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModules()
        {
            _modules.RegisterModules(_samplePackagedModules);

            _modules.UnregisterModules(_samplePackagedModules.Select(x => x.ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[0].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_samplePackagedModules[4].ModuleInfo.ModuleIdentity));
        }
    }
}

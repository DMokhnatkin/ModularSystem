using System;
using System.Linq;
using ModularSystem.Common;
using ModularSystem.Common.BLL;
using ModularSystem.Common.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Server.BLL
{
    [TestFixture]
    public class ModulesTest
    {
        private Modules _modules;
        private IModule[] _sampleModules;

        [SetUp]
        public void InitializeTest()
        {
            _modules = new Modules(new InMemoryModulesRepository());
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
                        new ModuleInfo(new ModuleIdentity("test", "1.0", ModuleType.Client), new [] { _sampleModules[0].ModuleInfo.ModuleIdentity }));
            _sampleModules[2] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Server), new [] { _sampleModules[0].ModuleInfo.ModuleIdentity }));
            _sampleModules[3] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Client), new [] { _sampleModules[1].ModuleInfo.ModuleIdentity, _sampleModules[2].ModuleInfo.ModuleIdentity }));
            _sampleModules[4] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test2", "1.0", ModuleType.Client), new ModuleIdentity[0]));
        }

        [Test]
        public void CheckDependencies()
        {
            Assert.IsFalse(_modules.CheckDependencies(_sampleModules[1].ModuleInfo).IsCheckSuccess);
            Assert.IsFalse(_modules.CheckDependencies(_sampleModules[3].ModuleInfo).IsCheckSuccess);
        }

        [Test]
        public void TestRegisterModule()
        {
            _modules.RegisterModule(_sampleModules[0]);
            Assert.AreEqual(_sampleModules[0], _modules.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.RegisterModule(_sampleModules[0]));
            _modules.RegisterModule(_sampleModules[1]);
            Assert.AreEqual(_sampleModules[1], _modules.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.RegisterModule(_sampleModules[1]));
            _modules.RegisterModule(_sampleModules[2]);
            _modules.RegisterModule(_sampleModules[3]);
            _modules.RegisterModule(_sampleModules[4]);
            Assert.AreEqual(_sampleModules[2], _modules.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[3], _modules.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[4], _modules.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestRegisterModules()
        {
            _modules.RegisterModules(new []
            {
                _sampleModules[3],
                _sampleModules[1],
                _sampleModules[4],
                _sampleModules[0],
                _sampleModules[2],
            });
            Assert.AreEqual(_sampleModules[0], _modules.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[1], _modules.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[2], _modules.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[3], _modules.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[4], _modules.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _modules.RegisterModules(_sampleModules);

            _modules.UnregisterModule(_sampleModules[4].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_sampleModules[3].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_sampleModules[2].ModuleInfo.ModuleIdentity);
            _modules.UnregisterModule(_sampleModules[1].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_modules.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));

            _modules.UnregisterModule(_sampleModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_modules.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _modules.UnregisterModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModules()
        {
            _modules.RegisterModules(_sampleModules);

            _modules.UnregisterModules(_sampleModules.Select(x => x.ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_modules.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));
        }
    }
}

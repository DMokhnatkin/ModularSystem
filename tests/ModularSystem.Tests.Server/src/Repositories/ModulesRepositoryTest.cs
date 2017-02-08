using System;
using ModularSystem.Common;
using ModularSystem.Server.Repositories;
using Moq;
using NUnit.Framework;

namespace ModularSystem.Tests.Server.Repositories
{
    [TestFixture]
    public class ModulesRepositoryTest
    {
        private ModulesRepository _repository;
        private IModule[] _sampleModules;

        [SetUp]
        public void InitializeTest()
        {
            _repository = new ModulesRepository();
            _sampleModules = new IModule[5];
            _sampleModules[0] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "1.0", ModuleType.Client), new ModuleInfo[0]));
            _sampleModules[1] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "1.0", ModuleType.Server), new ModuleInfo[0]));
            _sampleModules[2] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Client), new ModuleInfo[0]));
            _sampleModules[3] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test", "2.0", ModuleType.Server), new ModuleInfo[0]));
            _sampleModules[4] =
                Mock.Of<IModule>(
                    x =>
                        x.ModuleInfo ==
                        new ModuleInfo(new ModuleIdentity("test2", "1.0", ModuleType.Client), new ModuleInfo[0]));
        }

        [Test]
        public void TestRegisterModule()
        {
            _repository.RegisterModule(_sampleModules[0]);
            Assert.AreEqual(_sampleModules[0], _repository.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _repository.RegisterModule(_sampleModules[0]));
            _repository.RegisterModule(_sampleModules[1]);
            Assert.AreEqual(_sampleModules[1], _repository.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _repository.RegisterModule(_sampleModules[1]));
            _repository.RegisterModule(_sampleModules[2]);
            _repository.RegisterModule(_sampleModules[3]);
            _repository.RegisterModule(_sampleModules[4]);
            Assert.AreEqual(_sampleModules[2], _repository.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[3], _repository.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.AreEqual(_sampleModules[4], _repository.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));
        }

        [Test]
        public void TestUnregisterModule()
        {
            _repository.RegisterModule(_sampleModules[0]);
            _repository.RegisterModule(_sampleModules[1]);
            _repository.RegisterModule(_sampleModules[2]);
            _repository.RegisterModule(_sampleModules[3]);
            _repository.RegisterModule(_sampleModules[4]);

            _repository.UnregisterModule(_sampleModules[0].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_repository.GetModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            Assert.Throws<ArgumentException>(() => _repository.UnregisterModule(_sampleModules[0].ModuleInfo.ModuleIdentity));
            _repository.UnregisterModule(_sampleModules[1].ModuleInfo.ModuleIdentity);
            _repository.UnregisterModule(_sampleModules[2].ModuleInfo.ModuleIdentity);
            _repository.UnregisterModule(_sampleModules[3].ModuleInfo.ModuleIdentity);
            _repository.UnregisterModule(_sampleModules[4].ModuleInfo.ModuleIdentity);
            Assert.IsNull(_repository.GetModule(_sampleModules[1].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_repository.GetModule(_sampleModules[2].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_repository.GetModule(_sampleModules[3].ModuleInfo.ModuleIdentity));
            Assert.IsNull(_repository.GetModule(_sampleModules[4].ModuleInfo.ModuleIdentity));
        }
    }
}

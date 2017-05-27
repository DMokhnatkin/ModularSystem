using System;
using System.IO;
using ModularSystem.Common.Repositories;
using ModularSystem.Common.Repositories.UserModules;
using NUnit.Framework;

namespace ModularSystem.Tests.Common.Repositories.Concrete
{
    public class FileUserModulesRepositoryTest : BaseIUserModulesRepositoryTest
    {
        private string _filePath;

        /// <inheritdoc />
        protected override void InitializeTest()
        {
            _filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            UserModulesRepository = new FileUserModulesRepository(_filePath);
        }

        [TearDown]
        public void EndTest()
        {
            File.Delete(_filePath);
        }
    }
}

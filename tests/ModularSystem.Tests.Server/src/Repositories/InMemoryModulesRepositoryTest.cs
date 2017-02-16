using ModularSystem.Common.Repositories;

namespace ModularSystem.Tests.Server.Repositories
{
    public class InMemoryModulesRepositoryTest : BaseModulesRepositoryTest
    {
        /// <inheritdoc />
        public override IModulesRepository CreateModulesRepository()
        {
            return new InMemoryModulesRepository();
        }
    }
}

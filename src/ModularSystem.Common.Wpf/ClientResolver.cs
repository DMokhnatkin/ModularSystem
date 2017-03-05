using Microsoft.Practices.Unity;
using ModularSystem.Common.Wpf.UI;

namespace ModularSystem.Common.Wpf
{
    public static class ClientResolver
    {
        public static UnityContainer UnityContainer { get; } = new UnityContainer();

        static ClientResolver()
        {
            UnityContainer.RegisterInstance(new MainUi(), new ContainerControlledLifetimeManager());
        }

        public static MainUi GetUi()
        {
            return UnityContainer.Resolve<MainUi>();
        }
    }
}

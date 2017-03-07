using System.Runtime.CompilerServices;
using Microsoft.Practices.Unity;
using ModularSystem.Common.Wpf.Modules;

namespace ModularSystem.Common.Wpf.Context
{
    public class ClientAppContext
    {
        public static ClientAppContext CurrentContext
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set;
        }

        public ClientAppContext(InstalledModuleCollection installedModules, AuthenticationContext authenticationContext)
        {
            InstalledModules = installedModules;
            AuthenticationContext = authenticationContext;
            Container = new UnityContainer();
        }

        public AuthenticationContext AuthenticationContext { get; }

        /// <summary>
        /// Modules installed for cur session.
        /// </summary>
        public InstalledModuleCollection InstalledModules { get; set; }

        /// <summary>
        /// This container contains instances registered by modules. Modules can interact using this classes. 
        /// </summary>
        public UnityContainer Container { get; }
    }
}

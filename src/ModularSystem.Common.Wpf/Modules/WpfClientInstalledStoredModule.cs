using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ModularSystem.Common.MetaFiles;
using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Wpf.Modules
{
    public class WpfClientInstalledStoredModule : IModule, IPathStoredModule, IStartableModule
    {
        /// <inheritdoc />
        public ModuleIdentity ModuleIdentity { get; private set; }

        /// <inheritdoc />
        public ModuleIdentity[] Dependencies { get; private set; }

        /// <inheritdoc />
        public string Path { get; set; }

        public IWpfClientEntry WpfClientEntry { get; set; }

        /// <inheritdoc />
        public void InitializeFromPath(string path)
        {
            Path = path;
            var t = FindEntryClass();
            if (t != null)
                WpfClientEntry = (IWpfClientEntry)Activator.CreateInstance(t);
            var conf = MetaFileWrapper.FindInDirectory(Path);
            ModuleIdentity = ModuleIdentity.Parse(conf.Identity);
            Dependencies = conf.Dependencies.Select(ModuleIdentity.Parse).ToArray();
        }

        /// <summary>
        /// Find class which implements IWpfClientEntry in all module dll files
        /// </summary>
        /// <returns></returns>
        public Type FindEntryClass()
        {
            foreach (var t in Directory.GetFiles(Path, "*.dll"))
            {
                var assembly = Assembly.LoadFile(t);
                var c = assembly.GetTypes().FirstOrDefault(x => typeof(IWpfClientEntry).IsAssignableFrom(x));
                if (c != null)
                    return c;
            }
            //currentDomain.AssemblyResolve -= CurrentDomainAssemblyResolve;
            return null;
        }

        /// <inheritdoc />
        public void Start()
        {
            WpfClientEntry?.OnStart();
        }

        /// <inheritdoc />
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}

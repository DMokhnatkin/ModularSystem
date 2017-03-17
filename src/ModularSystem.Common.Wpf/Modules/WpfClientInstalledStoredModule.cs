using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ModularSystem.Common.Modules;
using Newtonsoft.Json;

namespace ModularSystem.Common.Wpf.Modules
{
    public class WpfClientInstalledStoredModule : IPathStoredModule
    {
        /// <inheritdoc />
        public ModuleInfo ModuleInfo { get; set; }

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
            string confPath = System.IO.Path.Combine(Path, ModuleSettings.ConfFileName);
            var conf = ModuleConf.LoadFromString(File.ReadAllText(confPath));
            ModuleInfo = new ModuleInfo(ModuleIdentity.Parse(conf.ModuleIdentity), conf.Dependencies.Select(ModuleIdentity.Parse).ToArray());
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
    }
}

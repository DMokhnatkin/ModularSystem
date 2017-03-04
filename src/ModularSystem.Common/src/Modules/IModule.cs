namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Base module interface. All modules implement it.
    /// </summary>
    public interface IModule
    {
        /// <see cref="ModuleInfo"/>
        ModuleInfo ModuleInfo { get; }
    }
}
namespace ModularSystem.Common.Modules
{
    public interface IInstalledModule : IModule
    {
        string BaseDir { get; set; }
    }
}
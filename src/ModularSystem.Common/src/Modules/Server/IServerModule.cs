namespace ModularSystem.Common.Modules.Server
{
    public interface IServerModule : IModule
    {
        void Start();
        void Stop();
    }
}

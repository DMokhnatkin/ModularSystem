namespace ModularSystem.Common.Modules
{
    /// <summary>
    /// Module which can be started (and stopped).
    /// </summary>
    public interface IStartableModule
    {
        void Start();
        void Stop();
    }
}

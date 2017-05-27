namespace ModularSystem.Common.Modules.Client
{
    public interface IClientModule : IModule
    {
        /// <summary>
        /// List of client types which can install this client module (only for client modules).
        /// </summary>
        string[] ClientTypes { get; }
    }
}

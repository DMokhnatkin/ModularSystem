namespace ModularSystem.Common.Modules.Generic
{
    public interface IPackagedModule<out T> : IPackagedModule
    {
        new T Data { get; }
    }
}
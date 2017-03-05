namespace ModularSystem.Common.Wpf
{
    public interface IWpfClientEntry
    {
        void OnInstalled();
        void OnRemoved();
        void OnStart();
        void OnExit();
    }
}

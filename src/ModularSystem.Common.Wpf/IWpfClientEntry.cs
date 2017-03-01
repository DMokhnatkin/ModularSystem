namespace ModularSystem.Common.Wpf
{
    public interface IWpfClientEntry
    {
        void OnInstalled();
        void OnRemoved();
        void OnStart(WpfClientStartArgs args);
        void OnExit(WpfClientExitArgs args);
    }
}

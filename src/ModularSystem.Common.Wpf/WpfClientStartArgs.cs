using System.Windows.Controls;

namespace ModularSystem.Common.Wpf
{
    public class WpfClientStartArgs
    {
        public WpfClientStartArgs(ContentControl parentContentControl)
        {
            WindowContentControl = parentContentControl;
        }

        public ContentControl WindowContentControl { get; private set; }
    }
}

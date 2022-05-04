using System.Windows.Controls;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    public record FactorySubControlTab
    {
        public UserControl UserControl { get; init; }
        public string Header { get; init; }
        public FactorySubControlTab(UserControl userControl, string header)
        {
            UserControl = userControl;
            Header = header;
        }
    }
}

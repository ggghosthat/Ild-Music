using Avalonia.Controls;
using Ild_Music.Core.Contracts.ViewModel;

namespace Ild_Music.Core.Contracts;

public interface IPlugin : IShare
{
    public Guid PluginId { get; }
    
    public string PluginName { get; }

    public string PluginDescription { get; }

    public bool IsActive { get; set; }

    public UserControl UserControl { get; set; }

    public BaseViewModel ViewModel { get; set; }
    
    public void Toggle();
}
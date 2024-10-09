namespace Ild_Music.Core.Contracts;

public interface IPlugin : IShare
{
    public Guid PluginId { get; }
    
    public string PluginName { get; }

    public string PluginDescription { get; }

    public bool IsActive { get; set; }

    public void Toggle();
}
namespace Ild_Music.Core.Configure;

public record Config()
{
    public string _playerSource;
    public string _cubeSource;
    public IEnumerable<string> _players;
    public IEnumerable<string> _repositories;
    public IEnumerable<string> _plugins;

    public IEnumerable<string> Players 
    {
        get => _players;
        set => _players = value;
    }

    public IEnumerable<string> Repositories 
    {
        get => _repositories;
        set => _repositories = value;
    }

    public string PlayerSource 
    {
        get => _playerSource;
        set => _playerSource = value;
    }

    public string RepositorySource 
    {
        get => _cubeSource;
        set => _cubeSource = value;
    }

    public IEnumerable<string> Plugins
    {
        get => _plugins;
        set => _plugins = value;
    } 
}

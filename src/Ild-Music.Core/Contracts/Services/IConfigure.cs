using Ild_Music.Core.Exceptions.Flag;

namespace Ild_Music.Core.Contracts;

public interface IConfigure
{
    public ReadOnlyMemory<char> ComponentsFile {get; init;}
 	public Config ConfigSheet {get; set;}    

    public void Parse();
}

public record Config()
{
    public string _playerSource;
    public string _cubeSource;
    public IEnumerable<string> _players;
    public IEnumerable<string> _cubes;

    public IEnumerable<string> Players 
    {
        get => _players;
        set => _players = value;
    }
    public IEnumerable<string> Cubes 
    {
        get => _cubes;
        set => _cubes = value;
    }

    public string PlayerSource 
    {
        get => _playerSource;
        set => _playerSource = value;
    }
    public string CubeSource 
    {
        get => _cubeSource;
        set => _cubeSource = value;
    }
}
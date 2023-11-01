using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShareInstances.Configure;
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

public class Configure:IConfigure
{
    public ReadOnlyMemory<char> ComponentsFile {get; init;}

    public Config ConfigSheet {get; set;}

    public Configure()
    {}

    public Configure(ReadOnlyMemory<char> componentsFile)
    {
        ComponentsFile = componentsFile;
        ParseAsync().Wait();
    }

    public async Task ParseAsync()
    {
        using FileStream openStream = File.OpenRead(ComponentsFile.ToString());
        ConfigSheet = await JsonSerializer.DeserializeAsync<Config>(openStream);
    }
}

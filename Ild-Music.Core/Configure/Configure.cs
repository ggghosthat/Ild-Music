using Ild_Music.Core.Contracts;

using System.Text.Json;
namespace ShareInstances.Configure;
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

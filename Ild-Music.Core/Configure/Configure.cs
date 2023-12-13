using Ild_Music.Core.Contracts;

using System.Text.Json;

namespace Ild_Music.Core.Configure;

public class Configure:IConfigure
{
    public ReadOnlyMemory<char> ComponentsFile {get; init;}

    public Config ConfigSheet {get; set;}

    public Configure()
    {}

    public Configure(string componentsFile)
    {
        ComponentsFile = componentsFile.AsMemory();
        ParseAsync().Wait();
    }

    public async Task ParseAsync()
    {
        using FileStream openStream = File.OpenRead(ComponentsFile.ToString());
        ConfigSheet = await JsonSerializer.DeserializeAsync<Config>(openStream);
    }
}

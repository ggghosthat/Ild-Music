using Ild_Music.Core.Contracts;
using Ild_Music.Core.Exceptions.Flag;

using System.Text.Json;

namespace Ild_Music.Core.Configure;

public class Configure : IConfigure, IErrorTracable
{
    public Configure()
    {}

    public Configure(string componentsFile)
    {
        ComponentsFile = componentsFile.AsMemory();
    }

    public ReadOnlyMemory<char> ComponentsFile {get; init;}

    public Config? ConfigSheet {get; set;}

    public List<ErrorFlag> Errors { get; private set; } = [];

    public void Parse()
    {
        bool isCompleted;
        try
        {
            using FileStream openStream = File.OpenRead(ComponentsFile.ToString());
            ConfigSheet = JsonSerializer.Deserialize<Config>(openStream);
        }
        catch(Exception ex)
        {
            var error = new ErrorFlag("configure", "parsing", ex.Message);
            Errors.Add(error);
        }
    }

    public bool CheckErrors(List<ErrorFlag> errorList)
    {
        bool result = false;

        if (Errors.Count > 0)
        {
            errorList.AddRange(Errors);
            Errors.Clear();
            result = true;
        }

        return result;
    }
}

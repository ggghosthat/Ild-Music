using ShareInstances.Stage;
using ShareInstances.Configure;
namespace Ild_Music.CLI.Test;
class Program
{
    //private static IConfigure configure = new Configure("./config.json".AsMemory());
    //private static Stage _stage = new (ref configure);

    public async static Task Main(string[] args)
    {
        IConfigure configure = new Configure("./config.json".AsMemory());
        Stage _stage = new (ref configure);
        await _stage.Build();
        _stage.SwitchCube(0);
        Console.WriteLine($"Point {_stage.CubeInstance.CubeName}");
    }
}

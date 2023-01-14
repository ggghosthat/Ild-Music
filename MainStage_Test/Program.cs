using System;
using System.IO;
using ShareInstances.Services.Entities;
using ShareInstances.Stage;
using SynchronizationBlock.Models.SynchArea;
using System.Linq;
using System.Reflection;
using ShareInstances;
using ShareInstances.PlayerResources;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MainStage_Test
{
    class Program
    {
        // private static Platform _platform = new();
        private static Stage _stage;

        static void Main(string[] args)
        {
            string corePlayerPath = "E:/ild_music/Ild-Music/Ild-Music-Core/bin/Debug/net6.0-windows";
            string areaPath = "E:/ild_music/Ild-Music/SynchronizationBlock/bin/Debug/net6.0";
            _stage = new(corePlayerPath, areaPath);
            var supporter = (SupporterService)_stage.serviceCenter.GetService("SupporterService");
            var zzz = new ObservableCollection<IPlayer>();
            var xxxx = _stage.Players.ToList()[0];
            Console.WriteLine(xxxx.CurrentTime);

            // var path = "E:/ild_music/Ild-Music/Ild-Music-Core/bin/Debug/net6.0-windows/Ild-Music-Core.dll";
            // var assembly = Assembly.LoadFrom(path);
            // foreach (var i in assembly.GetReferencedAssemblies().ToList().Where(x => x.Name.StartsWith("System") == false ))
            //     Console.WriteLine($"{i.Name} --- {i.Version}");
        }

    }
}

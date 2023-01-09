using System;
using System.IO;
using ShareInstances.Services.Entities;
using ShareInstances.Stage;
using SynchronizationBlock.Models.SynchArea;
using System.Linq;
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
            _stage.Players.ToList().ForEach(x => zzz.Add((IPlayer)x));
            foreach (var j in zzz)
                Console.WriteLine(j.PlayerName);
        }
    }
}

using System;
using System.IO;
using MainStage.Stage;
using System.Linq;

namespace MainStage_Test
{
    class Program
    {
        private static Platform _platform = new();

        static void Main(string[] args)
        {
            //string corePlayerPath = "E:/C# projects/Ild-Music/Ild-Music-Core/bin/Debug/net5.0-windows";
            //string areaPath = "E:/C# projects/Ild-Music/SynchronizationBlock/bin/Debug/net5.0";
            //_platform.InitPlayer(corePlayerPath);
            //_platform.InitSynch(areaPath);


            //foreach (var item in _platform.listPlayers)
            //    Console.WriteLine($"[PLAYER] {item.PlayerId} --- {item.PlayerName}");

            //foreach (var item in _platform.listSynchAreas)
            //    Console.WriteLine($"[SYNCH] {item.AreaId} --- {item.AreaName}");

            string startupPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName.ToString();
            var x = new DirectoryInfo(startupPath);
            Console.WriteLine(startupPath);
            x.GetDirectories().ToList().ForEach(y => Console.WriteLine($"\t{y}"));
            // Read the file as one string.
            
        }
    }
}

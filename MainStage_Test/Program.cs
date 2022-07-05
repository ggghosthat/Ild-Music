using System;
using MainStage.Stage;

namespace MainStage_Test
{
    class Program
    {
        private static Platform _platform = new();

        static void Main(string[] args)
        {
            string corePlayerPath = "E:/C# projects/Ild-Music/Ild-Music-Core/bin/Debug/net5.0-windows";
            string areaPath = "E:/C# projects/Ild-Music/SynchronizationBlock/bin/Debug/net5.0";
            _platform.InitPlayer(corePlayerPath);
            _platform.InitSynch(areaPath);

            
            foreach (var item in _platform._listPlayers)
                Console.WriteLine($"[PLAYER] {item.PlayerId} --- {item.PlayerName}");
            
            foreach (var item in _platform._listSynchAreas)
                Console.WriteLine($"[SYNCH] {item.AreaId} --- {item.AreaName}");
            
        }
    }
}

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
            _platform.InitPlayer(corePlayerPath);

            
            foreach (var item in _platform._listPlayers)
            {
                Console.WriteLine($"{item.PlayerId} --- {item.PlayerName}");
            }
        }
    }
}

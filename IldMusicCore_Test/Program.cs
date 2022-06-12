using Ild_Music_CORE.Models.Core.Session_Structure;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IldMusicCore_Test
{
    class Program
    {
        static bool isActive = true;

        static Track track1 = new Track("E:/C# projects/saint/2_5233518075101315631.mp3", "track1");
        static Track track2 = new Track("E:/C# projects/saint/2_5235868813781631546.mp3", "track2");
        static Track track3 = new Track("E:/C# projects/saint/2_5251668997736235620.mp3", "track3");
        static Track track4 = new Track("E:/C# projects/saint/Eminem Lose Yourself HD.mp3", "track4");

        static Tracklist playlist = new Tracklist(new List<Track>() { track1, track2, track3, track4});

        static PlayerWrap _player;
        static void Main(string[] args)
        {
            playlist.Order();
            Test1_PlaySingle();
            while (isActive)
            {
                Console.WriteLine("Your cmd sar : ");
                var cmd = Console.ReadLine();
                CommandProcessor(cmd);
            }
            
        }

        static void Test1_PlaySingle()
        {
            //_player = new PlayerWrap(track1);
            _player = new PlayerWrap(playlist, 1);
        }

        static void CommandProcessor(string cmd)
        {
            if (cmd == "play")
            {
                _player.StartPlayer();
            }
            if (cmd == "stop")
            {
                _player.StopPlayer();
            }
            if (cmd == "next") 
            {
                _player.DropNext();
            }
            if (cmd == "prev")
            {
                _player.DropPrevious();
            }
        }
    }
}

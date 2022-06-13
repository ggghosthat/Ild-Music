using Ild_Music_CORE.Models.Core.Session_Structure;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System;
using System.Collections.Generic;

namespace IldMusicCore_Test
{
    class Program
    {
        static PlayerWrap _player;
        static bool isActive = true;

        //static Track tr1 = new("E://gaga/start/music/2_5233518075101315631.mp3");
        static Track tr1 = new("E://gaga/start/music/2_5235868813781631546.mp3");
        static Track tr2 = new("E://gaga/start/music/2_5251668997736235620.mp3");

        static Tracklist playlist = new(new List<Track>() { tr1, tr2 });

        static void Main(string[] args)
        {
            playlist.Order();
            InitPlayer();
            
            while (isActive)
            {
                CommandProcesser();
            }
        }

        private static void InitPlayer() => _player = new (playlist, 1);

        private static void CommandProcesser()
        {
            Console.WriteLine("Type command : ");
            var cmd = Console.ReadLine();

            if (cmd is "play")
                _player.StartPlayer();
            if (cmd is "stop")
                _player.StopPlayer();
            if (cmd is "c")
                isActive = false;

        }
    }
}

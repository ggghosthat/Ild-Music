using Ild_Music_CORE.Models.Core.Session_Structure;
using ShareInstances.PlayerResources;
using System;
using System.Collections.Generic;

namespace IldMusicCore_Test
{
    class Program
    {
        static NAudioPlayer _player = new();
        static bool isActive = true;

        static Track tr1 = new("E://gaga/start/music/2_5233518075101315631.mp3", "1");
        static Track tr2 = new("E://gaga/start/music/2_5235868813781631546.mp3", "2");
        static Track tr3 = new("E://gaga/start/music/2_5251668997736235620.mp3", "3");

        static Playlist playlist = new(new List<Track>() { tr1, tr2, tr3 });

        static void Main(string[] args)
        {
            playlist.Order();
            InitPlayer();

            while (isActive)
            {
                CommandProcesser();
            }
        }

        private static void InitPlayer() => _player.SetPlaylistInstance(playlist, 0);

        private static void CommandProcesser()
        {
            Console.WriteLine("Type command : ");
            var cmd = Console.ReadLine();

            if (cmd is "play")
                _player.Play();
            if (cmd is "stop")
                _player.StopPlayer();
            if (cmd is "toggle")
                _player.Pause_ResumePlayer();
            if (cmd is "c")
                isActive = false;
            if (cmd == "name")
                Console.WriteLine(_player.CurrentTrack.Name);
            if (cmd is "next")
                _player.DropNext();
            if (cmd is "prev")
                _player.DropPrevious();

        }
    }
}

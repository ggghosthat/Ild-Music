using Ild_Music.Core.Instances;
using NAudioPlayerCore.Models;

public static class Program
{
    private static NAudioPlayer _player = new();
    public static void Main(string[] args)
    {
        var track = new Track(Guid.NewGuid(), "E:/repos/Ild-Music/PlayerTest/nsty.mp3".AsMemory(), "1".AsMemory(), "".AsMemory(), new byte[0].AsMemory(), TimeSpan.FromSeconds(1200), 200);
        _player.DropTrack(track);
        var cmd = Console.ReadLine();

        if (cmd == "toggle")
            _player.Toggle();
    }
}
using IldMusic.VlcPlayer;
using ShareInstances;
using ShareInstances.Instances;

using System;
using System.Threading.Tasks;
namespace App;
public class Program12
{

    private static VlcPlayer player = new VlcPlayer();

    public static async Task Main12(string[] args)
    {
         
        Track tr1 = new Track("/home/jake/Music/pumped_up_kicks.mp3".AsMemory(),
                              "Pumped Up Kicks".AsMemory(),
                              "".AsMemory(),
                              new byte[0],
                              TimeSpan.FromSeconds(180),
                              2009);
        Track tr2 = new Track("/home/jake/Music/a_lot.mp3".AsMemory(),
                              "A lot".AsMemory(),
                              "".AsMemory(),
                              new byte[0],
                              TimeSpan.FromSeconds(360),
                              2009);
        Track tr3 = new Track("/home/jake/Music/t-yCg-0-baE.mp3".AsMemory(),
                              "my life be like".AsMemory(),
                              "".AsMemory(),
                              new byte[0],
                              TimeSpan.FromSeconds(360),
                              2009);
        
        var playlist = new Playlist("zero".AsMemory(),
                                    "".AsMemory(),
                                    new byte[0],
                                    2009);
        playlist.AddTrack(ref tr1);
        playlist.AddTrack(ref tr2);
        playlist.AddTrack(ref tr3);
        
        player.IsPlaylistLoop = true;
        await player.DropPlaylist(playlist);
        //await player.DropTrack(tr2);
        Console.WriteLine("There we go!");
        while (true)
        {
            string input = Console.ReadLine();
            Execute(input);
        }
    }

    public static async void Execute(string cmd)
    {
        switch (cmd)
        {
            case ("toggle"):
                player.Toggle();
                break;
            case ("prev"):
                player.SkipPrev();
                break;
            case ("next"):
                player.SkipNext();
                break;
            default: 
                return;
        }
    }
}
